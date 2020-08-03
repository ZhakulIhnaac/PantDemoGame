using Assets.Scripts.Classes.Gameplay;
using Assets.Scripts.Constants;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Classes.Playables
{
    public class Barrack : Building, IBarrack
    {
        /* Events */
        public static event Action<List<GameObject>> UpdateProductionButtons;

        [SerializeField] private LayerMask _playableLayerMask;
        public List<GameObject> ProductionButtonList;
        private Vector2 _initialSpawnPosition;

        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            AudioSource.clip = CreatedSound;
            AudioSource.PlayOneShot(CreatedSound);
            name = "Barrack";
        }

        private void Start()
        {
            _initialSpawnPosition = (Vector2)transform.position + new Vector2((PlayableSize.x / 2) + GameController.GridSystem.NodeRadius, -((PlayableSize.y / 2) + GameController.GridSystem.NodeRadius));
        }

        /*
         
         */
        public void Produce(GameObject unitToProduce)
        {
            
            /* Define a direction vector do show the spawn location check direction.
             Since the spawns will wrap the spawner on clockwise direction and
             start point is the node below the bottom right point of the spawner,
             initial direction is left and code is checking for space in right for rotating.*/
            var direction = Vector2.left * GameController.GridSystem.NodeRadius * 2;
            var completedTurns = 0; // To widen the search for non-obstructed node area, turns taken around the building are used as multiplier.
            var positionFound = false; // Flag value checking if the current node is available.
            var spawnPosition = _initialSpawnPosition; // Spawn position to check for availability.
            var trialCounter = 0; // This counter prevents Unity to stuck in while loop for positionFound.
            const int trialLimit = 3;

            while (!positionFound)
            {
                trialCounter++;
                spawnPosition = _initialSpawnPosition + (Vector2.down + Vector2.right) * (completedTurns);
                var sideStepList = new List<float> { PlayableSize.x + 1 + (completedTurns) * 2, PlayableSize.y + 1 + (completedTurns) * 2, PlayableSize.x + 1 + (completedTurns) * 2, PlayableSize.y + 1 + (completedTurns) * 2 }; // List of number of steps the search algorithm needs to take on four sides of the building.
                while (sideStepList.Count > 0)
                {
                    for (var step = 0; step < sideStepList[0]; step++)
                    {
                        var spawnNode = GameController.GridSystem.NodeFromWorldPosition(spawnPosition);

                        if (spawnNode != null && !spawnNode.IsObstructed && !Physics2D.OverlapCircle(spawnPosition, GameController.GridSystem.NodeRadius, _playableLayerMask))
                        {
                            positionFound = true;
                            break;
                        }

                        spawnPosition += direction;
                    }

                    if (positionFound)
                    {
                        break;
                    }

                    direction = RotateVector(direction, -90f);
                    sideStepList.RemoveAt(0);
                }

                if (trialCounter >= trialLimit)
                {
                    break;
                }

                completedTurns++;
            }

            if (positionFound)
            {
                GameController.ObjectPooling.SpawnFromPool(unitToProduce, spawnPosition, Quaternion.identity);
            }
            else
            {
                SendWarningMessage(InGameDictionary.NoSuitableSpawnPlace);
            }

        }

        /*
         ShowProducibles method lists the production buttons in production menu.
         */
        public void ShowProducibles()
        {
            UpdateProductionButtons?.Invoke(ProductionButtonList);
        }

        /*
         Selected method is triggered when the barrack is selected.
         */
        public override void Selected()
        {
            ShowProducibles();
        }

        /*
         RotateVector rotates the given vector for given angle.
         */
        public Vector2 RotateVector(Vector2 pVector, float angle)
        {
            var radian = angle * Mathf.Deg2Rad;
            var x = pVector.x * Mathf.Cos(radian) - pVector.y * Mathf.Sin(radian);
            var y = pVector.x * Mathf.Sin(radian) + pVector.y * Mathf.Cos(radian);
            return new Vector2(x, y);
        }
    }
}

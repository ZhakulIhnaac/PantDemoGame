using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Classes.Gameplay;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class Barrack : Building, IBarrack
    {
        [SerializeField] private List<GameObject> _productionButtonList;
        [SerializeField] private LayerMask _playableLayerMask;
        private Vector2 initialSpawnPosition;

        void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            AudioSource.clip = BuildSound;
            AudioSource.PlayOneShot(BuildSound);
            name = "Barrack";
        }

        void Start()
        {
            initialSpawnPosition = (Vector2)transform.position + new Vector2((PlayableSize.x / 2) - GameController.GridSystem.NodeRadius, -((PlayableSize.y / 2) + GameController.GridSystem.NodeRadius));
        }


        public void Produce(GameObject unitToProduce)
        {
            var spawnPosition = initialSpawnPosition;
            var direction = Vector2.left * GameController.GridSystem.NodeRadius * 2;

            /* Define a direction vector do show the spawn location check direction.
             Since the spawns will wrap the spawner on clockwise direction and
             start point is the node below the bottom right point of the spawner,
             initial direction is left and code is checking for space in right for rotating.*/

            var turns = 1;
            var step = 0;
            var placeFound = false;

            while (!placeFound)
            {
                step++;

                if (GameController.GridSystem.NodeFromWorldPosition(spawnPosition) != null) // There is a node on spawn position
                {
                    if (GameController.GridSystem.NodeFromWorldPosition(spawnPosition).IsObstructed) // Is the node obstructed
                    {

                        if (step % (2 * (int)PlayableSize.x + 2 * (int)PlayableSize.y + 4 + (turns - 1) * 8) == 0) // Next turn around the building.
                        {
                            Debug.Log("NewTurn!");
                            turns++;
                        }
                        else
                        {
                            if (Physics2D.OverlapCircle(spawnPosition + (turns + 1) * RotateVector(direction, -90f), GameController.GridSystem.NodeRadius, _playableLayerMask) != null) // Is there a playable object on right
                            {
                                if (Physics2D.OverlapCircle(spawnPosition + (turns + 1) * RotateVector(direction, -90f), GameController.GridSystem.NodeRadius, _playableLayerMask).gameObject.GetComponent<Unit>() != null) // Is that a unit
                                {
                                    direction = RotateVector(direction, -90f);
                                }
                            }
                            else
                            {
                                direction = RotateVector(direction, -90f);
                            }
                        }

                    }
                    else
                    {
                        placeFound = true;
                        break;
                    }

                }

                    spawnPosition += direction;


                if (step > 100)
                {
                    break;
                }
            }

            GameController.ObjectPooling.SpawnFromPool(unitToProduce, GameController.GridSystem.NodeFromWorldPosition(spawnPosition).WorldPosition, Quaternion.identity);

        }

        public void ShowProducibles()
        {
            UnitsMenuContent.UnitsMenuContentInstance.DisplayProducibles(_productionButtonList);
        }

        public override void Selected()
        {
            ShowProducibles();
        }

        public Vector2 RotateVector(Vector2 v, float angle)
        {
            float radian = angle * Mathf.Deg2Rad;
            float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
            float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
            return new Vector2(_x, _y);
        }
    }
}

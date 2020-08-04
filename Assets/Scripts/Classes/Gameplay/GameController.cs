using System;
using System.Collections.Generic;
using Assets.Scripts.Classes.GamePlay;
using Assets.Scripts.Classes.Playables;
using Assets.Scripts.Classes.UI;
using Assets.Scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Classes.Gameplay
{
    public class GameController : MonoBehaviour, IMainGame
    {
        public static event Action<float> UpdatePowerText;
        public static event Action<GameObject> UpdateSelectedGameObject;
        public static event Action<List<GameObject>> UpdateProduciblesList;
        public static GameController Instance;
        public static AStarPathfinding AStarPathfinding;
        public static ObjectPooling ObjectPooling;
        public static GridSystem GridSystem;
        public static GuiController GuiController;
        public float PowerAmount;
        public GameObject SelectedGameObject;

        private void Awake()
        {
            if (Instance == null) // Singleton.
            {
                Instance = this;
            }
        }

        private void Start()
        {
            /*Singleton assignments*/
            AStarPathfinding = AStarPathfinding.Instance;
            ObjectPooling = ObjectPooling.Instance;
            GridSystem = GridSystem.Instance;
            GuiController = GuiController.Instance;

            /*Event subscriptions*/
            PowerPlant.ProducePower += OnPowerProduce; // PowerPlant's ProducePower is an event send by the power plant buildings.
            ObjectPooling.ReducePowerAmount += OnPowerAmountReduce; // PowerPlant's ProducePower is an event send by the power plant buildings.
            
        }

        /*
        If a power plant produces power, subscribed method OnPowerProduce adds it to the power amount in
        hand and publishes the PowerAmount for UpdatePowerText subscribers the power amount text.
         */
        public void OnPowerProduce(float pProduceAmount)
        {
            PowerAmount += pProduceAmount;
            UpdatePowerText?.Invoke(PowerAmount);
        }

        /*
        If the player uses resource, subscribed method OnPowerAmountReduce subtracts the used amount from the
        power amount at hand and publishes the PowerAmount for UpdatePowerText subscribers the power amount text.
         */
        public void OnPowerAmountReduce(float pReduceAmount) // If a power plant produces power, add it to the power amount in hand and update the power amount text.
        {
            PowerAmount -= pReduceAmount;
            UpdatePowerText?.Invoke(PowerAmount);
        }

        /*
         Since there is no destruction in the game, especially for GameController, this method is dummy and only
         been put to complete "event" logic. It unsubscribes from the PowerPlant's ProducePower event.
         */
        private void OnDestroy()
        {
            PowerPlant.ProducePower -= OnPowerProduce;
        }

        /*
         If a game object is clicked, it is assigned to the GameController Instance's
         SelectedGameObject after the previous selected game object is unassigned
         from the variable.
         */
        public void AssignNewSelected(RaycastHit2D pHitObject)
        {
            EmptyTheSelected();
            Instance.SelectedGameObject = pHitObject.collider.gameObject; // Add the hit playable object as selected game object.
            Instance.SelectedGameObject.GetComponent<Playable>().Selected(); // Since the selected object needs to be a playable object, there is no need to check the existence of component "Playable".
            UpdateSelectedGameObject?.Invoke(Instance.SelectedGameObject);
        }

        /*
         Removes the object assignment from the selected game object.
         */
        public void EmptyTheSelected()
        {
            Instance.SelectedGameObject = null;
            UpdateProduciblesList?.Invoke(null);// When the SelectedGameObject changes, the producibles menu on the right hand side of the GUI changes ass well based on what the new selected game object can produce. 
            UpdateSelectedGameObject?.Invoke(null);
        }

    }
}

using System;
using System.Collections.Generic;
using Assets.Scripts.Classes.Gameplay;
using Assets.Scripts.Classes.GamePlay;
using Assets.Scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Classes
{
    public class GameController : MonoBehaviour, IMainGame
    {
        public static event Action<string> GiveWarning;
        public static GameController Instance;
        public static AStarPathfinding AStarPathfinding;
        public static ObjectPooling ObjectPooling;
        public static GridSystem GridSystem;
        public static GuiController GuiController;
        public TextMeshProUGUI powerAmountText;
        public TextMeshProUGUI selectedGameObjectName;
        public GameObject selectedGameObject;
        public RawImage selectedGameObjectImage;
        public float powerAmount;

        void Awake()
        {
            if (Instance == null) // We will only have one MainGame object in out scene. Thus, we just make it unique (Singleton)
            {
                Instance = this;
            }
        }

        void Start()
        {
            AStarPathfinding = GetComponent<AStarPathfinding>();
            ObjectPooling = GetComponent<ObjectPooling>();
            GridSystem = GetComponent<GridSystem>();
            GuiController = GetComponent<GuiController>();
            PowerPlant.ProducePower += OnPowerProduce;
        }

        public void OnPowerProduce(float produceAmount)
        {
            powerAmount += produceAmount;
            UpdatePowerAmount();
        }

        public void UpdatePowerAmount()
        {
            powerAmountText.text = "Power Amount: " + powerAmount;
        }

        private void OnDestroy()
        {
            PowerPlant.ProducePower -= OnPowerProduce;
        }

        public void AssignNewSelected(RaycastHit2D hitObject)
        {
            EmptyTheSelected();
            selectedGameObject = hitObject.collider.gameObject; // Add the hit playable object as selected game object.
            selectedGameObject.GetComponent<Playable>().Selected(); // Since the selected object needs to be a playable object, there is no need to check the existance of component "Playable".
            selectedGameObjectImage.texture = selectedGameObject.GetComponent<SpriteRenderer>().sprite.texture;
            selectedGameObjectName.text = selectedGameObject.name;
        }

        public void EmptyTheSelected()
        {
            selectedGameObject = null;
            selectedGameObjectImage.texture = null;
            selectedGameObjectName.text = null;
            UnitsMenuContent.UnitsMenuContentInstance.DisplayProducibles();
        }

    }
}

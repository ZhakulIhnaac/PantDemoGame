using System.Xml;
using Assets.Scripts.Classes.Gameplay;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Classes
{
    public class Cursor : MonoBehaviour, ICursor
    {
        public EventSystem EventSystem;
        public static Cursor CursorInstance;
        public GameObject BuildingTemplate; // The building template which will be assigned on clicking the GUI button for new building.
        public GameObject TileTheCursorIsOn; // Ground tile which will be used as a base on grid based system on movements and builds.
        [SerializeField] private LayerMask _playableLayerMask;
        [SerializeField] private LayerMask _groundLayerMask;

        void Awake()
        {
            if (CursorInstance == null) // We will only have one MainGame object in out scene. Thus, we just make it unique (Singleton).
            {
                CursorInstance = this;
            }
        }

        private void Start()
        {
            EventSystem = EventSystem.current;
        }

        private void Update()
        {
            if (BuildingTemplate != null)
            {
                var groundTileCheckMouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                var groundTileCheck = Physics2D.Raycast(groundTileCheckMouseRay.origin, groundTileCheckMouseRay.direction, Mathf.Infinity, _groundLayerMask);
                if (groundTileCheck.collider != null)
                {
                    TileTheCursorIsOn = groundTileCheck.collider.gameObject;
                }
            }

            if (Input.anyKey) // To prevent the program to check every if loop for each Input.Key, all the button checks wrapped with this if condition.
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject()) // Check if the click is on UI.
                    {
                        return;
                    }

                    if (BuildingTemplate != null) // If there is a building template in hand...
                    {
                        BuildingTemplate.GetComponent<BuildingTemplate>().LeftMouseClick(); // Build on the clicked place.
                    }

                    else
                    {
                        var mouseRayToSend = Camera.main.ScreenPointToRay(Input.mousePosition);
                        var checkPlayableObjectHit = Physics2D.Raycast(mouseRayToSend.origin, mouseRayToSend.direction, Mathf.Infinity, _playableLayerMask);

                        if (checkPlayableObjectHit.collider != null) // If the ray hit anything on playableLayer...
                        {
                            GameController.Instance.AssignNewSelected(checkPlayableObjectHit); // Assign the hit object to selected object in main game.
                        }

                        else if (GameController.Instance.SelectedGameObject != null) // If there is a selected object in hand...
                        {
                            GameController.Instance.SelectedGameObject.GetComponent<Playable>().LeftMouseClick(); // Use left click method inside the selected game object. Selected game objects are taken from PlayableLayer, thus they will all inherit from IInteractable and will contain leftMouseClick.
                        }

                    }

                }

                else if (Input.GetMouseButtonDown(1))
                {
                    if (BuildingTemplate != null)
                    {
                        Destroy(BuildingTemplate); // Remove building template from selection.
                    }

                    else if (GameController.Instance.SelectedGameObject != null)
                    {
                        if (GameController.Instance.SelectedGameObject.GetComponent<Playable>() != null) // If a unit is selected...
                        {
                            GameController.Instance.SelectedGameObject.GetComponent<Playable>().RightMouseClick(); // Use right click method inside the selected game object. Selected game objects are taken from PlayableLayer, thus they will all inherit from IInteractable and will contain leftMouseClick.
                        }
                    }
                }
            }

        }

    }
}

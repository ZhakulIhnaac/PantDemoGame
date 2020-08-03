using System;
using Assets.Scripts.Classes.Gameplay;
using Assets.Scripts.Constants;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class BuildingTemplate : MonoBehaviour, IBuildingTemplate, IInteractable
    {
        public static event Action<string> GiveWarning;
        [SerializeField] private GameObject _objectToPlace;
        private int _numberOfNodesUnderTheTemplate;
        private bool _canPlaceBuilding;
        private float _xPosToAdd;
        private float _yPosToAdd;
        public Vector2 TemplateSize => (Vector2)gameObject.GetComponent<SpriteRenderer>().bounds.size;

        void Start()
        {
            _canPlaceBuilding = true;
            gameObject.AddComponent<AudioSource>();
            _xPosToAdd = ((GetComponent<BoxCollider2D>().size.x + 1) % 2) * GameController.GridSystem.NodeRadius; // Since this is a grid-based game and different buildings can have different sizes (4x4 and 3x2 etc.), vertical and horizontal sizes of the buildings must be taken into consideration.
            _yPosToAdd = ((GetComponent<BoxCollider2D>().size.y + 1) % 2) * GameController.GridSystem.NodeRadius; // If a side length (width or height) is even, then it will fall into the middle of the selected node and radius of the node must be added to alignment. Such case is not binding for odd lengths. They will already fall onto the node edge.
        }

        private void FixedUpdate()
        {
            if (Cursor.CursorInstance.TileTheCursorIsOn != null)
            {
                gameObject.transform.position = new Vector3(Cursor.CursorInstance.TileTheCursorIsOn.transform.position.x + _xPosToAdd, Cursor.CursorInstance.TileTheCursorIsOn.transform.position.y - _yPosToAdd); // Align the building center with tile.
            }

            // Check for ground availability
            Vector2 objBottomLeftPoint = (Vector2)transform.position + Vector2.left * TemplateSize.x / 2 + Vector2.down * TemplateSize.y / 2; // transform.position will give the middle point and we will subtract the halves of the height and width to find bottom left point
            _numberOfNodesUnderTheTemplate = 0;
            for (int i = 0; i < TemplateSize.x; i++)
            {
                for (int j = 0; j < TemplateSize.y; j++)
                {
                    var node = GameController.GridSystem.NodeFromWorldPosition(
                        objBottomLeftPoint + new Vector2(i * GameController.GridSystem.NodeDiameter,
                            j * GameController.GridSystem.NodeDiameter));

                    if (node != null && !node.IsObstructed)
                    {
                        _numberOfNodesUnderTheTemplate++;
                    }
                }
            }

            if (_numberOfNodesUnderTheTemplate == (int)TemplateSize.x * (int)TemplateSize.y)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                _canPlaceBuilding = true;
            }
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                _canPlaceBuilding = false;
            }

        }

        public void PlaceBuilding()
        {
            if (_canPlaceBuilding)
            {
                GameController.ObjectPooling.SpawnFromPool(_objectToPlace, transform.position, Quaternion.identity, gameObject);
            }
            else
            {
                GiveWarning?.Invoke(InGameDictionary.InappropriateBuildingPlacementWarning);
            }
        }

        public void LeftMouseClick()
        {
            PlaceBuilding();
        }

        public void RightMouseClick()
        {
            Destroy(gameObject);
        }

    }
}

using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class Unit : Playable, IBuilding
    {
        private List<Node> _pathToFollow;
        private Node _targetNode;
        [SerializeField] private float _stoppingDistance;
        [SerializeField] private float _movingSpeed;
        [SerializeField] private LayerMask _groundLayerMask;

        void Start()
        {
            _pathToFollow = new List<Node>();
        }

        void Update()
        {
            if (_pathToFollow.Count > 0 || _targetNode != null) // If there is a path to follow or a next target...
            {
                if (_pathToFollow.Count > 0 && _pathToFollow[0].IsObstructed)
                {
                    Move(_pathToFollow[_pathToFollow.Count - 1].WorldPosition); // The route is obstructed, find a new route.
                }

                else
                {
                    if (_targetNode == null) // If there is a path, but there is no next target...
                    {
                        _targetNode = _pathToFollow[0]; // The new target will be the node at the beginning of _pathToFollow list.
                        _targetNode.IsObstructed = true; // Obstruct the next-step node.
                        _pathToFollow.Remove(_pathToFollow[0]); // Also, the new target will be subtracted from the path list.
                    }

                    if (((Vector2)transform.position - _targetNode.WorldPosition).magnitude > _stoppingDistance) // if there is still way to go...
                    {

                        transform.position = Vector2.MoveTowards((Vector2)transform.position, _targetNode.WorldPosition, _movingSpeed * Time.deltaTime);
                    }
                    else // If the soldier has arrived the node...
                    {
                        if (_pathToFollow.Count != 0) // If this is the destination node...
                        {
                            _targetNode.IsObstructed = false; // Obstruct back the destination node.
                        }

                        _targetNode = null; // Remove the target.
                    }
                }
            }
        }

        public override void LeftMouseClick()
        {
            GameController.Instance.EmptyTheSelected();
        }

        public override void RightMouseClick()
        {
            Ray mouseRayToSend = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D checkGroundHit = Physics2D.Raycast(mouseRayToSend.origin, mouseRayToSend.direction, Mathf.Infinity, _groundLayerMask);

            if (checkGroundHit.collider != null)
            {
                //GridSystem.GridInstance.NodeFromWorldPosition();
                Move(checkGroundHit.collider.gameObject.transform.position);
            }
        }

        public void Move(Vector2 moveTarget)
        {
            var path = GameController.AStarPathfinding.FindPath(transform.position, moveTarget);
            if (path == null)
            {
                SendWarningMessage("Cannot go there!");
            }
            else
            {
                var movementStartNode = GameController.GridSystem.NodeFromWorldPosition(transform.position);
                movementStartNode.IsObstructed = false;
                _pathToFollow = path;
            }
        }
    }
}

using System.Collections.Generic;
using Assets.Scripts.Classes.Gameplay;
using Assets.Scripts.Constants;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes.Playables
{
    public class Unit : Playable, IBuilding
    {
        private List<Node> _pathToFollow;
        public AudioClip MoveSound;
        [SerializeField] private float _stoppingDistance;
        [SerializeField] private float _movingSpeed;
        [SerializeField] private LayerMask _groundLayerMask;

        private void Start()
        {
            _pathToFollow = new List<Node>();
        }

        private void Update()
        {
            if (_pathToFollow.Count > 0) // If there is a path to follow...
            {
                if (_pathToFollow[0].IsObstructed) // If the next step is obstructed...
                {
                    Move(_pathToFollow[_pathToFollow.Count - 1].WorldPosition); // Find a new route.
                }

                else // If the next step is not obstructed...
                {
                    if (((Vector2)transform.position - _pathToFollow[0].WorldPosition).magnitude > _stoppingDistance) // If there is still way to go...
                    {
                        transform.position = Vector2.MoveTowards((Vector2)transform.position, _pathToFollow[0].WorldPosition, _movingSpeed * Time.deltaTime); // Move towards the target.
                    }
                    else // If the soldier has arrived the node...
                    {
                        _pathToFollow[0].IsObstructed = true; // Obstruct the next-step node.
                        _pathToFollow.Remove(_pathToFollow[0]); // Also, the new target will be subtracted from the path list.
                    }
                }
            }
        }

        /*
         LeftMouseClick is triggered when left mouse button is clicked while the unit is selected.
         */
        public override void LeftMouseClick()
        {
            GameController.Instance.EmptyTheSelected();
        }

        /*
         RightMouseClick is triggered when right mouse button is clicked while the unit is selected.
         */
        public override void RightMouseClick()
        {
            var mouseRayToSend = Camera.main.ScreenPointToRay(Input.mousePosition);
            var checkGroundHit = Physics2D.Raycast(mouseRayToSend.origin, mouseRayToSend.direction, Mathf.Infinity, _groundLayerMask);

            if (checkGroundHit.collider != null)
            {
                Move(checkGroundHit.collider.gameObject.transform.position);
            }
        }

        /*
         Move order asks for a path from AStarPathfinding's FindPath method, then orders the unit
         to move to the next node on the returned list. If the returned path is null, method indicates
         that the unit cannot move to the target position.
         */
        public void Move(Vector2 moveTarget)
        {
            AudioSource.clip = MoveSound;
            AudioSource.PlayOneShot(MoveSound);

            var path = GameController.AStarPathfinding.FindPath(transform.position, moveTarget);
            if (path == null)
            {
                SendWarningMessage(InGameDictionary.NoPathFoundWarning);
                _pathToFollow.Clear();
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

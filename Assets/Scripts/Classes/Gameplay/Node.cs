using UnityEngine;

namespace Assets.Scripts.Classes.Gameplay
{
    public class Node
    {
        public int XPosInGrid; // X position in the node matrix.
        public int YPosInGrid; // Y position in the node matrix.
        public bool IsObstructed; // Is the node obstructed.
        public Vector2 WorldPosition; // World position of the node.
        public Node ParentNode; // Parent node, required for tracing back the shortest path.
        public int GCost; // Cost of moving to this node from the starting node.
        public int HCost; // Manhattan distance to the target node.
        public int FCost => GCost + HCost; // Total cost of the node.

        public Node(bool pIsObstructed, Vector3 pWorldPosition, int pXPosInGrid, int pYPosInGrid)
        {
            IsObstructed = pIsObstructed;
            WorldPosition = pWorldPosition;
            XPosInGrid = pXPosInGrid;
            YPosInGrid = pYPosInGrid;
        }
    }
}

using UnityEngine;

public class Node
{
    public int XPosInGrid; // X position in the node array
    public int YPosInGrid; // Y position in the node array
    public bool IsObstructed; // Is the node obstructed
    public Vector2 WorldPosition; // World position of the node
    public Node ParentNode; // Previous node to trace the shortest path
    public int GCost; // Cost of moving to this node
    public int HCost; // Manhattan distance to the goal
    public int FCost => GCost + HCost; // fCost calculation

    public Node(bool pIsObstructed, Vector3 pWorldPosition, int pXPosInGrid, int pYPosInGrid) // Constructor
    {
        IsObstructed = pIsObstructed;
        WorldPosition = pWorldPosition;
        XPosInGrid = pXPosInGrid;
        YPosInGrid = pYPosInGrid;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Classes.Gameplay;
using UnityEngine;

namespace Assets.Scripts.Classes.GamePlay
{
    //TODO: No path found olayı biraz daha optimize yazılabilir. Finish node etrafındaki bütün node'lar obstructed olmalı.
    public class AStarPathfinding : MonoBehaviour
    {

        /* A* ALGORITHM Steps
        G: Movement cost from starting point to current node.
        H: Manhattan distance between the current node and end node.
        F: Total score of the node, G + H.

         1. Get the square on the open list which has the lowest score. Let’s call this square S.
         2. Remove S from the open list and add S to the closed list.
         3. For each square T in S’s walkable adjacent tiles:
            - If T is in the closed list: Ignore it.
            - If T is not in the open list: Add it and compute its score.
            - If T is already in the open list: Check if the F score is lower when we use the current
            generated path to get there. If it is, update its score and update its parent as well.
         */

        public static AStarPathfinding Instance;
        private GridSystem _grid;

        void Awake()
        {
            if (Instance == null) // We will only have one MainGame object in out scene. Thus, we just make it unique (Singleton)
            {
                Instance = this;
            }
        }

        public List<Node> FindPath(Vector2 pStartPos, Vector2 pTargetPos) // This method will be triggered each time a moveable game object is supposed to go to a_TargetPos from a_StartPos.
        {
            _grid = gameObject.GetComponent<GridSystem>();
            Node startNode = _grid.NodeFromWorldPosition(pStartPos); // Get the starting position node.
            Node targetNode = _grid.NodeFromWorldPosition(pTargetPos); // Get the target position node.
            
            if (startNode != null && targetNode != null)
            {
                List<Node> OpenList = new List<Node>(); // This list will contain the nodes we will search on.
                HashSet<Node> ClosedList = new HashSet<Node>(); // HashSet is used for closed list since the items of closed list will not be checked.

                startNode.HCost = GetManhattanDistance(startNode, targetNode);
                OpenList.Add(startNode); // Initial step of A* algorithm, add the starting node to the observing list in order to give a start point to the algoritm.

                while (OpenList.Count > 0)
                {
                    // Step 1: Select the node with the lowest cost.
                    Node currentNode = OpenList.OrderBy(x => x.FCost).First();
                    if (currentNode == targetNode)
                    {
                        return GetFinalPath(startNode, targetNode);
                    }

                    // Step 2: Remove current node from open list and add it to the closed list.
                    OpenList.Remove(currentNode);
                    ClosedList.Add(currentNode);

                    // Step 3: Find adjacent walkable nodes and calculate the moveabiliy.
                    foreach (Node neighbourNode in _grid.GetNeighbourNodes(currentNode))
                    {
                        if (neighbourNode.IsObstructed || ClosedList.Contains(neighbourNode)) // If neighbour is in the closed list or obstructed (aka. immoveable): Ignore it.
                        {
                            continue;
                        }

                        if (!OpenList.Contains(neighbourNode)) //If neighbour is not in the open list, add it and compute its score.
                        {

                            neighbourNode.GCost = currentNode.GCost + GetManhattanDistance(currentNode, neighbourNode); // We don't add simply 1 since diagonal movement can ve activated.
                            neighbourNode.HCost = GetManhattanDistance(neighbourNode, targetNode);
                            neighbourNode.ParentNode = currentNode;
                            OpenList.Add(neighbourNode);

                        }
                        else // The neighbour node is in the open list.
                        {
                            if (neighbourNode.GCost > currentNode.GCost + GetManhattanDistance(currentNode, neighbourNode))
                            {
                                neighbourNode.GCost = currentNode.GCost + GetManhattanDistance(currentNode, neighbourNode); // If the neighbour node can be reached from the current node with a lower cost than the cost neigbour node already has (FCost), update GCost (thus the FCost).
                                neighbourNode.ParentNode = currentNode;
                            }
                        }
                    }
                }
            }

            return null; // if the function returns null, that means there is no suitable path found. While loop already returns the path if it is found.
                         // TODO: Herhangi bir path olmaması durumunda her node bakılacağı için performans ölebilir. Bunu düzeltmek için her iterasyon sonunda target node'un bütün komşuları immoveable ise no suitable path döndür denebilir.

        }

        private int GetManhattanDistance(Node a_firstNode, Node a_secondNode)
        {
            int xDistance = Mathf.Abs(a_firstNode.XPosInGrid - a_secondNode.XPosInGrid);
            int yDistance = Mathf.Abs(a_firstNode.YPosInGrid - a_secondNode.YPosInGrid);

            return xDistance + yDistance;
        }

        private List<Node> GetFinalPath(Node pStartNode, Node pEndNode)
        {
            List<Node> FinalPath = new List<Node>();
            Node CurrentNode = pEndNode;

            while (CurrentNode != pStartNode)
            {
                FinalPath.Add(CurrentNode);
                CurrentNode = CurrentNode.ParentNode;
            }

            FinalPath.Reverse();

            return FinalPath;
        }
    }
}

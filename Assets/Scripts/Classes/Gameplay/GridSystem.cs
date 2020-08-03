using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Assets.Scripts.Classes.Gameplay
{
    public class GridSystem : MonoBehaviour
    {
        public static GridSystem Instance; // Singleton.
        public GameObject GridReferenceGround; // Ground where the grid will cover.
        public GameObject GridGround; // Tile the grid will place.
        public LayerMask PlayableLayer; // To detect the non-moveable grids, this layer will be used to define obstacles
        public Vector2 GridWorldSize; // World size of the grid object
        public float NodeRadius; // Mostly, the _nodeDiameter will be used, but radius will be useful while building the grid
        private Node[,] _grid; // The grid we will store the nodes inside
        public float NodeDiameter;
        private int _gridSizeX;
        private int _gridSizeY;

        private void Awake()
        {
            if (Instance == null) // We will only have one MainGame object in out scene. Thus, we just make the grid unique (Singleton)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            NodeDiameter = 2 * NodeRadius;
            _gridSizeX = Mathf.RoundToInt(GridWorldSize.x / NodeDiameter); // Find number of nodes on horizontal axis of the grid
            _gridSizeY = Mathf.RoundToInt(GridWorldSize.y / NodeDiameter); // Find number of nodes on vertical axis of the grid
            CreateGrid();
        }

        /*
         Initial method for GridSystem object. CreateGrid creates a rectangular grid
         by taking the GridGround property as a base. Number of nodes on the grid is
         given by GridWorldSize, both for horizontal and vertical axises.
         */
        public void CreateGrid()
        {
            _grid = new Node[_gridSizeX, _gridSizeY]; // Initializing the node array.
            Vector2 bottomLeftPoint = (Vector2)GridReferenceGround.transform.position - Vector2.right * GridWorldSize.x / 2 - Vector2.up * GridWorldSize.y / 2; // transform.position will give the middle point and we will subtract the halves of the height and width to find bottom left point
            for (var i = 0; i < _gridSizeX; i++) // Grid column count
            {
                for (var j = 0; j < _gridSizeY; j++) // Grid row count
                {
                    var worldPoint = bottomLeftPoint + Vector2.right * (i * NodeDiameter + NodeRadius) + Vector2.up * (j * NodeDiameter + NodeRadius);
                    bool obstacle = Physics2D.OverlapCircle(worldPoint, NodeRadius, PlayableLayer); // Checking for obstacle on node
                    Instantiate(GridGround, worldPoint, Quaternion.identity); // Instantiate the ground sprite at the same position as the node.
                    _grid[i, j] = new Node(obstacle, worldPoint, i, j);
                }
            }
        }

        /*
         NodeFromWorldPosition takes a coordinate in terms of world position, then by taking the left-bottom corner
         of the grid reference ground's world position, it finds out which node's area PWorldPosition falls in. 
         */
        public Node NodeFromWorldPosition(Vector2 pWorldPosition)
        {
            Vector2 bottomLeftPoint = (Vector2)GridReferenceGround.transform.position - Vector2.right * GridWorldSize.x / 2 - Vector2.up * GridWorldSize.y / 2; // Find the position of the bottom left point to use it as a reference point.
            var directionVector = pWorldPosition - bottomLeftPoint; // Point out the location of the given world position relative to the bottom left point.
            var xCoordinate = Mathf.FloorToInt(directionVector.x / NodeDiameter);
            var yCoordinate = Mathf.FloorToInt(directionVector.y / NodeDiameter);

            if (xCoordinate >= 0 && xCoordinate < _gridSizeX)
            {
                if (yCoordinate >= 0 && yCoordinate < _gridSizeY)
                {
                    return _grid[xCoordinate, yCoordinate];
                }
            }

            return null;
        }

        /*
         GetNeighbourNodes takes a node, pCurrentNode, and it checks 8 adjacent possible-node
         positions including the ones in the corners for existence of nodes. If there is a node
         in the checked position, it gets added into the neighbour nodes list. Lastly, this list
         is returned to the method caller.
         */
        public List<Node> GetNeighbourNodes(Node pCurrentNode)
        {
            var neighbouringNodes = new List<Node>();
            var checkEightSides = new List<Vector2>
            {
                Vector2.up,
                Vector2.up + Vector2.right,
                Vector2.right,
                Vector2.down + Vector2.right,
                Vector2.down,
                Vector2.down + Vector2.left,
                Vector2.left,
                Vector2.up + Vector2.left,
            };

            foreach (var side in checkEightSides)
            {
                var neighbourInquiry = CheckNeighbourNode(pCurrentNode, side);
                if (neighbourInquiry != null)
                {
                    neighbouringNodes.Add(neighbourInquiry);
                }
            }

            return neighbouringNodes;
        }

        /*
         CheckNeighbourNode checks if pPosition falls into a node's area and returns the node if it exists.
         */
        Node CheckNeighbourNode(Node pCurrentNode, Vector2 pPosition)
        {
            var checkPosX = pCurrentNode.XPosInGrid + (int)pPosition.x;
            var checkPosY = pCurrentNode.YPosInGrid + (int)pPosition.y;

            if (checkPosX >= 0 && checkPosX < _gridSizeX)
            {
                if (checkPosY >= 0 && checkPosY < _gridSizeY)
                {
                    return _grid[checkPosX, checkPosY];
                }
            }

            return null;
        }

        /*
         ObstructNode changes the status of the given node in terms of being obstructed or not.
         */
        void ObstructNode(Node node)
        {
            node.IsObstructed = true;
        }
    }
}

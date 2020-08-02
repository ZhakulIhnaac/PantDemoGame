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
        public float _nodeDiameter;
        private int _gridSizeX;
        private int _gridSizeY;

        void Awake()
        {
            if (Instance == null) // We will only have one MainGame object in out scene. Thus, we just make the grid unique (Singleton)
            {
                Instance = this;
            }
        }

        void Start()
        {
            _nodeDiameter = 2 * NodeRadius;
            _gridSizeX = Mathf.RoundToInt(GridWorldSize.x / _nodeDiameter); // Find number of nodes on horizontal axis of the grid
            _gridSizeY = Mathf.RoundToInt(GridWorldSize.y / _nodeDiameter); // Find number of nodes on vertical axis of the grid
            CreateGrid();
        }

        public void CreateGrid()
        {
            _grid = new Node[_gridSizeX, _gridSizeY]; // Initializing the node array.
            Vector2 bottomLeftPoint = (Vector2)GridReferenceGround.transform.position - Vector2.right * GridWorldSize.x / 2 - Vector2.up * GridWorldSize.y / 2; // transform.position will give the middle point and we will subtract the halves of the height and width to find bottom left point
            for (int i = 0; i < _gridSizeX; i++) // Grid column count
            {
                for (int j = 0; j < _gridSizeY; j++) // Grid row count
                {
                    Vector2 worldPoint = bottomLeftPoint + Vector2.right * (i * _nodeDiameter + NodeRadius) + Vector2.up * (j * _nodeDiameter + NodeRadius);
                    bool obstacle = Physics2D.OverlapCircle(worldPoint, NodeRadius, PlayableLayer); // Checking for obstacle on node
                    Instantiate(GridGround, worldPoint, Quaternion.identity); // Instantiate the ground sprite at the same position as the node.
                    _grid[i, j] = new Node(obstacle, worldPoint, i, j);
                }
            }
        }

        public Node NodeFromWorldPosition(Vector2 pWorldPosition)
        {
            Vector2 bottomLeftPoint = (Vector2)GridReferenceGround.transform.position - Vector2.right * GridWorldSize.x / 2 - Vector2.up * GridWorldSize.y / 2; // Find the position of the bottom left point to use it as a reference point.
            var directionVector = pWorldPosition - bottomLeftPoint; // Point out the location of the given world position relative to the bottom left point.
            var xCoordinate = Mathf.FloorToInt(directionVector.x / _nodeDiameter);
            var yCoordinate = Mathf.FloorToInt(directionVector.y / _nodeDiameter);

            if (xCoordinate >= 0 && xCoordinate < _gridSizeX)
            {
                if (yCoordinate >= 0 && yCoordinate < _gridSizeY)
                {
                    return _grid[xCoordinate, yCoordinate];
                }
            }

            return null;
        }

        public List<Node> GetNeighbourNodes(Node pCurrentNode)
        {
            List<Node> NeighbouringNodes = new List<Node>();
            List<Vector2> checkEightSides = new List<Vector2>
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
                    NeighbouringNodes.Add(neighbourInquiry);
                }
            }

            return NeighbouringNodes;
        }

        Node CheckNeighbourNode(Node pCurrentNode, Vector2 position)
        {
            List<Node> NeighbouringNodes = new List<Node>();

            int checkPosX = pCurrentNode.XPosInGrid + (int)position.x;
            int checkPosY = pCurrentNode.YPosInGrid + (int)position.y;

            if (checkPosX >= 0 && checkPosX < _gridSizeX)
            {
                if (checkPosY >= 0 && checkPosY < _gridSizeY)
                {
                    return _grid[checkPosX, checkPosY];
                }
            }

            return null;
        }

        void ObstructNode(Node node)
        {
            node.IsObstructed = true;
        }
    }
}

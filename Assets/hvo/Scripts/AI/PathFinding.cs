using HVO.Scripts.Managers;
using UnityEngine;

namespace HVO.Scripts.AI
{
    public class PathFinding
    {
        private readonly TilemapManager _tilemapManager;
        private int _with;
        private int _height;
        private Vector3Int _gridOffset;

        public Node[,] Grid { get; private set; }

        public PathFinding(TilemapManager tilemapManager)
        {
            _tilemapManager = tilemapManager;

            GetCellBounds();
        }

        private void GetCellBounds()
        {
            _tilemapManager.PathFindingTilemap.CompressBounds();
            _gridOffset = _tilemapManager.PathFindingTilemap.cellBounds.min;

            var bounds = _tilemapManager.PathFindingTilemap.cellBounds;

            _with = bounds.size.x;
            _height = bounds.size.y;

            InitializeGrid();
        }

        private void InitializeGrid()
        {
            Grid = new Node[_with, _height];

            var cellSize = _tilemapManager.PathFindingTilemap.cellSize;

            for (int x = 0; x < _with; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var nodeLeftBottomPosition = new Vector3Int(x + _gridOffset.x, y + _gridOffset.y);
                    var isWalkable = _tilemapManager.TestCanWalkAtTile(nodeLeftBottomPosition);
                    var node = new Node(nodeLeftBottomPosition, cellSize, isWalkable);

                    Grid[x, y] = node;
                }
            }
        }

        public void FindPath(Vector3 startPosition, Vector3 endPosition)
        {
            Node startNode = FindNode(startPosition);
            Node endNode = FindNode(endPosition);

            Debug.Log("Start Node: " + startNode);
            Debug.Log("End Node: " + endNode);
        }

        private Node FindNode(Vector3 position)
        {
            Vector3Int flooredPosition = new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));

            int gridX = flooredPosition.x - _gridOffset.x;
            int gridY = flooredPosition.y - _gridOffset.y;

            if (gridX >= 0 && gridX < _with && gridY >= 0 && gridY < _height)
            {
                return Grid[gridX, gridY];
            }

            Debug.Log($"Node not found at position: {position}");
            return null;
        }
    }
}
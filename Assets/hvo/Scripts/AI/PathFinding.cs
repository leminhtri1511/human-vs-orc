using HVO.Scripts.Managers;
using UnityEngine;

namespace HVO.Scripts.AI
{
    public class PathFinding
    {
        private readonly TilemapManager _tilemapManager;
        private int _with;
        private int _height;

        public Node[,] Grid { get; private set; }

        public PathFinding(TilemapManager tilemapManager)
        {
            _tilemapManager = tilemapManager;

            GetCellBounds();
        }

        private void GetCellBounds()
        {
            _tilemapManager.PathFindingTilemap.CompressBounds();
            var bounds = _tilemapManager.PathFindingTilemap.cellBounds;

            _with = bounds.size.x;
            _height = bounds.size.y;

            InitializeGrid();
        }

        private void InitializeGrid()
        {
            var offset = _tilemapManager.PathFindingTilemap.cellBounds;
            var halfCellSize = _tilemapManager.PathFindingTilemap.cellSize / 2;


            Grid = new Node[_with, _height];

            for (int x = 0; x < _with; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var nodeLeftBottomPosition = new Vector3Int(x + offset.x, y + offset.y);
                    var nodeCenterPositon = nodeLeftBottomPosition + halfCellSize;
                    var isWalkable = _tilemapManager.TestCanWalkAtTile(nodeLeftBottomPosition);
                    var node = new Node(nodeCenterPositon.x, nodeCenterPositon.y, isWalkable);

                    Grid[x, y] = node;

                    if (!isWalkable)
                    {
                        Debug.Log($"NODE x: {x}, y: {y} | POS: Vt2({node.x}, {node.y}) | IsWalkable: {isWalkable}");
                    }
                }
            }
        }
    }
}
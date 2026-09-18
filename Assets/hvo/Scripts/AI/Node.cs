using UnityEngine;

namespace HVO.Scripts.AI
{
    public class Node
    {
        public int X;
        public int Y;
        public bool IsWalkable;
        public float CenterX;
        public float CenterY;

        public Node(Vector3Int position, Vector3 cellSize, bool isWalkable)
        {
            X = position.x;
            Y = position.y;

            var halfCellSize = cellSize / 2f;
            var nodeCenterPosition = position + halfCellSize;

            CenterX = nodeCenterPosition.x;
            CenterY = nodeCenterPosition.y;
            IsWalkable = isWalkable;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
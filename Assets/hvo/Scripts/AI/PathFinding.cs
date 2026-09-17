using UnityEngine;

namespace HVO.Scripts.AI
{
    public class PathFinding
    {
        private readonly Node[,] _grid;
        public Node[,] Grid => _grid;

        public PathFinding(int width, int height)
        {
            _grid = new Node[width, height];

            Debug.Log(_grid.Length);
            //21:45 -> 26:52
        }
    }
}
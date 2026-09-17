namespace HVO.Scripts.AI
{
    public class Node
    {
        public float x;
        public float y;
        public bool IsWalkable;

        public Node(float x, float y, bool isWalkable)
        {
            this.x = x;
            this.y = y;
            this.IsWalkable = isWalkable;
        }
    }
}
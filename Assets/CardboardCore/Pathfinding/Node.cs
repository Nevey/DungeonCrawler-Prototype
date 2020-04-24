using UnityEngine;

namespace CardboardCore.PathFinding
{
    public struct Node
    {
        public readonly int x;
        public readonly int y;
        public readonly int Weight;

        public Node(Vector2Int coords, int weight)
        {
            x = coords.x;
            y = coords.y;
            Weight = weight;
        }

        public static bool operator ==(Node a, Node b)
        {
            return a.x == b.x && a.y == b.y;
        }
        
        public static bool operator !=(Node a, Node b)
        {
            return a.x != b.x || a.y != b.y;
        }
    }
}
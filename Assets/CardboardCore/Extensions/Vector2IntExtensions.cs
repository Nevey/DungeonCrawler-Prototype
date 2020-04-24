using UnityEngine;

namespace CardboardCore.Extensions
{
    public static class Vector2IntExtensions
    {
        public static bool Contains(this Vector2Int[] vectors, int x, int y)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                if (vectors[i].x == x && vectors[i].y == y)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
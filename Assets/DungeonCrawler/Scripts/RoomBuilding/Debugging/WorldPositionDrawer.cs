using UnityEditor;
using UnityEngine;

namespace DungeonCrawler.RoomBuilding.Debugging
{
    public class WorldPositionDrawer : MonoBehaviour
    {
        public int x;
        public int y;

        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position;
            pos.y += 1f;

            // Handles.Label(pos, $"{transform.position.x} - {transform.position.z}");
            Handles.Label(pos, $"{x} - {y}");
        }
    }
}

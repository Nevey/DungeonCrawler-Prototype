using UnityEditor;
using UnityEngine;

namespace DungeonCrawler.RoomBuilding.Debugging
{
    public class WorldPositionDrawer : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position;
            pos.y += 1f;

            Handles.Label(pos, $"{transform.position.x} - {transform.position.z}");
        }
    }
}

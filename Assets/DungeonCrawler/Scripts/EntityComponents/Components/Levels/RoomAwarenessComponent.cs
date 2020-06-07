using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    public class RoomAwarenessComponent : Component
    {
        private RoomData currentRoomData;

        public RoomAwarenessComponent(Entity owner) : base(owner)
        {
        }

        public void EnterRoom(RoomData roomData)
        {
            currentRoomData = roomData;
        }

        public bool CanWalk(int currentX, int currentY, int targetX, int targetY)
        {
            return true;
        }
    }
}
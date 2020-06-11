using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public abstract class RoomBuilderState : State
    {
        /// <summary>
        /// Data of current room
        /// </summary>
        public RoomDataComponent currentRoom;

        /// <summary>
        /// Card data of room to spawn
        /// </summary>
        public RoomCardDataComponent roomCardDataComponent;

        public int x;
        public int y;
    }
}

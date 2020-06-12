using System.Collections.Generic;
using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class RoomRegistryComponent : Component
    {
        public List<RoomDataComponent> rooms { get; private set; }

        protected override void OnStart()
        {
            rooms = new List<RoomDataComponent>();
        }

        public void Add(RoomDataComponent roomDataComponent)
        {
            rooms.Add(roomDataComponent);
        }
    }
}

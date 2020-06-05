using System;
using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    /// <summary>
    /// Loads RoomData based on a given room id
    /// </summary>
    public class RoomDataLoaderComponent : Component
    {
        public event Action<RoomData> RoomDataLoadedEvent;

        public RoomDataLoaderComponent(Entity owner) : base(owner)
        {
        }

        public void LoadRoom(int id)
        {
            RoomData roomData = new RoomDataLoader().Load(id);
            RoomDataLoadedEvent?.Invoke(roomData);
        }
    }
}
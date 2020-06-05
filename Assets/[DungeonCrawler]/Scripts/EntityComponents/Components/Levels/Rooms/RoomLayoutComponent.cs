using System;
using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    /// <summary>
    /// Holds room data and grid information
    /// </summary>
    public class RoomLayoutComponent : Component
    {
        private RoomDataLoaderComponent roomDataLoaderComponent;
        private RoomData roomData;

        public event Action<RoomData> OnDataUpdatedEvent;

        public RoomLayoutComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            roomDataLoaderComponent = GetComponent<RoomDataLoaderComponent>();
            roomDataLoaderComponent.RoomDataLoadedEvent += OnRoomDataLoaded;
        }

        protected override void OnStop()
        {
            roomDataLoaderComponent.RoomDataLoadedEvent -= OnRoomDataLoaded;
        }

        private void OnRoomDataLoaded(RoomData roomData)
        {
            this.roomData = roomData;

            OnDataUpdatedEvent?.Invoke(roomData);
        }
    }
}
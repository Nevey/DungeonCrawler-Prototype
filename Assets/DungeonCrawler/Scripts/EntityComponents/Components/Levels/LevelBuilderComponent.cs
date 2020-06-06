using System.Collections.Generic;
using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class LevelBuilderComponent : Component
    {
        private GameplayEntityFactory entityFactory;
        private List<Entity> roomEntities;

        public LevelBuilderComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            entityFactory = new GameplayEntityFactory();
            roomEntities = new List<Entity>();
        }

        private Entity CreateRoomEntity(string entityId)
        {
            Entity roomEntity = entityFactory.Instantiate(entityId);

            roomEntities.Add(roomEntity);

            return roomEntity;
        }

        public void CreateInitialRoom()
        {
            Entity entity = CreateRoomEntity("InitialRoomEntity");
            entity.GetComponent<RoomDataComponent>().Load(0);
            entity.GetComponent<SpawnPlayerComponent>().Spawn();
        }

        public void CreateRoom(int id)
        {
            Entity entity = CreateRoomEntity("RoomEntity");
            entity.GetComponent<RoomDataComponent>().Load(id);
        }
    }
}
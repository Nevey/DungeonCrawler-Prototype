using System.Collections.Generic;
using CardboardCore.DI;
using CardboardCore.EntityComponents;
using DungeonCrawler.EntityComponents;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.Levels
{
    [Injectable(Singleton = true)]
    public class LevelManager
    {
        private GameplayEntityFactory entityFactory;

        private List<Entity> roomEntities;

        public LevelManager()
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
            entity.GetComponent<RoomDataLoaderComponent>().LoadRoom(0);
            entity.GetComponent<SpawnPlayerComponent>().Spawn(2, 2);
        }

        public void CreateRoom(int id)
        {
            Entity entity = CreateRoomEntity("RoomEntity");
            entity.GetComponent<RoomDataLoaderComponent>().LoadRoom(id);
        }
    }
}
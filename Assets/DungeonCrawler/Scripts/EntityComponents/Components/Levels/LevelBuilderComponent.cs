using System;
using System.Collections.Generic;
using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    public class LevelBuilderComponent : Component
    {
        private GameplayEntityFactory gameplayEntityFactory;
        private List<Entity> roomEntities;

        private int tilesLoadedCount;

        public List<Entity> RoomEntities => roomEntities;

        public event Action<RoomDataComponent> LevelBuildingFinishedEvent;

        public LevelBuilderComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            gameplayEntityFactory = new GameplayEntityFactory();
            roomEntities = new List<Entity>();
        }

        private Entity CreateRoomEntity(string entityId)
        {
            Entity roomEntity = gameplayEntityFactory.Instantiate(entityId);

            roomEntities.Add(roomEntity);

            return roomEntity;
        }

        private void CreateTiles(RoomDataComponent roomDataComponent)
        {
            for (int x = 0; x < roomDataComponent.roomData.gridSizeX; x++)
            {
                for (int y = 0; y < roomDataComponent.roomData.gridSizeY; y++)
                {
                    roomDataComponent.AddTile(CreateTile(roomDataComponent.roomData, roomDataComponent.roomData.tiles[x, y]));
                }
            }
        }

        private TileDataComponent CreateTile(RoomData roomData, TileData tileData)
        {
            Entity tileEntity = gameplayEntityFactory.Instantiate("TileEntity");

            TileDataComponent tileDataComponent = tileEntity.GetComponent<TileDataComponent>();
            tileDataComponent.SetData(roomData, tileData);

            tileEntity.GetComponent<GridPositionComponent>().SetPosition(tileData.x, tileData.y);
            tileEntity.GetComponent<PositionComponent>().SetPosition(tileData.x, 0f, tileData.y);
            tileEntity.GetComponent<RotationComponent>().SetRotation(90f, 0f, 0f);

            TileViewComponent tileViewComponent = tileEntity.GetComponent<TileViewComponent>();
            tileViewComponent.LoadFinishedEvent += OnTileViewLoadFinished;
            tileViewComponent.Load();

            return tileDataComponent;
        }

        private void OnTileViewLoadFinished(ViewComponent viewComponent)
        {
            viewComponent.LoadFinishedEvent -= OnTileViewLoadFinished;

            RoomData roomData = viewComponent.GetComponent<TileDataComponent>().parentRoom;

            int tileCount = 0;
            for (int x = 0; x < roomData.gridSizeX; x++)
            {
                for (int y = 0; y < roomData.gridSizeY; y++)
                {
                    if (roomData.tiles[x, y].tileState == TileState.Default)
                    {
                        tileCount++;
                    }
                }
            }

            tilesLoadedCount++;

            if (tilesLoadedCount == tileCount)
            {
                LevelBuildingFinishedEvent?.Invoke(roomEntities[0].GetComponent<RoomDataComponent>());
            }
        }

        public void CreateRoom(int id, string key = "RoomEntity")
        {
            RoomData roomData = new RoomDataLoader().Load(id);

            Entity entity = CreateRoomEntity(key);

            RoomDataComponent roomDataComponent = entity.GetComponent<RoomDataComponent>();
            roomDataComponent.SetRoomData(roomData);

            CreateTiles(roomDataComponent);
        }

        public void CreateInitialRoom()
        {
            CreateRoom(2, "InitialRoomEntity");
        }
    }
}
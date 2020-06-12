using System;
using CardboardCore.EC;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EC.Components
{
    public class AreaBuilderComponent : Component
    {
        protected GameplayEntityFactory gameplayEntityFactory;
        protected RoomDataComponent currentlyBuildingRoom;
        protected int totalTileViewsToLoad;
        protected int currentTileviewsLoaded;

        private RoomRegistryComponent roomRegistryComponent;
        private TileRegistryComponent tileRegistryComponent;

        public event Action<RoomDataComponent> AreaBuildingFinishedEvent;

        protected override void OnStart()
        {
            gameplayEntityFactory = new GameplayEntityFactory();

            roomRegistryComponent = GetComponent<RoomRegistryComponent>();
            tileRegistryComponent = GetComponent<TileRegistryComponent>();
        }

        protected void CreateRoomBase(RoomData roomData, UnityEngine.Vector3 position)
        {
            // Create the room entity
            Entity roomEntity = gameplayEntityFactory.Instantiate("RoomEntity");

            currentlyBuildingRoom = roomEntity.GetComponent<RoomDataComponent>();
            currentlyBuildingRoom.SetRoomData(roomData);

            roomRegistryComponent.Add(currentlyBuildingRoom);

            // Find amount of tile views we need to load
            SetupTotalTileViewsToLoad();
        }

        protected TileDataComponent CreateTile(TileData tileData)
        {
            Entity tileEntity = gameplayEntityFactory.Instantiate("TileEntity");

            TileDataComponent tileDataComponent = tileEntity.GetComponent<TileDataComponent>();
            tileDataComponent.SetData(currentlyBuildingRoom, tileData);

            tileRegistryComponent.Add(tileDataComponent);

            tileEntity.GetComponent<GridPositionComponent>().SetPosition(tileData.x, tileData.y);
            tileEntity.GetComponent<PositionComponent>().SetPosition(tileData.x, 0f, tileData.y);
            tileEntity.GetComponent<RotationComponent>().SetRotation(90f, 0f, 0f);

            TileViewComponent tileViewComponent = tileEntity.GetComponent<TileViewComponent>();
            tileViewComponent.LoadFinishedEvent += OnTileViewLoadFinished;
            tileViewComponent.Load();

            return tileDataComponent;
        }

        protected void SetRoomEntityPosition(RoomData roomData, int x, int y)
        {
            // Set world position values
            PositionComponent positionComponent = currentlyBuildingRoom.GetComponent<PositionComponent>();
            float positionX = x + roomData.gridSizeX / 2f;
            float positionZ = y + roomData.gridSizeY / 2f;

            positionComponent.SetPosition(positionX, 0f, positionZ);
        }

        protected void SetupTotalTileViewsToLoad()
        {
            // Find amount of tile views we need to load
            totalTileViewsToLoad = 0;
            for (int x = 0; x < currentlyBuildingRoom.roomData.gridSizeX; x++)
            {
                for (int y = 0; y < currentlyBuildingRoom.roomData.gridSizeY; y++)
                {
                    TileData tileData = currentlyBuildingRoom.roomData.tiles[x, y];

                    if (tileData.tileState == TileState.Default || tileData.tileState == TileState.Doorway)
                    {
                        totalTileViewsToLoad++;
                    }
                }
            }
        }

        private void OnTileViewLoadFinished(ViewComponent viewComponent)
        {
            viewComponent.LoadFinishedEvent -= OnTileViewLoadFinished;

            currentTileviewsLoaded++;

            if (currentTileviewsLoaded == totalTileViewsToLoad)
            {
                AreaBuildingFinishedEvent?.Invoke(currentlyBuildingRoom);
                currentTileviewsLoaded = 0;
            }
        }

        public void PlayRoomBuildAnimation(Action callback = null)
        {
            int tileAnimations = currentlyBuildingRoom.tileDataComponents.Count;

            for (int i = 0; i < currentlyBuildingRoom.tileDataComponents.Count; i++)
            {
                currentlyBuildingRoom.tileDataComponents[i].GetComponent<TileViewComponent>().PlaySpawnAnimation(() =>
                {
                    tileAnimations--;

                    if (tileAnimations == 0)
                    {
                        callback?.Invoke();
                    }
                });
            }
        }
    }
}

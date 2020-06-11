using System;
using System.Collections.Generic;
using CardboardCore.EntityComponents;
using CardboardCore.Utilities;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    public class RoomBuilderComponent : Component
    {
        private GameplayEntityFactory gameplayEntityFactory;
        private RoomDataComponent currentlyBuildingRoom;
        private int totalTileViewsToLoad;
        private int currentTileviewsLoaded;

        public List<RoomDataComponent> rooms { get; private set; }

        public event Action<RoomDataComponent> LevelBuildingFinishedEvent;

        protected override void OnStart()
        {
            gameplayEntityFactory = new GameplayEntityFactory();
            rooms = new List<RoomDataComponent>();
        }

        private void CreateTiles()
        {
            for (int x = 0; x < currentlyBuildingRoom.roomData.gridSizeX; x++)
            {
                for (int y = 0; y < currentlyBuildingRoom.roomData.gridSizeY; y++)
                {
                    currentlyBuildingRoom.AddTile(CreateTile(currentlyBuildingRoom.roomData.tiles[x, y]));
                }
            }
        }

        private TileDataComponent CreateTile(TileData tileData)
        {
            Entity tileEntity = gameplayEntityFactory.Instantiate("TileEntity");

            TileDataComponent tileDataComponent = tileEntity.GetComponent<TileDataComponent>();
            tileDataComponent.SetData(currentlyBuildingRoom.roomData, tileData);

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

            currentTileviewsLoaded++;

            if (currentTileviewsLoaded == totalTileViewsToLoad)
            {
                LevelBuildingFinishedEvent?.Invoke(currentlyBuildingRoom);

                currentTileviewsLoaded = 0;
                currentlyBuildingRoom = null;
            }
        }

        private void SetupTotalTileViewsToLoad()
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

        public void CreateRoom(int id, string key = "RoomEntity")
        {
            // Create the room entity
            Entity roomEntity = gameplayEntityFactory.Instantiate(key);

            // Set room data
            RoomData roomData = new RoomDataLoader().Load(id);

            currentlyBuildingRoom = roomEntity.GetComponent<RoomDataComponent>();
            currentlyBuildingRoom.SetRoomData(roomData);

            // Set world position values
            PositionComponent positionComponent = roomEntity.GetComponent<PositionComponent>();
            float positionX = positionComponent.position.x + roomData.gridSizeX / 2f;
            float positionZ = positionComponent.position.z + roomData.gridSizeY / 2f;
            positionComponent.SetPosition(positionX, 0f, positionZ);

            // Find amount of tile views we need to load
            SetupTotalTileViewsToLoad();

            // Add current room data component to rooms list for future access
            rooms.Add(currentlyBuildingRoom);

            // Start creating tiles!
            CreateTiles();
        }

        public void CreateInitialRoom()
        {
            CreateRoom(2);
        }

        public void CreateCorridor(RoomDataComponent currentRoom, int x, int y)
        {
            Entity corridorEntity = gameplayEntityFactory.Instantiate("CorridorEntity");

            RoomData roomData = new RoomData();
            roomData.id = -1; // Not used during runtime, only for loading

            UnityEngine.Vector2Int[] potentialSpawnLocations = currentRoom.GetPotentialSpawnLocations(x, y);

            int randomIndex = UnityEngine.Random.Range(0, potentialSpawnLocations.Length);
            UnityEngine.Vector2Int spawnLocation = potentialSpawnLocations[randomIndex];

            UnityEngine.Vector2Int direction = spawnLocation - new UnityEngine.Vector2Int(x, y);

            // TODO: Add one or two turns in the corridor
            int corridorLength = 4;

            roomData.gridSizeX = direction.x * corridorLength;
            roomData.gridSizeY = direction.y * corridorLength;

            currentlyBuildingRoom = corridorEntity.GetComponent<RoomDataComponent>();
            currentlyBuildingRoom.SetRoomData(roomData);

            PositionComponent positionComponent = corridorEntity.GetComponent<PositionComponent>();
            float positionX = positionComponent.position.x + roomData.gridSizeX / 2f;
            float positionZ = positionComponent.position.z + roomData.gridSizeY / 2f;
            positionComponent.SetPosition(positionX, 0f, positionZ);

            // Find amount of tile views we need to load
            SetupTotalTileViewsToLoad();

            rooms.Add(currentlyBuildingRoom);

            // Create corridor tiles
            for (int i = 0; i < corridorLength; i++)
            {
                UnityEngine.Vector2Int d = direction * (i + 1);

                TileData tileData = new TileData();
                tileData.x = x + d.x;
                tileData.y = y + d.y;
                tileData.tileState = TileState.Default;
                tileData.walkableState = WalkableState.Walkable;

                TileDataComponent tileDataComponent = CreateTile(tileData);
                TileViewComponent tileViewComponent = tileDataComponent.GetComponent<TileViewComponent>();
                tileViewComponent.SetupSpawnAnimationOnViewLoaded(i);

                currentlyBuildingRoom.AddTile(tileDataComponent);
            }
        }
    }
}

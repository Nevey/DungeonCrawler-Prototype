using System;
using System.Collections.Generic;
using CardboardCore.EntityComponents;
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

        private void CreateBase(RoomData roomData, string key, UnityEngine.Vector3 position)
        {
            // Create the room entity
            Entity roomEntity = gameplayEntityFactory.Instantiate(key);

            currentlyBuildingRoom = roomEntity.GetComponent<RoomDataComponent>();
            currentlyBuildingRoom.SetRoomData(roomData);

            // Set world position values
            PositionComponent positionComponent = roomEntity.GetComponent<PositionComponent>();
            float positionX = position.x + roomData.gridSizeX / 2f;
            float positionZ = position.z + roomData.gridSizeY / 2f;
            positionComponent.SetPosition(positionX, 0f, positionZ);

            // Find amount of tile views we need to load
            SetupTotalTileViewsToLoad();

            // Add current room data component to rooms list for future access
            rooms.Add(currentlyBuildingRoom);
        }

        private void CreateRoomTiles()
        {
            for (int x = 0; x < currentlyBuildingRoom.roomData.gridSizeX; x++)
            {
                for (int y = 0; y < currentlyBuildingRoom.roomData.gridSizeY; y++)
                {
                    currentlyBuildingRoom.AddTile(CreateTile(currentlyBuildingRoom.roomData.tiles[x, y]));
                }
            }
        }

        private void CreateCorridorTiles(int x, int y, int corridorLength, UnityEngine.Vector2Int spawnDirection)
        {
            for (int i = 0; i < corridorLength; i++)
            {
                UnityEngine.Vector2Int d = spawnDirection * (i + 1);

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

        public void CreateRoom(int id, string key = "RoomEntity", UnityEngine.Vector3? position = null)
        {
            position = position == null ? position = UnityEngine.Vector3.zero : position;

            RoomData roomData = new RoomDataLoader().Load(id);

            CreateBase(roomData, key, position.Value);
            CreateRoomTiles();
        }

        public void CreateInitialRoom()
        {
            CreateRoom(2);
        }

        public void CreateCorridor(RoomDataComponent currentRoom, int x, int y)
        {
            // Get potential spawn locations around given coords
            UnityEngine.Vector2Int[] potentialSpawnLocations = currentRoom.GetPotentialSpawnLocations(x, y);

            // Get an actual spawn location, randomly
            int randomIndex = UnityEngine.Random.Range(0, potentialSpawnLocations.Length);
            UnityEngine.Vector2Int spawnLocation = potentialSpawnLocations[randomIndex];

            // Set spawn direction, away from given coords
            UnityEngine.Vector2Int spawnDirection = spawnLocation - new UnityEngine.Vector2Int(x, y);

            // TODO: Add one or two turns in the corridor
            // Set length of the corridor
            int corridorLength = 4;

            // Create room data, based on corridor length and spawn direction
            RoomData roomData = new RoomData();
            roomData.gridSizeX = spawnDirection.x * corridorLength;
            roomData.gridSizeY = spawnDirection.y * corridorLength;

            // Get spawn location in world space based on given coords
            UnityEngine.Vector3 position = new UnityEngine.Vector3(x, 0f, y);

            // Create base room
            CreateBase(roomData, "CorridorEntity", position);

            // Create corridor tiles
            CreateCorridorTiles(x, y, corridorLength, spawnDirection);
        }
    }
}

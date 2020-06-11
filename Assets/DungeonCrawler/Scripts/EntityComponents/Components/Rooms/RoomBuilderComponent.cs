using System;
using System.Collections.Generic;
using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    public class RoomBuilderComponent : Component
    {
        [TweakableField] private int initialRoomId;

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

        private void CreateRoomBase(RoomData roomData, UnityEngine.Vector3 position)
        {
            // Create the room entity
            Entity roomEntity = gameplayEntityFactory.Instantiate("RoomEntity");

            currentlyBuildingRoom = roomEntity.GetComponent<RoomDataComponent>();
            currentlyBuildingRoom.SetRoomData(roomData);

            // Find amount of tile views we need to load
            SetupTotalTileViewsToLoad();

            // Add current room data component to rooms list for future access
            rooms.Add(currentlyBuildingRoom);
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

        private void CreateRoomTiles(int offsetX, int offsetY, UnityEngine.Vector2Int spawnDirection)
        {
            for (int x = 0; x < currentlyBuildingRoom.roomData.gridSizeX; x++)
            {
                for (int y = 0; y < currentlyBuildingRoom.roomData.gridSizeY; y++)
                {
                    TileData tileData = currentlyBuildingRoom.roomData.tiles[x, y];
                    tileData.x = tileData.x + offsetX;
                    tileData.y = tileData.y + offsetY;

                    TileDataComponent tileDataComponent = CreateTile(tileData);

                    currentlyBuildingRoom.AddTile(tileDataComponent);
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

        private void SetRoomEntityPosition(RoomData roomData, int x, int y)
        {
            // Set world position values
            PositionComponent positionComponent = currentlyBuildingRoom.GetComponent<PositionComponent>();
            float positionX = x + roomData.gridSizeX / 2f;
            float positionZ = y + roomData.gridSizeY / 2f;
            positionComponent.SetPosition(positionX, 0f, positionZ);
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

        public void CreateRoom(RoomData roomData, UnityEngine.Vector3? position = null, UnityEngine.Vector2Int? direction = null)
        {
            // Get position
            position = position == null ? position = UnityEngine.Vector3.zero : position;

            // Create base room
            CreateRoomBase(roomData, position.Value);

            int offsetX = (int)position.Value.x;
            int offsetY = (int)position.Value.z;

            TileData tileData = null;

            if (direction == null)
            {
                tileData = new TileData();
            }
            else if (direction.Value == UnityEngine.Vector2Int.up)
            {
                // Get free tile at bottom row
                tileData = currentlyBuildingRoom.GetFreeTileAtY(0);
                offsetX -= tileData.x;
            }
            else if (direction.Value == UnityEngine.Vector2Int.down)
            {
                // Get free tile at top row
                tileData = currentlyBuildingRoom.GetFreeTileAtY(roomData.gridSizeY - 1);
                offsetX -= tileData.x;
            }
            else if (direction.Value == UnityEngine.Vector2Int.left)
            {
                // Get free tile at right column
                tileData = currentlyBuildingRoom.GetFreeTileAtX(0);
                offsetY -= tileData.y;
            }
            else if (direction.Value == UnityEngine.Vector2Int.right)
            {
                // Get free tile at left column
                tileData = currentlyBuildingRoom.GetFreeTileAtX(roomData.gridSizeX - 1);
                offsetY -= tileData.y;
            }


            currentlyBuildingRoom.offsetX = offsetX;
            currentlyBuildingRoom.offsetY = offsetY;

            if (direction == null)
            {
                direction = UnityEngine.Vector2Int.up;
            }

            // Create room tiles
            CreateRoomTiles(offsetX, offsetY, direction.Value);

            SetRoomEntityPosition(roomData, offsetX, offsetY);
        }

        public void CreateRoom(int id, UnityEngine.Vector3? position = null, UnityEngine.Vector2Int? direction = null)
        {
            // Load room data based on given id
            RoomData roomData = new RoomDataLoader().Load(id);

            CreateRoom(roomData, position, direction);
        }

        public void CreateInitialRoom()
        {
            CreateRoom(initialRoomId);
        }

        public bool CreateCorridor(RoomDataComponent currentRoom, int x, int y, out UnityEngine.Vector3 spawnOffset, out UnityEngine.Vector2Int spawnDirection)
        {
            // Get potential spawn locations around given coords
            UnityEngine.Vector2Int[] potentialSpawnLocations = currentRoom.GetPotentialSpawnLocations(x, y);

            if (potentialSpawnLocations.Length == 0)
            {
                spawnOffset = UnityEngine.Vector3.zero;
                spawnDirection = UnityEngine.Vector2Int.zero;
                return false;
            }

            // Get an actual spawn location, randomly
            int randomIndex = UnityEngine.Random.Range(0, potentialSpawnLocations.Length);
            UnityEngine.Vector2Int spawnLocation = potentialSpawnLocations[randomIndex];

            // Set spawn direction, away from given coords
            spawnDirection = spawnLocation - new UnityEngine.Vector2Int(x, y);

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
            CreateRoomBase(roomData, position);

            // Create corridor tiles
            CreateCorridorTiles(x, y, corridorLength, spawnDirection);

            spawnOffset = new UnityEngine.Vector3(spawnLocation.x, 0f, spawnLocation.y);
            spawnOffset.x += spawnDirection.x * corridorLength;
            spawnOffset.z += spawnDirection.y * corridorLength;

            SetRoomEntityPosition(roomData, (int)spawnOffset.x, (int)spawnOffset.z);

            return true;
        }
    }
}

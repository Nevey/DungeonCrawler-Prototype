using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CorridorBuilderComponent : AreaBuilderComponent
    {
        private void CreateCorridorTiles(int x, int y, int corridorLength, UnityEngine.Vector2Int spawnDirection)
        {
            for (int i = 0; i < corridorLength; i++)
            {
                UnityEngine.Vector2Int spawnPosition = spawnDirection * (i + 1);

                TileData tileData = new TileData();
                tileData.x = x + spawnPosition.x;
                tileData.y = y + spawnPosition.y;
                tileData.tileState = TileState.Default;
                tileData.walkableState = WalkableState.Walkable;

                TileDataComponent tileDataComponent = CreateTile(tileData);
                TileViewComponent tileViewComponent = tileDataComponent.GetComponent<TileViewComponent>();
                tileViewComponent.SetupSpawnAnimationOnViewLoaded(i);

                currentlyBuildingRoom.AddTile(tileDataComponent);
            }
        }

        public void CreateCorridor(RoomDataComponent currentRoom, int x, int y, out UnityEngine.Vector3 spawnOffset, out UnityEngine.Vector2Int spawnDirection)
        {
            // Get potential spawn locations around given coords
            UnityEngine.Vector2Int[] potentialSpawnLocations = currentRoom.GetPotentialSpawnLocations(x, y);

            if (potentialSpawnLocations.Length == 0)
            {
                spawnOffset = UnityEngine.Vector3.zero;
                spawnDirection = UnityEngine.Vector2Int.zero;
                return;
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
            roomData.gridSizeX = roomData.gridSizeX == 0 ? 1 : roomData.gridSizeX;
            roomData.gridSizeY = roomData.gridSizeY == 0 ? 1 : roomData.gridSizeY;

            // Faking tile data...
            roomData.tiles = new TileData[roomData.gridSizeX, roomData.gridSizeY];

            for (int tilesX = 0; tilesX < roomData.gridSizeX; tilesX++)
            {
                for (int tilesY = 0; tilesY < roomData.gridSizeY; tilesY++)
                {
                    roomData.tiles[tilesX, tilesY] = new TileData();
                }
            }

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
        }
    }
}

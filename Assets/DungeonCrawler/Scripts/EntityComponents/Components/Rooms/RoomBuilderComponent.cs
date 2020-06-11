using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    public class RoomBuilderComponent : AreaBuilderComponent
    {
        [TweakableField] private int initialRoomId;

        private void CreateRoomTiles(int offsetX, int offsetY, UnityEngine.Vector2Int spawnDirection)
        {
            UnityEngine.Vector2 gridCenter = new UnityEngine.Vector2(
                currentlyBuildingRoom.roomData.gridSizeX / 2f,
                currentlyBuildingRoom.roomData.gridSizeY / 2f);

            gridCenter.x -= 0.5f;
            gridCenter.y -= 0.5f;

            for (int x = 0; x < currentlyBuildingRoom.roomData.gridSizeX; x++)
            {
                for (int y = 0; y < currentlyBuildingRoom.roomData.gridSizeY; y++)
                {
                    TileData tileData = currentlyBuildingRoom.roomData.tiles[x, y];
                    tileData.x = tileData.x + offsetX;
                    tileData.y = tileData.y + offsetY;

                    TileDataComponent tileDataComponent = CreateTile(tileData);

                    // Calculate distance in ints, for a delay effect
                    float distanceFromGridCenter = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(x, y), gridCenter);

                    TileViewComponent tileViewComponent = tileDataComponent.GetComponent<TileViewComponent>();
                    tileViewComponent.SetupSpawnAnimation(distanceFromGridCenter);

                    currentlyBuildingRoom.AddTile(tileDataComponent);
                }
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
    }
}

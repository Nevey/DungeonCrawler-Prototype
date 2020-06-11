using System.Collections.Generic;
using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    /// <summary>
    /// Holds room data and grid information
    /// </summary>
    public class RoomDataComponent : Component
    {
        private GameplayEntityFactory gameplayEntityFactory;

        /// <summary>
        /// List of all tiles in this room
        /// </summary>
        /// <value></value>
        public List<TileDataComponent> tileDataComponents { get; private set; }

        /// <summary>
        /// List of all cards in this room
        /// </summary>
        /// <value></value>
        public List<RoomCardDataComponent> roomCardDataComponents { get; private set; }

        public RoomData roomData { get; private set; }

        protected override void OnStart()
        {
            gameplayEntityFactory = new GameplayEntityFactory();

            tileDataComponents = new List<TileDataComponent>();
            roomCardDataComponents = new List<RoomCardDataComponent>();
        }

        public void SetRoomData(RoomData roomData)
        {
            this.roomData = roomData;
        }

        public void AddTile(TileDataComponent tileDataComponent)
        {
            tileDataComponents.Add(tileDataComponent);
        }

        public TileDataComponent[] GetTiles(TileState tileState)
        {
            List<TileDataComponent> foundTiles = new List<TileDataComponent>();

            for (int i = 0; i < tileDataComponents.Count; i++)
            {
                if (tileDataComponents[i].Data.tileState == tileState)
                {
                    foundTiles.Add(tileDataComponents[i]);
                }
            }

            return foundTiles.ToArray();
        }

        public TileDataComponent GetFreeTile()
        {
            TileDataComponent[] tiles = GetTiles(TileState.Default);
            int randomIndex = UnityEngine.Random.Range(0, tiles.Length);

            return tiles[randomIndex];
        }

        public TileData[] GetSurroundingTiles(int x, int y)
        {
            int right = x + 1;
            int left = x - 1;
            int up = y + 1;
            int down = y - 1;

            List<TileData> tiles = new List<TileData>();

            if (right < roomData.gridSizeX)
            {
                tiles.Add(roomData.tiles[right, y]);
            }

            if (left >= 0)
            {
                tiles.Add(roomData.tiles[left, y]);
            }

            if (up < roomData.gridSizeY)
            {
                tiles.Add(roomData.tiles[x, up]);
            }

            if (down >= 0)
            {
                tiles.Add(roomData.tiles[x, down]);
            }

            return tiles.ToArray();
        }

        public TileData[] GetSurroundingTiles(int x, int y, TileState tileState)
        {
            List<TileData> tiles = new List<TileData>();
            TileData[] surroundingTiles = GetSurroundingTiles(x, y);

            foreach (TileData tile in surroundingTiles)
            {
                if (tile.tileState == tileState)
                {
                    tiles.Add(tile);
                }
            }

            return tiles.ToArray();
        }

        public UnityEngine.Vector2Int[] GetPotentialSpawnLocations(int x, int y)
        {
            List<UnityEngine.Vector2Int> locations = new List<UnityEngine.Vector2Int>();

            int right = x + 1;
            int left = x - 1;
            int up = y + 1;
            int down = y - 1;

            if (right > roomData.gridSizeX - 1)
            {
                locations.Add(new UnityEngine.Vector2Int(right, y));
            }

            if (left < 0)
            {
                locations.Add(new UnityEngine.Vector2Int(left, y));
            }

            if (up > roomData.gridSizeY - 1)
            {
                locations.Add(new UnityEngine.Vector2Int(x, up));
            }

            if (down < 0)
            {
                locations.Add(new UnityEngine.Vector2Int(x, down));
            }

            return locations.ToArray();
        }

        public void AddRoomCard(RoomCardDataComponent roomCardDataComponent)
        {
            roomCardDataComponents.Add(roomCardDataComponent);
        }
    }
}

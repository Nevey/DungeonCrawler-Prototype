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

        public void AddRoomCard(RoomCardDataComponent roomCardDataComponent)
        {
            roomCardDataComponents.Add(roomCardDataComponent);
        }
    }
}
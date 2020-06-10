using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    public class TileDataComponent : Component
    {
        private TileData tileData;
        public RoomData parentRoom { get; private set; }

        public TileData Data => tileData;

        public void SetData(RoomData roomData, TileData tileData)
        {
            parentRoom = roomData;
            this.tileData = tileData;
        }
    }
}
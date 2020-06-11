using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    public class TileDataComponent : Component
    {
        public TileData tileData { get; private set; }
        public RoomData parentRoom { get; private set; }

        public void SetData(RoomData roomData, TileData tileData)
        {
            parentRoom = roomData;
            this.tileData = tileData;
        }
    }
}

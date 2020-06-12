using CardboardCore.EC;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EC.Components
{
    public class TileDataComponent : Component
    {
        public TileData tileData { get; private set; }
        public RoomDataComponent parentRoom { get; private set; }

        public void SetData(RoomDataComponent roomDataComponent, TileData tileData)
        {
            parentRoom = roomDataComponent;
            this.tileData = tileData;
        }
    }
}

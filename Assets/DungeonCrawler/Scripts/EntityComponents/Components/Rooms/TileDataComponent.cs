using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    public class TileDataComponent : Component
    {
        private TileData tileData;
        public RoomData parentRoom { get; private set; }

        public TileData Data => tileData;

        public TileDataComponent(Entity owner) : base(owner)
        {
        }

        public void SetData(RoomData roomData, TileData tileData)
        {
            parentRoom = roomData;
            this.tileData = tileData;
        }
    }
}
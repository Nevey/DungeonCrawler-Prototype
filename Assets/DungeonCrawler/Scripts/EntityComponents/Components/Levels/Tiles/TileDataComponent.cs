using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    public class TileDataComponent : Component
    {
        private TileData tileData;

        public TileData Data => tileData;

        public TileDataComponent(Entity owner) : base(owner)
        {
        }

        public void SetData(TileData tileData)
        {
            this.tileData = tileData;
        }
    }
}
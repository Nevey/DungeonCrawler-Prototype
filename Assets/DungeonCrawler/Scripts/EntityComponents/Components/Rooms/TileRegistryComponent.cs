using System.Collections.Generic;
using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class TileRegistryComponent : Component
    {
        public List<TileDataComponent> tiles { get; private set; }

        protected override void OnStart()
        {
            tiles = new List<TileDataComponent>();
        }

        public void Add(TileDataComponent tileDataComponent)
        {
            tiles.Add(tileDataComponent);
        }

        public TileDataComponent GetTile(int x, int y)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].tileData.x == x && tiles[i].tileData.y == y)
                {
                    return tiles[i];
                }
            }

            return null;
        }
    }
}

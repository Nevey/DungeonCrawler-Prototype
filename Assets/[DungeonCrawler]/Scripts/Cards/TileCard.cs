using DungeonCrawler.Levels;

namespace DungeonCrawler.Cards
{
    public class TileCard : Card
    {
        private Tile tile;

        public TileCard(int id, string name, Tile tile)
            : base(id, name)
        {
            this.tile = tile;
        }
    }
}
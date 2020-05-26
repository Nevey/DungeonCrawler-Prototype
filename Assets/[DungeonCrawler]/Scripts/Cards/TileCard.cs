using DungeonCrawler.Levels;

namespace DungeonCrawler.Cards
{
    public class TileCard : Card
    {
        private TileData tile;

        public TileCard(int id, string name, TileData tile)
            : base(id, name)
        {
            this.tile = tile;
        }
    }
}
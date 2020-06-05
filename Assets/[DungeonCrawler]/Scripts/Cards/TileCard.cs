using DungeonCrawler.Levels;

namespace DungeonCrawler.Cards
{
    public class TileCard : Card
    {
        private TileData tileData;

        public TileCard(int id, string name, TileData tileData)
            : base(id, name)
        {
            this.tileData = tileData;
        }
    }
}
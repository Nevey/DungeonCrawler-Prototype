using CardboardCore.EntityComponents;
using DungeonCrawler.Cards;

namespace DungeonCrawler.EntityComponents.Components
{
    public class TileCardDataComponent : CardDataComponent<TileCardData>
    {
        public TileCardDataComponent(Entity owner) : base(owner)
        {
        }
    }
}
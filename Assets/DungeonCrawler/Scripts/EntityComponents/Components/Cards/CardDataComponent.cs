using CardboardCore.EntityComponents;
using DungeonCrawler.Cards;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CardDataComponent : Component
    {
        private CardData cardData;

        public CardDataComponent(Entity owner) : base(owner)
        {
        }

        public void SetData(CardData cardData)
        {
            this.cardData = cardData;
        }
    }
}
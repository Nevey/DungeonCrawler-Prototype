using CardboardCore.EntityComponents;
using DungeonCrawler.Cards;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CardDataComponent<T> : Component
        where T : CardData
    {
        protected T cardData;

        public CardDataComponent(Entity owner) : base(owner)
        {
        }

        public void SetData(T cardData)
        {
            this.cardData = cardData;
        }
    }
}
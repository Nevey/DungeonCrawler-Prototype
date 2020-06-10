using CardboardCore.EntityComponents;
using DungeonCrawler.Cards;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CardDataComponent : Component
    {
        protected CardData cardData;
    }

    public class CardDataComponent<T> : CardDataComponent
        where T : CardData
    {
        // protected T cardData;

        public void SetData(T cardData)
        {
            this.cardData = cardData;
        }

        public T GetCardData()
        {
            return cardData as T;
        }
    }
}
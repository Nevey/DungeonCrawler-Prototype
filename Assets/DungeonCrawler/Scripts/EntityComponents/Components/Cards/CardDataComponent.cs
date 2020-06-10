using CardboardCore.EntityComponents;
using DungeonCrawler.Cards;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CardDataComponent<T> : Component
        where T : CardData
    {
        protected T cardData;

        public void SetData(T cardData)
        {
            this.cardData = cardData;
        }
    }
}
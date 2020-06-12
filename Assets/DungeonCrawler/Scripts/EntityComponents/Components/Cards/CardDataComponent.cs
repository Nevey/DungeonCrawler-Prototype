using System;
using CardboardCore.EntityComponents;
using DungeonCrawler.Cards;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CardDataComponent : Component
    {
        protected CardData cardData;

        public event Action<CardDataComponent> StoppedEvent;

        protected override void OnStop()
        {
            StoppedEvent?.Invoke(this);
        }
    }

    public class CardDataComponent<T> : CardDataComponent
        where T : CardData
    {
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

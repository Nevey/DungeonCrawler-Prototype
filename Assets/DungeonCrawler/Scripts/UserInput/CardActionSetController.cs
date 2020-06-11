using System;
using CardboardCore.UserInput;

namespace DungeonCrawler.UserInput
{
    public class CardActionSetController : ActionSetController
    {
        private CardActionSet cardActionSet;

        public event Action ContinueInputReleasedEvent;

        protected override void Awake()
        {
            base.Awake();

            cardActionSet = AddActionSet<CardActionSet>();
            cardActionSet.ContinueInputReleasedEvent += OnContinueInputReleased;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            cardActionSet.ContinueInputReleasedEvent -= OnContinueInputReleased;
        }

        private void OnContinueInputReleased()
        {
            ContinueInputReleasedEvent?.Invoke();
        }
    }
}

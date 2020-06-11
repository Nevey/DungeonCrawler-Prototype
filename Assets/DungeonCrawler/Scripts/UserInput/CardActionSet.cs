using CardboardCore.UserInput;
using CardboardCore.UserInput.Actions;
using UnityEngine;

namespace DungeonCrawler.UserInput
{
    public class CardActionSet : ActionSet
    {
        private ButtonAction mouseContinueAction;
        private ButtonAction spacebarContinueAction;

        public event System.Action ContinueInputReleasedEvent;

        public CardActionSet()
        {
            mouseContinueAction = CreateButtonAction(KeyCode.Mouse0);
            spacebarContinueAction = CreateButtonAction(KeyCode.Space);
        }

        protected override void OnBind()
        {
            mouseContinueAction.ReleaseEvent += OnContinueInputReleased;
            spacebarContinueAction.ReleaseEvent += OnContinueInputReleased;
        }

        protected override void OnUnbind()
        {
            mouseContinueAction.ReleaseEvent -= OnContinueInputReleased;
            spacebarContinueAction.ReleaseEvent -= OnContinueInputReleased;
        }

        private void OnContinueInputReleased()
        {
            ContinueInputReleasedEvent?.Invoke();
        }
    }
}

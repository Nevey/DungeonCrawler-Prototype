using CardboardCore.DI;
using DungeonCrawler.UserInput;

namespace DungeonCrawler.RoomBuilding.States
{
    public class WaitForUserInputState : RoomBuilderState
    {
        [Inject] private InputManager inputManager;

        protected override void OnEnter()
        {
            inputManager.cardActionSetController.ContinueInputReleasedEvent += OnContinueInputReleased;
            inputManager.cardActionSetController.Bind();
        }

        protected override void OnExit()
        {
        }

        private void OnContinueInputReleased()
        {
            inputManager.cardActionSetController.ContinueInputReleasedEvent -= OnContinueInputReleased;
            inputManager.cardActionSetController.Unbind();

            owner.ToNextState();
        }
    }
}

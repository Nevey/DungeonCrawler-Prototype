using CardboardCore.StateMachines;
using DungeonCrawler.RoomBuilding;

namespace DungeonCrawler.Gameplay.States
{
    public class StartGameplayState : State
    {
        private InitialRoomBuilderStateMachine initialRoomBuilderStateMachine;
        protected override void OnEnter()
        {
            initialRoomBuilderStateMachine = new InitialRoomBuilderStateMachine();

            initialRoomBuilderStateMachine.StoppedEvent += OnRoomBuilderStateMachineStopped;
            initialRoomBuilderStateMachine.Start();
        }

        protected override void OnExit()
        {
            initialRoomBuilderStateMachine.StoppedEvent -= OnRoomBuilderStateMachineStopped;
            initialRoomBuilderStateMachine = null;
        }

        private void OnRoomBuilderStateMachineStopped()
        {
            owner.ToNextState();
        }
    }
}

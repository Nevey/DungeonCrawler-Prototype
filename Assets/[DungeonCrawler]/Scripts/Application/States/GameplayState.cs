using CardboardCore.StateMachines;
using DungeonCrawler.Gameplay;

namespace DungeonCrawler.Application.States
{
    public class GameplayState : State
    {
        private GameplayStateMachine gameplayStateMachine;

        protected override void OnEnter()
        {
            gameplayStateMachine = new GameplayStateMachine();
            gameplayStateMachine.Start();
        }

        protected override void OnExit()
        {
            gameplayStateMachine.Stop();
        }
    }
}
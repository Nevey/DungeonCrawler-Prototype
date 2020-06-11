using CardboardCore.StateMachines;
using DungeonCrawler.Gameplay.States;

namespace DungeonCrawler.Gameplay
{
    public class GameplayStateMachine : StateMachine
    {
        public GameplayStateMachine()
        {
            SetInitialState<StartGameplayState>();
            AddTransition<StartGameplayState, PlayerMovementState>();
        }
    }
}

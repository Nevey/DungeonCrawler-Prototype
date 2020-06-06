using CardboardCore.StateMachines;
using DungeonCrawler.Gameplay.States;

namespace DungeonCrawler.Gameplay
{
    public class GameplayStateMachine : StateMachine
    {
        public GameplayStateMachine()
        {
            SetInitialState<InitializeLevelState>();
            AddTransition<InitializeLevelState, SpawnPlayersState>();
            AddTransition<SpawnPlayersState, PlaceCardsState>();
            AddTransition<PlaceCardsState, PlayerMovementState>();
        }
    }
}
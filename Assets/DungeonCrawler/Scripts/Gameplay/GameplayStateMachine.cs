using CardboardCore.StateMachines;
using DungeonCrawler.Gameplay.States;

namespace DungeonCrawler.Gameplay
{
    public class GameplayStateMachine : StateMachine
    {
        public GameplayStateMachine()
        {
            SetInitialState<SpawnCameraState>();
            AddTransition<SpawnCameraState, InitializeLevelState>();
            AddTransition<InitializeLevelState, SpawnPlayersState>();
            AddTransition<SpawnPlayersState, PlaceCardsState>();
            AddTransition<PlaceCardsState, PlayerMovementState>();
        }
    }
}
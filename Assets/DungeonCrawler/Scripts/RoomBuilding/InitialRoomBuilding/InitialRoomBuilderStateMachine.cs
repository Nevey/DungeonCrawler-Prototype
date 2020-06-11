using CardboardCore.StateMachines;
using DungeonCrawler.RoomBuilding.States;

namespace DungeonCrawler.RoomBuilding
{
    public class InitialRoomBuilderStateMachine : StateMachine
    {
        public InitialRoomBuilderStateMachine()
        {
            SetInitialState<SpawnCameraState>();
            AddTransition<SpawnCameraState, SpawnInitialRoomState>();
            AddTransition<SpawnInitialRoomState, AnimateRoomState>();
            AddTransition<AnimateRoomState, SpawnPlayersState>();
            AddTransition<SpawnPlayersState, PlaceCardsState>();
            AddTransition<PlaceCardsState, StopInitialRoomBuildingState>();
        }
    }
}

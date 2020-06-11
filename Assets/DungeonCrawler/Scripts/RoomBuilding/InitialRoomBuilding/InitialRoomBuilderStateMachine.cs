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
            AddTransition<SpawnInitialRoomState, SpawnPlayersState>();
            AddTransition<SpawnPlayersState, PlaceCardsState>();
            AddTransition<PlaceCardsState, StopInitialRoomBuildingState>();
        }
    }
}

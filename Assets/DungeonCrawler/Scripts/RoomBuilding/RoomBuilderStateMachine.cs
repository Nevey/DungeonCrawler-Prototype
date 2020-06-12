using System.Collections.Generic;
using CardboardCore.StateMachines;
using DungeonCrawler.EC.Components;
using DungeonCrawler.RoomBuilding.States;

namespace DungeonCrawler.RoomBuilding
{
    public class RoomBuilderStateMachine : StateMachine
    {

        public RoomBuilderStateMachine(RoomDataComponent currentRoom, RoomCardDataComponent roomCardDataComponent, int x, int y)
        {
            SetupTransitions();
            SetupRoomBuilderStates(currentRoom, roomCardDataComponent, x, y);
        }

        protected virtual void SetupTransitions()
        {
            SetInitialState<CameraFocusOnCardState>();
            AddTransition<CameraFocusOnCardState, AnimateCardPickupState>();
            AddTransition<AnimateCardPickupState, WaitForUserInputState>();
            AddTransition<WaitForUserInputState, CreateCorridorAndRoomState>();
            AddTransition<CreateCorridorAndRoomState, PlaceCardsState>();
            AddTransition<PlaceCardsState, StopStateMachineState>();
        }

        private void SetupRoomBuilderStates(RoomDataComponent currentRoom, RoomCardDataComponent roomCardDataComponent, int x, int y)
        {
            foreach (KeyValuePair<System.Type, State> item in stateDict)
            {
                if (item.Value is RoomBuilderState state)
                {
                    state.currentRoom = currentRoom;
                    state.roomCardDataComponent = roomCardDataComponent;
                    state.x = x;
                    state.y = y;
                }
            }
        }
    }
}

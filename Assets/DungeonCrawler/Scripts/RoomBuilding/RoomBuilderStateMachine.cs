using System.Collections.Generic;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents.Components;
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
            AddTransition<WaitForUserInputState, CreateCorridorState>();
            AddTransition<CreateCorridorState, AnimateCardPlacementState>();
        }

        private void SetupRoomBuilderStates(RoomDataComponent currentRoom, RoomCardDataComponent roomCardDataComponent, int x, int y)
        {
            foreach (KeyValuePair<System.Type, State> item in stateDict)
            {
                RoomBuilderState state = item.Value as RoomBuilderState;
                state.currentRoom = currentRoom;
                state.roomCardDataComponent = roomCardDataComponent;
                state.x = x;
                state.y = y;
            }
        }
    }
}

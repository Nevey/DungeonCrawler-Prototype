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
            SetInitialState<CameraFocusOnCardState>();
            AddTransition<CameraFocusOnCardState, AnimateCardPickupState>();
            AddTransition<AnimateCardPickupState, WaitForUserInputState>();
            AddTransition<WaitForUserInputState, CreateCorridorState>();
            AddTransition<CreateCorridorState, SpawnRoomState>();
            AddTransition<SpawnRoomState, AnimateCardPlacementState>();
            // TODO: Animate room creation state

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

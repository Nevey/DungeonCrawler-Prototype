using System.Collections.Generic;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents.Components;
using DungeonCrawler.RoomBuilding.States;

namespace DungeonCrawler.RoomBuilding
{
    public class RoomBuilderStateMachine : StateMachine
    {
        public RoomBuilderStateMachine(RoomCardDataComponent roomCardDataComponent)
        {
            SetInitialState<CameraFocusOnCardState>();
            AddTransition<CameraFocusOnCardState, AnimateCardState>();

            foreach (KeyValuePair<System.Type, State> item in stateDict)
            {
                RoomBuilderState state = item.Value as RoomBuilderState;
                state.roomCardDataComponent = roomCardDataComponent;
            }
        }
    }
}

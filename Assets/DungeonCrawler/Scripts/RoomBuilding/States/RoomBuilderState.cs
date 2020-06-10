using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public abstract class RoomBuilderState : State
    {
        public RoomCardDataComponent roomCardDataComponent;
    }
}
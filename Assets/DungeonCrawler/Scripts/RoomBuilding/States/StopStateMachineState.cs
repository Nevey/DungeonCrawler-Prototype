using CardboardCore.StateMachines;

namespace DungeonCrawler.RoomBuilding.States
{
    public class StopStateMachineState : State
    {
        protected override void OnEnter()
        {
            owner.Stop();
        }

        protected override void OnExit()
        {

        }
    }
}

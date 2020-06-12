using CardboardCore.DI;
using CardboardCore.EC;
using CardboardCore.StateMachines;
using DungeonCrawler.EC.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public class AnimateRoomState : State
    {
        [Inject] private EntityRegister entityRegister;

        protected override void OnEnter()
        {
            entityRegister.FindEntity("LevelEntity").GetComponent<RoomBuilderComponent>().PlayRoomBuildAnimation();
            owner.ToNextState();
        }

        protected override void OnExit()
        {

        }
    }
}

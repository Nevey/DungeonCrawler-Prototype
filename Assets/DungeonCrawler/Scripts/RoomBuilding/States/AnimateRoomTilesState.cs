using CardboardCore.DI;
using CardboardCore.EntityComponents;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents.Components;

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

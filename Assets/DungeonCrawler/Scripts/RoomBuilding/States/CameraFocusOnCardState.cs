using CardboardCore.DI;
using CardboardCore.EC;
using DungeonCrawler.EC.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public class CameraFocusOnCardState : RoomBuilderState
    {
        [Inject] private EntityRegister entityRegister;

        protected override void OnEnter()
        {
            Entity cameraEntity = entityRegister.FindEntity("GameplayCameraEntity");
            cameraEntity.GetComponent<CameraTargetComponent>().SetTarget(roomCardDataComponent.owner, () => { owner.ToNextState(); });
        }

        protected override void OnExit()
        {

        }
    }
}

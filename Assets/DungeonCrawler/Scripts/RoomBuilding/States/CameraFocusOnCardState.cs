using CardboardCore.DI;
using CardboardCore.EntityComponents;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public class CameraFocusOnCardState : RoomBuilderState
    {
        [Inject] private EntityRegister entityRegister;

        protected override void OnEnter()
        {
            Entity cameraEntity = entityRegister.FindEntity("GameplayCameraEntity");
            cameraEntity.GetComponent<CameraTargetComponent>().SetTarget(roomCardDataComponent.owner);
        }

        protected override void OnExit()
        {

        }
    }
}
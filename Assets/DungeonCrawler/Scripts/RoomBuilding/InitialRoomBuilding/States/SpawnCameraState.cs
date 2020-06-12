using CardboardCore.DI;
using CardboardCore.EC;
using CardboardCore.StateMachines;
using DungeonCrawler.EC;
using DungeonCrawler.EC.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public class SpawnCameraState : State
    {
        private CameraViewComponent cameraViewComponent;

        protected override void OnEnter()
        {
            Entity cameraEntity = new GameplayEntityFactory().Instantiate("GameplayCameraEntity");

            cameraViewComponent = cameraEntity.GetComponent<CameraViewComponent>();
            cameraViewComponent.LoadFinishedEvent += OnCameraViewLoadFinished;
            cameraViewComponent.Load();
        }

        protected override void OnExit()
        {
        }

        private void OnCameraViewLoadFinished(ViewComponent viewComponent)
        {
            cameraViewComponent.LoadFinishedEvent -= OnCameraViewLoadFinished;
            owner.ToNextState();
        }
    }
}

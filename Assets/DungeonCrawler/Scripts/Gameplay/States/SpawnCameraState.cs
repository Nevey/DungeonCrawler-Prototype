using CardboardCore.DI;
using CardboardCore.EntityComponents;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.Gameplay.States
{
    public class SpawnCameraState : State
    {
        [Inject] private EntityRegister entityRegister;

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
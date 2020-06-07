using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CameraRotateAroundComponent : Component
    {
        private CameraTargetComponent cameraTargetComponent;
        private PositionComponent positionComponent;
        private RotationComponent rotationComponent;

        public CameraRotateAroundComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            cameraTargetComponent = GetComponent<CameraTargetComponent>();
            positionComponent = GetComponent<PositionComponent>();
            rotationComponent = GetComponent<RotationComponent>();
        }
    }
}
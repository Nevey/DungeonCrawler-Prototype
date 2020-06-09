using CardboardCore.EntityComponents;
using CardboardCore.Utilities;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CameraRotateAroundComponent : Component
    {
        private CameraTargetComponent cameraTargetComponent;
        private PositionComponent positionComponent;
        private RotationComponent rotationComponent;

        private float rotation;

        public CameraRotateAroundComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            cameraTargetComponent = GetComponent<CameraTargetComponent>();

            positionComponent = GetComponent<PositionComponent>();
            rotationComponent = GetComponent<RotationComponent>();
        }

        protected override void OnUpdate(float deltaTime)
        {
            rotation += 10f * deltaTime;

            SetPosition(cameraTargetComponent.target);
        }

        private void SetPosition(PositionComponent target)
        {
            if (target == null)
            {
                return;
            }

            UnityEngine.Vector3 offset = new UnityEngine.Vector3(0f, 4.5f, 5f);
            UnityEngine.Vector3 relativePosition = RotationUtil.GetVectorSimple(0f, rotation, 0f, offset);

            positionComponent.SetPosition(target.position + relativePosition);
        }
    }
}
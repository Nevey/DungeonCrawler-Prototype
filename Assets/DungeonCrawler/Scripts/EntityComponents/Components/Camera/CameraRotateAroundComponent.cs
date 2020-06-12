using CardboardCore.EC;
using CardboardCore.Utilities;

namespace DungeonCrawler.EC.Components
{
    public class CameraRotateAroundComponent : Component
    {
        [TweakableField] private float rotation;
        [TweakableField] private bool isRotating;
        [TweakableField] private float rotationSpeed;

        private CameraTargetComponent cameraTargetComponent;
        private PositionComponent positionComponent;
        private RotationComponent rotationComponent;

        protected override void OnStart()
        {
            cameraTargetComponent = GetComponent<CameraTargetComponent>();
            cameraTargetComponent.TargetUpdatedEvent += OnCameraTargetUpdated;

            positionComponent = GetComponent<PositionComponent>();
            rotationComponent = GetComponent<RotationComponent>();
        }

        protected override void OnStop()
        {
            cameraTargetComponent.TargetUpdatedEvent -= OnCameraTargetUpdated;
        }

        private void OnCameraTargetUpdated(PositionComponent positionComponent)
        {
            SetPosition(positionComponent);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (!isRotating)
            {
                return;
            }

            rotation += rotationSpeed * deltaTime;
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
            UnityEngine.Vector3 targetPosition = target.position + relativePosition;

            positionComponent.SetPositionAnimated(targetPosition);
        }
    }
}

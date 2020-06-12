using System;
using CardboardCore.EC;
using DG.Tweening;

namespace DungeonCrawler.EC.Components
{
    public class CameraTargetComponent : Component
    {
        private CameraViewComponent cameraViewComponent;

        private Tween tween;
        private UnityEngine.Vector3 currentTargetPosition;

        public PositionComponent target { get; private set; }

        public event Action<PositionComponent> TargetUpdatedEvent;

        protected override void OnStart()
        {
            cameraViewComponent = GetComponent<CameraViewComponent>();
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (target == null)
            {
                return;
            }

            cameraViewComponent.LookAt(currentTargetPosition);
        }

        public void SetTarget(PositionComponent target, TweenCallback callback = null)
        {
            this.target = target;

            tween?.Kill();
            tween = DOTween.To(() => currentTargetPosition, x => currentTargetPosition = x, target.position, 1f);
            tween.SetEase(Ease.InOutQuad);
            tween.OnComplete(callback);
            tween.Play();

            TargetUpdatedEvent?.Invoke(target);
        }

        public void SetTarget(Entity entity, TweenCallback callback = null)
        {
            PositionComponent positionComponent = entity.GetComponent<PositionComponent>(true);
            SetTarget(positionComponent, callback);
        }
    }
}

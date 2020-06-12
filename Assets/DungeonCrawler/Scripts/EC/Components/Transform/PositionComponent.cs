using System;
using CardboardCore.EC;
using DG.Tweening;

namespace DungeonCrawler.EC.Components
{
    public class PositionComponent : Component
    {
        public UnityEngine.Vector3 position { get; protected set; }

        private Tween movementTween;

        public event Action<UnityEngine.Vector3> PositionUpdatedEvent;

        protected void DispatchPositionUpdatedEvent()
        {
            PositionUpdatedEvent?.Invoke(position);
        }

        public void SetPosition(UnityEngine.Vector3 position)
        {
            this.position = position;
            DispatchPositionUpdatedEvent();
        }

        public void SetPosition(float x, float y, float z)
        {
            position = new UnityEngine.Vector3(x, y, z);
            DispatchPositionUpdatedEvent();
        }

        public Tween SetPositionAnimated(UnityEngine.Vector3 targetPosition, float duration = 1f, Ease ease = Ease.InOutQuad)
        {
            movementTween?.Kill();
            movementTween = DOTween.To(() => position, x => position = x, targetPosition, duration);
            movementTween.SetEase(ease);
            movementTween.OnUpdate(DispatchPositionUpdatedEvent);
            movementTween.Play();

            return movementTween;
        }

        public Tween SetPositionAnimated(float x, float y, float z, float duration = 1f, Ease ease = Ease.InOutQuad)
        {
            UnityEngine.Vector3 targetPosition = new UnityEngine.Vector3(x, y, z);
            return SetPositionAnimated(targetPosition, duration, ease);
        }
    }
}

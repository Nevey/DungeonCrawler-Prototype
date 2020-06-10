using System;
using CardboardCore.EntityComponents;
using DG.Tweening;

namespace DungeonCrawler.EntityComponents.Components
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

        public void SetPositionAnimated(UnityEngine.Vector3 targetPosition)
        {
            movementTween?.Kill();
            movementTween = DOTween.To(() => position, x => position = x, targetPosition, 1f);
            movementTween.SetEase(Ease.InOutQuad);
            movementTween.OnUpdate(DispatchPositionUpdatedEvent);
            movementTween.Play();
        }

        public void SetPositionAnimated(float x, float y, float z)
        {
            UnityEngine.Vector3 targetPosition = new UnityEngine.Vector3(x, y, z);
            SetPositionAnimated(targetPosition);

        }
    }
}

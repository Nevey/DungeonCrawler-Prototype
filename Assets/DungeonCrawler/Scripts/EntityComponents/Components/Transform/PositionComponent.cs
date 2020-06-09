using System;
using CardboardCore.EntityComponents;
using DG.Tweening;

namespace DungeonCrawler.EntityComponents.Components
{
    public class PositionComponent : Component
    {
        [TweakableField] private float hopHeight = 0.5f;

        private Tween movementTween;
        private Sequence hopSequence;

        public UnityEngine.Vector3 position { get; private set; }

        public event Action<UnityEngine.Vector3> PositionUpdatedEvent;
        public event Action MovementAnimationFinishedEvent;

        public PositionComponent(Entity owner) : base(owner)
        {
        }

        private void DispatchPositionUpdatedEvent()
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

        public void HopToPosition(float x, float y, float z, float duration, TweenCallback callback)
        {
            UnityEngine.Vector3 targetPosition = new UnityEngine.Vector3(x, y, z);

            movementTween?.Kill();
            movementTween = DOTween.To(() => position, p => position = p, targetPosition, duration);
            movementTween.OnUpdate(DispatchPositionUpdatedEvent);
            movementTween.OnComplete(() =>
            {
                MovementAnimationFinishedEvent?.Invoke();
                callback?.Invoke();
            });
            movementTween.Play();


            float height = position.y;

            hopSequence?.Kill();
            hopSequence = DOTween.Sequence();

            Tween jumpTween = DOTween.To(() => height, tHeight => height = tHeight, hopHeight, duration * 0.5f);
            jumpTween.SetEase(Ease.OutQuad);
            hopSequence.Append(jumpTween);

            Tween landTween = DOTween.To(() => height, tHeight => height = tHeight, y, duration * 0.5f);
            landTween.SetEase(Ease.InQuad);
            hopSequence.Insert(duration * 0.5f, landTween);

            hopSequence.OnUpdate(() =>
            {
                UnityEngine.Vector3 p = position;
                p.y = height;
                position = p;

                DispatchPositionUpdatedEvent();
            });

            hopSequence.Play();
        }
    }
}
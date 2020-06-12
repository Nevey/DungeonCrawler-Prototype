using System;
using CardboardCore.EC;
using DG.Tweening;

namespace DungeonCrawler.EC.Components
{
    public class RotationComponent : Component
    {
        private Tween rotationTween;

        public UnityEngine.Quaternion rotation { get; private set; }
        public UnityEngine.Vector3 euler => rotation.eulerAngles;

        public event Action<UnityEngine.Quaternion> RotationUpdatedEvent;

        private void DispatchRotationUpdatedEvent()
        {
            RotationUpdatedEvent?.Invoke(rotation);
        }

        public void SetRotation(UnityEngine.Quaternion rotation, bool @override = false)
        {
            this.rotation = rotation;

            // (Temp) Solution to not write pre-invented rotation code
            if (@override)
            {
                return;
            }

            DispatchRotationUpdatedEvent();
        }

        public void SetRotation(UnityEngine.Vector3 euler)
        {
            SetRotation(UnityEngine.Quaternion.Euler(euler));
        }

        public void SetRotation(float x, float y, float z)
        {
            UnityEngine.Vector3 euler = new UnityEngine.Vector3(x, y, z);
            SetRotation(euler);
        }

        public void SetRandomRotation()
        {
            SetRotation(UnityEngine.Random.rotation);
        }

        public void SetRandomRotationX()
        {
            UnityEngine.Quaternion randomRotation = UnityEngine.Random.rotation;
            UnityEngine.Vector3 eulerAngles = new UnityEngine.Vector3(randomRotation.eulerAngles.x, euler.y, euler.z);
            SetRotation(eulerAngles);
        }

        public void SetRandomRotationY()
        {
            UnityEngine.Quaternion randomRotation = UnityEngine.Random.rotation;
            UnityEngine.Vector3 eulerAngles = new UnityEngine.Vector3(euler.x, randomRotation.eulerAngles.y, euler.z);

            SetRotation(eulerAngles);
        }

        public void SetRandomRotationZ()
        {
            UnityEngine.Quaternion randomRotation = UnityEngine.Random.rotation;
            UnityEngine.Vector3 eulerAngles = new UnityEngine.Vector3(euler.x, euler.y, randomRotation.eulerAngles.z);

            SetRotation(eulerAngles);
        }

        public Tween SetRotationAnimated(UnityEngine.Vector3 targetEuler, float duration = 1f, Ease ease = Ease.InOutQuad)
        {
            rotationTween?.Kill();
            rotationTween = DOTween.To(() => rotation, x => rotation = x, targetEuler, duration);
            rotationTween.SetEase(ease);
            rotationTween.OnUpdate(DispatchRotationUpdatedEvent);
            rotationTween.Play();

            return rotationTween;
        }

        public Tween SetRotationAnimated(UnityEngine.Quaternion rotation, float duration = 1f, Ease ease = Ease.InOutQuad)
        {
            UnityEngine.Vector3 targetEuler = rotation.eulerAngles;
            return SetRotationAnimated(targetEuler, duration, ease);
        }

        public Tween SetRotationAnimated(float x, float y, float z, float duration = 1f, Ease ease = Ease.InOutQuad)
        {
            UnityEngine.Vector3 targetEuler = new UnityEngine.Vector3(x, y, z);
            return SetRotationAnimated(targetEuler, duration, ease);
        }
    }
}

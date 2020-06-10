using System;
using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class RotationComponent : Component
    {
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

        public void SetRandomRotationX(float y = 0f, float z = 0f)
        {
            UnityEngine.Quaternion randomRotation = UnityEngine.Random.rotation;
            UnityEngine.Vector3 euler = new UnityEngine.Vector3(randomRotation.eulerAngles.x, y, z);

            SetRotation(euler);
        }

        public void SetRandomRotationY(float x = 0f, float z = 0f)
        {
            UnityEngine.Quaternion randomRotation = UnityEngine.Random.rotation;
            UnityEngine.Vector3 euler = new UnityEngine.Vector3(x, randomRotation.eulerAngles.y, z);

            SetRotation(euler);
        }

        public void SetRandomRotationZ(float x = 0f, float y = 0f)
        {
            UnityEngine.Quaternion randomRotation = UnityEngine.Random.rotation;
            UnityEngine.Vector3 euler = new UnityEngine.Vector3(x, y, randomRotation.eulerAngles.z);

            SetRotation(euler);
        }
    }
}
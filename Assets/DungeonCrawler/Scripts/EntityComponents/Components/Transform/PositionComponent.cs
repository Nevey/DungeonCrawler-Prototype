using System;
using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class PositionComponent : Component
    {
        public UnityEngine.Vector3 position { get; protected set; }

        public event Action<UnityEngine.Vector3> PositionUpdatedEvent;

        public PositionComponent(Entity owner) : base(owner)
        {
        }

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
    }
}
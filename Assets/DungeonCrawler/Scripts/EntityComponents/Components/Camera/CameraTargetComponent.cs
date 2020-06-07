using System;
using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CameraTargetComponent : Component
    {
        private CameraViewComponent cameraViewComponent;
        public ViewComponent target { get; private set; }

        public event Action<ViewComponent> TargetUpdatedEvent;

        public CameraTargetComponent(Entity owner) : base(owner)
        {
        }

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

            cameraViewComponent.LookAt(target);
        }

        public void SetTarget(ViewComponent target)
        {
            this.target = target;
            TargetUpdatedEvent?.Invoke(target);
        }

        public void SetTarget(Entity entity)
        {
            ViewComponent targetViewComponent = entity.GetComponent<ViewComponent>(true);
            SetTarget(targetViewComponent);
        }
    }
}
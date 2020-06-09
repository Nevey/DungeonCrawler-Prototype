using System;
using CardboardCore.EntityComponents;
using DG.Tweening;

namespace DungeonCrawler.EntityComponents.Components
{
    public class PlayerViewComponent : ViewComponent
    {
        [TweakableField] private float moveTweenDuration;

        private GridPositionComponent gridPositionComponent;

        public event Action MovementFinishedEvent;

        public PlayerViewComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            base.OnStart();

            gridPositionComponent = GetComponent<GridPositionComponent>();
            gridPositionComponent.PositionUpdatedEvent += OnGridPositionUpdated;
        }

        protected override void OnStop()
        {
            base.OnStop();

            gridPositionComponent.PositionUpdatedEvent -= OnGridPositionUpdated;
        }

        private void OnGridPositionUpdated(int x, int y)
        {
            positionComponent.HopToPosition(x, 0f, y, moveTweenDuration, () =>
            {
                MovementFinishedEvent?.Invoke();
            });
        }
    }
}
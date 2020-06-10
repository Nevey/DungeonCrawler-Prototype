using System;
using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class PlayerViewComponent : ViewComponent
    {
        [TweakableField] private float moveTweenDuration;

        private GridPositionComponent gridPositionComponent;
        private new HoppingPositionComponent positionComponent;

        public event Action MovementFinishedEvent;

        protected override void OnStart()
        {
            base.OnStart();

            gridPositionComponent = GetComponent<GridPositionComponent>();
            gridPositionComponent.PositionUpdatedEvent += OnGridPositionUpdated;

            positionComponent = GetComponent<HoppingPositionComponent>();
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
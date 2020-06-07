using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class PlayerViewComponent : ViewComponent
    {
        private GridPositionComponent gridPositionComponent;

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
            positionComponent.SetPosition(x, 0f, y);
        }
    }
}
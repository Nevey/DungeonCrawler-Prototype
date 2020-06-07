using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CardViewComponent : ViewComponent
    {
        private GridPositionComponent gridPositionComponent;

        public CardViewComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            base.OnStart();
            LoadFinishedEvent += OnLoadFinished;

            gridPositionComponent = GetComponent<GridPositionComponent>();
        }

        protected override void OnStop()
        {
            base.OnStop();
            LoadFinishedEvent -= OnLoadFinished;
        }

        private void OnLoadFinished()
        {
            PlaceOnGridBacksideUp();
        }

        private void PlaceOnGridBacksideUp()
        {
            positionComponent.SetPosition(gridPositionComponent.x, 0.1f, gridPositionComponent.y);
            rotationComponent.SetRandomRotationZ(90f);
        }
    }
}
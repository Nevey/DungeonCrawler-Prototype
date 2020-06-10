using CardboardCore.EntityComponents;
using CardboardCore.Utilities;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CardViewComponent : ViewComponent
    {
        private GridPositionComponent gridPositionComponent;

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

        private void OnLoadFinished(ViewComponent viewComponent)
        {
            PlaceOnGridBacksideUp();
        }

        private void PlaceOnGridBacksideUp()
        {
            positionComponent.SetPosition(gridPositionComponent.x, 0.1f, gridPositionComponent.y);

            rotationComponent.SetRotation(90f, 0f, 0f);
            rotationComponent.SetRandomRotationZ();
        }



        public void PlayPickupAnimation()
        {
            // camera focus on card
            // move up and rotate to 0 0 0
            // rotate towards camera
            // move to camera
        }
    }
}
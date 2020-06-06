using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CardViewComponent : ViewComponent
    {
        public CardViewComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            base.OnStart();

            LoadFinishedEvent += OnLoadFinished;
        }

        protected override void OnStop()
        {
            base.OnStop();

            LoadFinishedEvent -= OnLoadFinished;
        }

        private void OnLoadFinished()
        {
            PlaceOnGridBackUp();
        }

        private void PlaceOnGridBackUp()
        {
            gameObject.transform.position = new UnityEngine.Vector3(
                positionComponent.x,
                0.1f,
                positionComponent.y
            );

            UnityEngine.Quaternion randomRotation = UnityEngine.Random.rotation;

            gameObject.transform.eulerAngles = new UnityEngine.Vector3(
                90f,
                0f,
                randomRotation.eulerAngles.z);
        }
    }
}
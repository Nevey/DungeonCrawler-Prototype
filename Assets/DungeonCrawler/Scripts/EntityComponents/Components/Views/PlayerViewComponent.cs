using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class PlayerViewComponent : ViewComponent
    {
        public PlayerViewComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}
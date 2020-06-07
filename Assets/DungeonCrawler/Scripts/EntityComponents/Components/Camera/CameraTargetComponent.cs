using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CameraTargetComponent : Component
    {
        private ViewComponent target;

        public CameraTargetComponent(Entity owner) : base(owner)
        {
        }
    }
}
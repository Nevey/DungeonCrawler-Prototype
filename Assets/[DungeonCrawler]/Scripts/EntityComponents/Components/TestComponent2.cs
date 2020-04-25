using CardboardCore.EntityComponents;
using CardboardCore.Utilities;

namespace DungeonCrawler.EntityComponents.Components
{
    public class TestComponent2 : Component
    {
        [TweakableField] private bool randomBoolean;

        protected override void OnStart()
        {
            Log.Write(randomBoolean);
        }

        protected override void OnUpdate(double deltaTime)
        {

        }
    }
}

using CardboardCore.EntityComponents;
using CardboardCore.Utilities;

namespace DungeonCrawler.EntityComponents.Components
{
    public class TestComponent : Component
    {
        [TweakableField] private int randomInteger;

        [TweakableField] private string randomString;

        protected override void OnStart()
        {
            Log.Write(randomInteger);
        }

        protected override void OnUpdate(double deltaTime)
        {
            randomInteger++;
            Log.Write(deltaTime);
        }
    }
}

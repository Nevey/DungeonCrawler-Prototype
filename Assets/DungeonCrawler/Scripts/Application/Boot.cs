using CardboardCore.DI;
using CardboardCore.Loop;

namespace DungeonCrawler.Application
{
    public class Boot : CardboardCoreBehaviour
    {
        private UpdateLoop updateLoop;
        private ApplicationStateMachine applicationStateMachine = new ApplicationStateMachine();

        protected override void Start()
        {
            base.Start();

            updateLoop = gameObject.AddComponent<UpdateLoop>();

            DontDestroyOnLoad(gameObject);

            applicationStateMachine.Start();
        }

        protected override void OnDestroy()
        {
            applicationStateMachine.Stop();

            base.OnDestroy();
        }
    }
}

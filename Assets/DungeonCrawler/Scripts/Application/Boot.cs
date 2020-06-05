using CardboardCore.DI;
using CardboardCore.Loop;

namespace DungeonCrawler.Application
{
    public class Boot : CardboardCoreBehaviour
    {
        [Inject] private UpdateLoop updateLoop;

        private ApplicationStateMachine applicationStateMachine = new ApplicationStateMachine();

        protected override void Start()
        {
            base.Start();

            DontDestroyOnLoad(gameObject);

            applicationStateMachine.Start();
        }

        protected override void OnDestroy()
        {
            applicationStateMachine.Stop();

            updateLoop.StopLoop();

            base.OnDestroy();
        }
    }
}

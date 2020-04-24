using CardboardCore.DI;
using CardboardCore.EntityComponents;
using CardboardCore.Loop;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents;

namespace DungeonCrawler.Application.States
{
    public class BootState : State
    {
        [Inject] private GameEntityFactory factory;

        protected override void OnEnter()
        {
            Entity entity = factory.Instantiate("test");
        }

        protected override void OnExit()
        {

        }
    }
}

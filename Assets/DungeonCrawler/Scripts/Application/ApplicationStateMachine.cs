using CardboardCore.StateMachines;
using DungeonCrawler.Application.States;

namespace DungeonCrawler.Application
{
    public class ApplicationStateMachine : StateMachine
    {
        public ApplicationStateMachine()
        {
            SetInitialState<BootState>();
            AddTransition<BootState, GameplayState>();
        }
    }
}

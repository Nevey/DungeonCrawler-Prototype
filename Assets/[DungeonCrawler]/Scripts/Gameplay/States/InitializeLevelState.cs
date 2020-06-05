using CardboardCore.DI;
using CardboardCore.StateMachines;
using DungeonCrawler.Levels;

namespace DungeonCrawler.Gameplay.States
{
    public class InitializeLevelState : State
    {
        [Inject] private LevelManager levelManager;

        protected override void OnEnter()
        {
            levelManager.CreateInitialRoom();
        }

        protected override void OnExit()
        {

        }
    }
}
using CardboardCore.EntityComponents;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.Gameplay.States
{
    public class InitializeLevelState : State
    {
        private GameplayEntityFactory gameplayEntityFactory = new GameplayEntityFactory();

        private Entity levelEntity;

        protected override void OnEnter()
        {
            levelEntity = gameplayEntityFactory.Instantiate("LevelEntity");
            levelEntity.GetComponent<LevelBuilderComponent>().CreateInitialRoom();

            // TODO: Wait for all players to have their level loaded
            owner.ToNextState();
        }

        protected override void OnExit()
        {

        }
    }
}
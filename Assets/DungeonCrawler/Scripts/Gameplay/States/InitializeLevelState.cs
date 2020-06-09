using System;
using CardboardCore.DI;
using CardboardCore.EntityComponents;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.Gameplay.States
{
    public class InitializeLevelState : State
    {
        [Inject] private EntityRegister entityRegister;

        private GameplayEntityFactory gameplayEntityFactory = new GameplayEntityFactory();

        private Entity levelEntity;

        protected override void OnEnter()
        {
            levelEntity = gameplayEntityFactory.Instantiate("LevelEntity");

            LevelBuilderComponent levelBuilderComponent = levelEntity.GetComponent<LevelBuilderComponent>();
            levelBuilderComponent.LevelBuildingFinishedEvent += OnLevelBuildingFinished;
            levelBuilderComponent.CreateInitialRoom();

            // TODO: Wait for all players to have their level loaded
            owner.ToNextState();
        }

        protected override void OnExit()
        {

        }

        private void OnLevelBuildingFinished(RoomDataComponent roomDataComponent)
        {
            // roomDataComponent.LevelBuildingFinishedEvent -= OnLevelBuildingFinished;

            CameraTargetComponent cameraTargetComponent = entityRegister.FindEntity("GameplayCameraEntity").GetComponent<CameraTargetComponent>();
            cameraTargetComponent.SetTarget(roomDataComponent.owner);
        }
    }
}
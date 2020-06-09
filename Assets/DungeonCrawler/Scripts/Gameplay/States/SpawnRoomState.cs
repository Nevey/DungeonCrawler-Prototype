using CardboardCore.DI;
using CardboardCore.EntityComponents;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.Gameplay.States
{
    public class SpawnRoomState : State
    {
        [Inject] private EntityRegister entityRegister;

        private GameplayEntityFactory gameplayEntityFactory = new GameplayEntityFactory();

        private RoomBuilderComponent roomBuilderComponent;

        protected override void OnEnter()
        {
            Entity levelEntity = gameplayEntityFactory.Instantiate("LevelEntity");

            roomBuilderComponent = levelEntity.GetComponent<RoomBuilderComponent>();
            roomBuilderComponent.LevelBuildingFinishedEvent += OnLevelBuildingFinished;
            roomBuilderComponent.CreateInitialRoom(); // TODO: spawn room based on room card
        }

        protected override void OnExit()
        {

        }

        private void OnLevelBuildingFinished(RoomDataComponent roomDataComponent)
        {
            roomBuilderComponent.LevelBuildingFinishedEvent -= OnLevelBuildingFinished;

            CameraTargetComponent cameraTargetComponent = entityRegister.FindEntity("GameplayCameraEntity").GetComponent<CameraTargetComponent>();
            cameraTargetComponent.SetTarget(roomDataComponent.owner);

            // TODO: Wait for all players to have their level loaded
            owner.ToNextState();
        }
    }
}
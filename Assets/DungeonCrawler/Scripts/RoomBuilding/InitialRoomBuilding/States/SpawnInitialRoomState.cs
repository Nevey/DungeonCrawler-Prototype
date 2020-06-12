using CardboardCore.DI;
using CardboardCore.EC;
using CardboardCore.StateMachines;
using DungeonCrawler.EC;
using DungeonCrawler.EC.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public class SpawnInitialRoomState : State
    {
        [Inject] private EntityRegister entityRegister;

        private GameplayEntityFactory gameplayEntityFactory = new GameplayEntityFactory();

        private RoomBuilderComponent roomBuilderComponent;

        protected override void OnEnter()
        {
            Entity levelEntity = gameplayEntityFactory.Instantiate("LevelEntity");

            roomBuilderComponent = levelEntity.GetComponent<RoomBuilderComponent>();
            roomBuilderComponent.AreaBuildingFinishedEvent += OnLevelBuildingFinished;
            roomBuilderComponent.CreateInitialRoom();
        }

        protected override void OnExit()
        {

        }

        private void OnLevelBuildingFinished(RoomDataComponent roomDataComponent)
        {
            roomBuilderComponent.AreaBuildingFinishedEvent -= OnLevelBuildingFinished;

            CameraTargetComponent cameraTargetComponent = entityRegister.FindEntity("GameplayCameraEntity").GetComponent<CameraTargetComponent>();
            cameraTargetComponent.SetTarget(roomDataComponent.owner);

            // TODO: Wait for all players to have their level loaded
            owner.ToNextState();
        }
    }
}

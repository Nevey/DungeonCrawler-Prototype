using CardboardCore.DI;
using CardboardCore.EntityComponents;
using DungeonCrawler.EntityComponents.Components;
using UnityEngine;

namespace DungeonCrawler.RoomBuilding.States
{
    public class CreateCorridorAndRoomState : RoomBuilderState
    {
        [Inject] private EntityRegister entityRegister;

        private CorridorBuilderComponent corridorBuilderComponent;
        private RoomBuilderComponent roomBuilderComponent;
        private CameraTargetComponent cameraTargetComponent;

        private Vector3 spawnOffset;
        private Vector2Int spawnDirection;

        protected override void OnEnter()
        {
            Entity levelEntity = entityRegister.FindEntity("LevelEntity");

            corridorBuilderComponent = levelEntity.GetComponent<CorridorBuilderComponent>();
            roomBuilderComponent = levelEntity.GetComponent<RoomBuilderComponent>();

            cameraTargetComponent = entityRegister.FindEntity("GameplayCameraEntity").GetComponent<CameraTargetComponent>();

            corridorBuilderComponent.AreaBuildingFinishedEvent += OnCorridorBuildingFinished;
            corridorBuilderComponent.CreateCorridor(currentRoom, x, y, out spawnOffset, out spawnDirection);
        }

        protected override void OnExit()
        {

        }

        private void OnCorridorBuildingFinished(RoomDataComponent obj)
        {
            corridorBuilderComponent.AreaBuildingFinishedEvent -= OnCorridorBuildingFinished;

            roomBuilderComponent.AreaBuildingFinishedEvent += OnRoomBuildingFinished;
            roomBuilderComponent.CreateRoom(roomCardDataComponent.GetCardData().id, spawnOffset, spawnDirection);
        }

        private void OnRoomBuildingFinished(RoomDataComponent obj)
        {
            roomBuilderComponent.AreaBuildingFinishedEvent -= OnRoomBuildingFinished;

            CardViewComponent cardViewComponent = roomCardDataComponent.GetComponent<CardViewComponent>();
            cardViewComponent.PlayPlacementAnimation(obj.owner, PlayRoomBuildAnimation);

            cameraTargetComponent.SetTarget(obj.GetComponent<PositionComponent>());
        }

        private void PlayRoomBuildAnimation()
        {
            roomBuilderComponent.PlayRoomBuildAnimation(owner.ToNextState);
        }
    }
}

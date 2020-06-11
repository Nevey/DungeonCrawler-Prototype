using CardboardCore.DI;
using CardboardCore.EntityComponents;
using DungeonCrawler.EntityComponents.Components;
using UnityEngine;

namespace DungeonCrawler.RoomBuilding.States
{
    public class CreateCorridorAndRoomState : RoomBuilderState
    {
        [Inject] private EntityRegister entityRegister;

        protected override void OnEnter()
        {
            Entity levelEntity = entityRegister.FindEntity("LevelEntity");

            RoomBuilderComponent roomBuilderComponent = levelEntity.GetComponent<RoomBuilderComponent>();
            CorridorBuilderComponent corridorBuilderComponent = levelEntity.GetComponent<CorridorBuilderComponent>();

            if (corridorBuilderComponent.CreateCorridor(currentRoom, x, y, out Vector3 spawnOffset, out Vector2Int spawnDirection))
            {
                roomBuilderComponent.CreateRoom(roomCardDataComponent.GetCardData().id, spawnOffset, spawnDirection);
            }
        }

        protected override void OnExit()
        {

        }
    }
}

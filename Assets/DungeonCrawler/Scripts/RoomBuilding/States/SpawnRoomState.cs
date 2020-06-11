using CardboardCore.DI;
using CardboardCore.EntityComponents;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public class SpawnRoomState : RoomBuilderState
    {
        [Inject] private EntityRegister entityRegister;

        protected override void OnEnter()
        {
            Entity levelEntity = entityRegister.FindEntity("LevelEntity");
            RoomBuilderComponent roomBuilderComponent = levelEntity.GetComponent<RoomBuilderComponent>();
        }

        protected override void OnExit()
        {
        }
    }
}
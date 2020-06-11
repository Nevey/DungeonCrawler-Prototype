using CardboardCore.DI;
using CardboardCore.EntityComponents;
using DungeonCrawler.Cards;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public class CreateCorridorState : RoomBuilderState
    {
        [Inject] private EntityRegister entityRegister;

        protected override void OnEnter()
        {
            Entity levelEntity = entityRegister.FindEntity("LevelEntity");

            RoomBuilderComponent roomBuilderComponent = levelEntity.GetComponent<RoomBuilderComponent>();
            roomBuilderComponent.CreateCorridor(currentRoom, x, y);
        }

        protected override void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }
}

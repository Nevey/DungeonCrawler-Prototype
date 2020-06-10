using CardboardCore.DI;
using CardboardCore.EntityComponents;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public class AnimateCardState : RoomBuilderState
    {
        [Inject] private EntityRegister entityRegister;

        protected override void OnEnter()
        {
            Entity entity = entityRegister.FindEntity("GameplayCameraEntity");

            CardViewComponent cardViewComponent = roomCardDataComponent.GetComponent<CardViewComponent>();
            cardViewComponent.PlayPickupAnimation(entity);
        }

        protected override void OnExit()
        {

        }
    }
}

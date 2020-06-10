using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public class AnimateCardState : RoomBuilderState
    {
        protected override void OnEnter()
        {
            ViewComponent viewComponent = roomCardDataComponent.GetComponent<ViewComponent>();
        }

        protected override void OnExit()
        {

        }
    }
}
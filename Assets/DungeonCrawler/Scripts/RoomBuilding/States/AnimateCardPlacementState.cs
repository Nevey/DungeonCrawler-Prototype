using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public class AnimateCardPlacementState : RoomBuilderState
    {
        protected override void OnEnter()
        {
            CardViewComponent cardViewComponent = roomCardDataComponent.GetComponent<CardViewComponent>();
            // Get current tile, get it's room, get all free tiles surrounding the doorway tile, start trying to spawn room and keep trying until a proper fit was found
        }

        protected override void OnExit()
        {
        }
    }
}

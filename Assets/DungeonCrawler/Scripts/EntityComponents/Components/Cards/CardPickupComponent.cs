using CardboardCore.EntityComponents;
using CardboardCore.Utilities;

namespace DungeonCrawler.EntityComponents.Components
{
    public class CardPickupComponent : Component
    {
        private GridPositionComponent gridPositionComponent;
        private HoppingPositionComponent hoppingPositionComponent;
        private RoomAwarenessComponent roomAwarenessComponent;
        private MovementInputComponent movementInputComponent;

        protected override void OnStart()
        {
            gridPositionComponent = GetComponent<GridPositionComponent>();
            hoppingPositionComponent = GetComponent<HoppingPositionComponent>();
            roomAwarenessComponent = GetComponent<RoomAwarenessComponent>();
            movementInputComponent = GetComponent<MovementInputComponent>();

            hoppingPositionComponent.MovementAnimationFinishedEvent += OnMovementFinished;
        }

        protected override void OnStop()
        {
            hoppingPositionComponent.MovementAnimationFinishedEvent -= OnMovementFinished;
        }

        private void OnMovementFinished()
        {
            if (roomAwarenessComponent.GetRoomCardAtGridLocation(gridPositionComponent.x, gridPositionComponent.y,
                out RoomCardDataComponent roomCardDataComponent))
            {
                movementInputComponent.Lock();

                CardViewComponent cardViewComponent = roomCardDataComponent.GetComponent<CardViewComponent>();
                cardViewComponent.PlayPickupAnimation();
            }
        }
    }
}
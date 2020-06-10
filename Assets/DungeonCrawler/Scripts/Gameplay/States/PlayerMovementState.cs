using CardboardCore.DI;
using CardboardCore.EntityComponents;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents.Components;
using DungeonCrawler.RoomBuilding;

namespace DungeonCrawler.Gameplay.States
{
    public class PlayerMovementState : State
    {
        [Inject] private EntityRegister entityRegister;

        private MovementInputComponent movementInputComponent;
        private CardPickupComponent cardPickupComponent;

        protected override void OnEnter()
        {
            // TODO: Once multiplayer(ed), figure out if everyone is allowed to move at the same time, or if it's turn based
            // TODO: Receive other player's movement, send local player movement

            Entity playerEntity = entityRegister.FindEntity("PlayerEntity");

            movementInputComponent = playerEntity.GetComponent<MovementInputComponent>();
            movementInputComponent.SetGameplayCameraEntity(entityRegister.FindEntity("GameplayCameraEntity"));
            movementInputComponent.Bind();

            cardPickupComponent = playerEntity.GetComponent<CardPickupComponent>();
            cardPickupComponent.CardPickedUpEvent += OnCardPickedUp;
        }

        protected override void OnExit()
        {
            movementInputComponent.Unbind();
            cardPickupComponent.CardPickedUpEvent -= OnCardPickedUp;
        }

        private void OnCardPickedUp(CardDataComponent cardDataComponent)
        {
            movementInputComponent.Unbind();

            if (cardDataComponent is RoomCardDataComponent roomCardDataComponent)
            {
                RoomBuilderStateMachine roomBuilderStateMachine = new RoomBuilderStateMachine(roomCardDataComponent);
                roomBuilderStateMachine.Start();
            }
        }
    }
}

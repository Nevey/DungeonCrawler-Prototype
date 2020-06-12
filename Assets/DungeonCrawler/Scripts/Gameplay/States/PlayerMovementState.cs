using CardboardCore.DI;
using CardboardCore.EC;
using CardboardCore.StateMachines;
using DungeonCrawler.EC.Components;
using DungeonCrawler.RoomBuilding;
using DungeonCrawler.UserInput;

namespace DungeonCrawler.Gameplay.States
{
    public class PlayerMovementState : State
    {
        [Inject] private EntityRegister entityRegister;
        [Inject] private InputManager inputManager;

        private Entity playerEntity;
        private MovementInputComponent movementInputComponent;
        private CardPickupComponent cardPickupComponent;
        private RoomAwarenessComponent roomAwarenessComponent;
        private GridPositionComponent gridPositionComponent;
        private RoomBuilderStateMachine roomBuilderStateMachine;

        protected override void OnEnter()
        {
            // TODO: Once multiplayer(ed), figure out if everyone is allowed to move at the same time, or if it's turn based
            // TODO: Receive other player's movement, send local player movement

            playerEntity = entityRegister.FindEntity("PlayerEntity");

            movementInputComponent = playerEntity.GetComponent<MovementInputComponent>();
            movementInputComponent.SetMovementActionSetController(inputManager.movementActionSetController);
            movementInputComponent.SetGameplayCameraEntity(entityRegister.FindEntity("GameplayCameraEntity"));
            movementInputComponent.EnableInput();

            roomAwarenessComponent = playerEntity.GetComponent<RoomAwarenessComponent>();
            gridPositionComponent = playerEntity.GetComponent<GridPositionComponent>();

            cardPickupComponent = playerEntity.GetComponent<CardPickupComponent>();
            cardPickupComponent.CardPickedUpEvent += OnCardPickedUp;
        }

        protected override void OnExit()
        {
            movementInputComponent.DisableInput();
            cardPickupComponent.CardPickedUpEvent -= OnCardPickedUp;
        }

        private void OnCardPickedUp(CardDataComponent cardDataComponent)
        {
            movementInputComponent.DisableInput();

            if (cardDataComponent is RoomCardDataComponent roomCardDataComponent)
            {
                roomBuilderStateMachine = new RoomBuilderStateMachine(
                    roomAwarenessComponent.currentRoom,
                    roomCardDataComponent,
                    gridPositionComponent.x,
                    gridPositionComponent.y);

                roomBuilderStateMachine.Start();
                roomBuilderStateMachine.StoppedEvent += OnRoomBuilderStateMachineStopped;
            }
        }

        private void OnRoomBuilderStateMachineStopped()
        {
            roomBuilderStateMachine.StoppedEvent -= OnRoomBuilderStateMachineStopped;
            roomBuilderStateMachine = null;

            movementInputComponent.EnableInput();

            CameraTargetComponent cameraTargetComponent = entityRegister.FindEntity("GameplayCameraEntity").GetComponent<CameraTargetComponent>();
            cameraTargetComponent.SetTarget(playerEntity);
        }
    }
}

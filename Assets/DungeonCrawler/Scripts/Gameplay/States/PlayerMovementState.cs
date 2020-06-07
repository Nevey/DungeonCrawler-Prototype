using CardboardCore.DI;
using CardboardCore.EntityComponents;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.Gameplay.States
{
    public class PlayerMovementState : State
    {
        [Inject] private EntityRegister entityRegister;

        private MovementInputComponent movementInputComponent;

        protected override void OnEnter()
        {
            // TODO: Once multiplayer(ed), figure out if everyone is allowed to move at the same time, or if it's turn based
            // TODO: Receive other player's movement, send local player movement

            Entity playerEntity = entityRegister.FindEntity("PlayerEntity");

            movementInputComponent = playerEntity.GetComponent<MovementInputComponent>();
            movementInputComponent.SetGameplayCameraEntity(entityRegister.FindEntity("GameplayCameraEntity"));
            movementInputComponent.Bind();
        }

        protected override void OnExit()
        {
            movementInputComponent.Unbind();
        }
    }
}
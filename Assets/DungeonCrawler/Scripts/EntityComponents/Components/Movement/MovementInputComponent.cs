using CardboardCore.EC;
using CardboardCore.Utilities;
using DungeonCrawler.UserInput;

namespace DungeonCrawler.EC.Components
{
    public class MovementInputComponent : CardboardCore.EC.Component
    {
        private MovementActionSetController movementActionSetController;
        private Entity gameplayCameraEntity;

        // Owner's components
        private HoppingPositionComponent hoppingPositionComponent;
        private GridPositionComponent gridPositionComponent;
        private RoomAwarenessComponent roomAwarenessComponent;

        // Other's components
        private RotationComponent cameraRotationComponent;

        protected override void OnStart()
        {
            hoppingPositionComponent = GetComponent<HoppingPositionComponent>();
            gridPositionComponent = GetComponent<GridPositionComponent>();
            roomAwarenessComponent = GetComponent<RoomAwarenessComponent>();

            hoppingPositionComponent.MovementAnimationFinishedEvent += OnMovementAnimationFinished;
        }

        protected override void OnStop()
        {
            base.OnStop();

            DisableInput();

            movementActionSetController.InputEvent -= OnMovementInput;
            hoppingPositionComponent.MovementAnimationFinishedEvent -= OnMovementAnimationFinished;
        }

        private void OnMovementAnimationFinished()
        {
            movementActionSetController.Bind();
        }

        private void OnMovementInput(object sender, MovementInputEventArgs e)
        {
            e = ModifyInputBasedOnCameraAngle(e);

            if (!roomAwarenessComponent.CanWalk(e))
            {
                return;
            }

            switch (e.inputDirection)
            {
                case InputDirection.Horizontal:
                    gridPositionComponent.UpdateX(e.strength);
                    break;

                case InputDirection.Vertical:
                    gridPositionComponent.UpdateY(e.strength);
                    break;
            }

            Log.Write($"New Position: {gridPositionComponent.x} - {gridPositionComponent.y}");

            movementActionSetController.Unbind();
        }

        private MovementInputEventArgs ModifyInputBasedOnCameraAngle(MovementInputEventArgs e)
        {
            if (gameplayCameraEntity == null)
            {
                return e;
            }

            float yAngle = cameraRotationComponent.euler.y;

            if (yAngle < 135f && yAngle >= 45f)
            {
                e.inputDirection = e.inputDirection.Swap();

                if (e.inputDirection == InputDirection.Vertical)
                {
                    e.strength *= -1;
                }
            }
            if (yAngle < 225f && yAngle >= 135f)
            {
                e.strength *= -1;
            }
            if (yAngle < 315f && yAngle >= 225f)
            {
                e.inputDirection = e.inputDirection.Swap();

                if (e.inputDirection == InputDirection.Horizontal)
                {
                    e.strength *= -1;
                }
            }

            return e;
        }

        public void SetMovementActionSetController(MovementActionSetController movementActionSetController)
        {
            movementActionSetController.InputEvent += OnMovementInput;

            this.movementActionSetController = movementActionSetController;
        }

        public void SetGameplayCameraEntity(Entity gameplayCameraEntity)
        {
            this.gameplayCameraEntity = gameplayCameraEntity;
            cameraRotationComponent = gameplayCameraEntity.GetComponent<RotationComponent>();
        }

        public void EnableInput()
        {
            movementActionSetController.Bind();
        }

        public void DisableInput()
        {
            movementActionSetController.Unbind();
        }
    }
}

using CardboardCore.EntityComponents;
using CardboardCore.Utilities;
using DungeonCrawler.UserInput;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DungeonCrawler.EntityComponents.Components
{
    public class MovementInputComponent : CardboardCore.EntityComponents.Component
    {
        [TweakableField] private string prefabKey;

        // Owner's components
        private Entity gameplayCameraEntity;
        private HoppingPositionComponent hoppingPositionComponent;
        private GridPositionComponent gridPositionComponent;
        private RoomAwarenessComponent roomAwarenessComponent;

        // Other's components
        private RotationComponent cameraRotationComponent;

        // Other objects
        private MovementInputManager movementInputManager;

        private bool bindInputOnceLoaded;

        private MovementInputState movementInputState;

        public enum MovementInputState
        {
            Unlocked,
            Locked
        }

        protected override void OnStart()
        {
            hoppingPositionComponent = GetComponent<HoppingPositionComponent>();
            gridPositionComponent = GetComponent<GridPositionComponent>();
            roomAwarenessComponent = GetComponent<RoomAwarenessComponent>();

            Log.Write(hoppingPositionComponent);

            hoppingPositionComponent.MovementAnimationFinishedEvent += OnMovementAnimationFinished;

            // TODO: Don't do this for every MovementInputComponent!
            // Unfortunately, movement input manager has to be a unity object, otherwise input won't be properly registered
            AsyncOperationHandle<UnityEngine.GameObject> handle = Addressables.LoadAssetAsync<UnityEngine.GameObject>(prefabKey);
            handle.Completed += OnMovementInputLoaded;
        }

        protected override void OnStop()
        {
            base.OnStop();

            Unbind();

            hoppingPositionComponent.MovementAnimationFinishedEvent -= OnMovementAnimationFinished;
            movementInputManager.InputEvent -= OnMovementInput;

            UnityEngine.MonoBehaviour.Destroy(movementInputManager.gameObject);
        }

        private void OnMovementAnimationFinished()
        {
            movementInputManager?.Bind();
        }

        private void OnMovementInputLoaded(AsyncOperationHandle<GameObject> handle)
        {
            handle.Completed -= OnMovementInputLoaded;
            Addressables.Release(handle);

            UnityEngine.GameObject gameObject = UnityEngine.MonoBehaviour.Instantiate(handle.Result);
            movementInputManager = gameObject.GetComponent<MovementInputManager>();

            movementInputManager.InputEvent += OnMovementInput;

            if (bindInputOnceLoaded)
            {
                movementInputManager.Bind();
                bindInputOnceLoaded = false;
            }
        }

        private void OnMovementInput(object sender, MovementInputEventArgs e)
        {
            // We're in need of an extra layer of control, which bypasses the Bind state of the input manager
            if (movementInputState == MovementInputState.Locked)
            {
                return;
            }

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

            movementInputManager.Unbind();
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

        public void Bind()
        {
            if (movementInputManager == null)
            {
                bindInputOnceLoaded = true;
                return;
            }

            movementInputManager.Bind();
        }

        public void Unbind()
        {
            movementInputManager.Unbind();
        }

        public void SetGameplayCameraEntity(Entity gameplayCameraEntity)
        {
            this.gameplayCameraEntity = gameplayCameraEntity;
            cameraRotationComponent = gameplayCameraEntity.GetComponent<RotationComponent>();
        }

        public void Lock()
        {
            movementInputState = MovementInputState.Locked;
        }

        public void Unlock()
        {
            movementInputState = MovementInputState.Unlocked;
        }
    }
}
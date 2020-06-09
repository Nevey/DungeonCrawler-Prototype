using CardboardCore.DI;
using CardboardCore.EntityComponents;
using DungeonCrawler.UserInput;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DungeonCrawler.EntityComponents.Components
{
    public class MovementInputComponent : CardboardCore.EntityComponents.Component
    {
        [TweakableField] private string prefabKey;

        private Entity gameplayCameraEntity;
        private RotationComponent cameraRotationComponent;
        private GridPositionComponent positionComponent;
        private RoomAwarenessComponent roomAwarenessComponent;
        private MovementInputManager movementInputManager;
        private bool bindInputOnceLoaded;

        public MovementInputComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            Injector.Inject(this);

            positionComponent = GetComponent<GridPositionComponent>();
            roomAwarenessComponent = GetComponent<RoomAwarenessComponent>();

            // TODO: Don't do this for every MovementInputComponent!
            // Unfortunately, movement input manager has to be a unity object, otherwise input won't be properly registered
            AsyncOperationHandle<UnityEngine.GameObject> handle = Addressables.LoadAssetAsync<UnityEngine.GameObject>(prefabKey);
            handle.Completed += OnMovementInputLoaded;
        }

        protected override void OnStop()
        {
            base.OnStop();

            Unbind();

            movementInputManager.InputEvent -= OnMovementInput;

            UnityEngine.MonoBehaviour.Destroy(movementInputManager.gameObject);

            Injector.Dump(this);
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
            e = ModifyInputBasedOnCameraAngle(e);

            if (!roomAwarenessComponent.CanWalk(e))
            {
                return;
            }

            switch (e.inputDirection)
            {
                case InputDirection.Horizontal:
                    positionComponent.UpdateX(e.strength);
                    break;

                case InputDirection.Vertical:
                    positionComponent.UpdateY(e.strength);
                    break;
            }
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
    }
}
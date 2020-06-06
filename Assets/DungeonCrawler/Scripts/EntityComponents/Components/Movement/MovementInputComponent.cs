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

        private PositionComponent positionComponent;
        private MovementInputManager movementInputManager;
        private bool bindInputOnceLoaded;

        public MovementInputComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            positionComponent = GetComponent<PositionComponent>();

            AsyncOperationHandle<UnityEngine.GameObject> handle = Addressables.LoadAssetAsync<UnityEngine.GameObject>(prefabKey);
            handle.Completed += OnLoadPrefabCompleted;
        }

        protected override void OnStop()
        {
            base.OnStop();

            Unbind();

            movementInputManager.InputEvent -= OnMovementInput;

            UnityEngine.MonoBehaviour.Destroy(movementInputManager.gameObject);
        }

        private void OnLoadPrefabCompleted(AsyncOperationHandle<GameObject> handle)
        {
            handle.Completed -= OnLoadPrefabCompleted;
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
    }
}
using CardboardCore.EC;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using UnityEngine;

namespace DungeonCrawler.EC.Components
{
    public class ViewComponent : CardboardCore.EC.Component
    {
        [TweakableField] private string key;

        protected PositionComponent positionComponent;
        protected RotationComponent rotationComponent;
        public GameObject gameObject { get; protected set; }

        public event Action<ViewComponent> LoadFinishedEvent;

        protected override void OnStart()
        {
            positionComponent = GetComponent<PositionComponent>();

            if (positionComponent != null)
            {
                positionComponent.PositionUpdatedEvent += OnPositionUpdated;
            }

            rotationComponent = GetComponent<RotationComponent>();

            if (rotationComponent != null)
            {
                rotationComponent.RotationUpdatedEvent += OnRotationUpdated;
            }
        }

        protected override void OnStop()
        {
            MonoBehaviour.Destroy(gameObject);

            if (positionComponent != null)
            {
                positionComponent.PositionUpdatedEvent -= OnPositionUpdated;
            }

            if (rotationComponent != null)
            {
                rotationComponent.RotationUpdatedEvent -= OnRotationUpdated;
            }
        }

        /// <summary>
        /// Handles visual changes on position data changed. Override to remove default behaviour.
        /// </summary>
        /// <param name="position">Vector3 World Position</param>
        protected virtual void OnPositionUpdated(Vector3 position)
        {
            if (gameObject == null)
            {
                return;
            }

            gameObject.transform.position = position;
        }

        /// <summary>
        /// Handles visual changes on rotation data changed. Override to remove default behaviour.
        /// </summary>
        /// <param name="rotation"></param>
        protected virtual void OnRotationUpdated(Quaternion rotation)
        {
            if (gameObject == null)
            {
                return;
            }

            gameObject.transform.rotation = rotation;
        }

        private void OnLoadPrefabCompleted(AsyncOperationHandle<GameObject> handle)
        {
            // Addressables.Release(handle);
            handle.Completed -= OnLoadPrefabCompleted;

            GameObject gameObject = UnityEngine.MonoBehaviour.Instantiate(handle.Result);
            gameObject.transform.position = positionComponent == null ? Vector3.zero : positionComponent.position;
            gameObject.transform.rotation = rotationComponent == null ? Quaternion.identity : rotationComponent.rotation;

            this.gameObject = gameObject;

            LoadFinishedEvent?.Invoke(this);
        }

        public virtual void Load()
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);
            handle.Completed += OnLoadPrefabCompleted;
        }

        public void LookAt(PositionComponent positionComponent)
        {
            LookAt(positionComponent.position);
        }

        public void LookAt(UnityEngine.Vector3 target)
        {
            gameObject.transform.LookAt(target);

            // TODO: Create own LookAt code, so we don't have to do weird stuff like this
            rotationComponent.SetRotation(gameObject.transform.rotation, true);
        }
    }
}

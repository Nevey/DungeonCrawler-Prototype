using CardboardCore.EntityComponents;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using GameObject = UnityEngine.GameObject;
using Vector3 = UnityEngine.Vector3;
using MonoBehaviour = UnityEngine.MonoBehaviour;
using System;
using UnityEngine;

namespace DungeonCrawler.EntityComponents.Components
{
    public class ViewComponent : CardboardCore.EntityComponents.Component
    {
        [TweakableField] private string key;

        protected PositionComponent positionComponent;
        protected RotationComponent rotationComponent;
        protected GameObject gameObject;

        protected event Action LoadFinishedEvent;

        public ViewComponent(Entity owner) : base(owner)
        {
        }

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
            Addressables.Release(handle);
            handle.Completed -= OnLoadPrefabCompleted;

            GameObject gameObject = UnityEngine.MonoBehaviour.Instantiate(handle.Result);
            gameObject.transform.position = positionComponent == null ? Vector3.zero : positionComponent.position;
            gameObject.transform.rotation = rotationComponent == null ? Quaternion.identity : rotationComponent.rotation;

            this.gameObject = gameObject;

            LoadFinishedEvent?.Invoke();
        }

        public virtual void Load()
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);
            handle.Completed += OnLoadPrefabCompleted;
        }
    }
}
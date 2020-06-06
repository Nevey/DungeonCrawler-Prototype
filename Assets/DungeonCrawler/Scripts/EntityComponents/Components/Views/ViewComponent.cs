using CardboardCore.EntityComponents;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using GameObject = UnityEngine.GameObject;
using Vector3 = UnityEngine.Vector3;
using MonoBehaviour = UnityEngine.MonoBehaviour;
using System;

namespace DungeonCrawler.EntityComponents.Components
{
    public class ViewComponent : CardboardCore.EntityComponents.Component
    {
        [TweakableField] private string key;

        protected PositionComponent positionComponent;
        protected GameObject gameObject;

        protected event Action LoadFinishedEvent;

        public ViewComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            positionComponent = GetComponent<PositionComponent>();
            positionComponent.PositionUpdatedEvent += OnPositionUpdated;
        }

        protected override void OnStop()
        {
            MonoBehaviour.Destroy(gameObject);
            positionComponent.PositionUpdatedEvent -= OnPositionUpdated;
        }

        /// <summary>
        /// Handles visual changes on position data changed. Override to remove default behaviour.
        /// </summary>
        /// <param name="x">World Position X</param>
        /// <param name="y">World Position Y</param>
        protected virtual void OnPositionUpdated(int x, int y)
        {
            if (gameObject == null)
            {
                return;
            }

            gameObject.transform.position = new Vector3(positionComponent.x, 0f, positionComponent.y);
        }

        private void OnLoadPrefabCompleted(AsyncOperationHandle<GameObject> handle)
        {
            Addressables.Release(handle);
            handle.Completed -= OnLoadPrefabCompleted;

            GameObject gameObject = UnityEngine.MonoBehaviour.Instantiate(handle.Result);
            gameObject.transform.position = new Vector3(positionComponent.x, 0f, positionComponent.y);

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
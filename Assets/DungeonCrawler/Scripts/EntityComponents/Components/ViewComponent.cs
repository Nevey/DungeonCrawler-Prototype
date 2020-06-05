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
        }

        protected override void OnStop()
        {
            MonoBehaviour.Destroy(gameObject);
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
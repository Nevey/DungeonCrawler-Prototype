using CardboardCore.EntityComponents;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using GameObject = UnityEngine.GameObject;
using Vector3 = UnityEngine.Vector3;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace DungeonCrawler.EntityComponents.Components
{
    public class ViewComponent : CardboardCore.EntityComponents.Component
    {
        [TweakableField] private string key;

        private PositionComponent positionComponent;
        private GameObject gameObject;

        public ViewComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            positionComponent = GetComponent<PositionComponent>();

            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);
            handle.Completed += OnLoadPrefabCompleted;
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

        }
    }
}
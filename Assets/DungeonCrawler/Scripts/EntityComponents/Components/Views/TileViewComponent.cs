using CardboardCore.EntityComponents;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DungeonCrawler.EntityComponents.Components
{
    public class TileViewComponent : ViewComponent
    {
        [TweakableField] private string doorwayMaterialKey;

        private TileDataComponent tileDataComponent;

        public TileViewComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            base.OnStart();

            LoadFinishedEvent += OnLoadFinished;

            tileDataComponent = GetComponent<TileDataComponent>(true);
        }

        protected override void OnStop()
        {
            base.OnStop();

            LoadFinishedEvent -= OnLoadFinished;
        }

        private void OnLoadFinished()
        {
            // SetMaterial();
        }

        private void SetMaterial()
        {
            if (tileDataComponent.Data.tileState != Levels.TileState.Doorway)
            {
                return;
            }

            AsyncOperationHandle<Material> handle = Addressables.LoadAssetAsync<Material>(doorwayMaterialKey);
            handle.Completed += OnLoadPrefabCompleted;
        }

        private void OnLoadPrefabCompleted(AsyncOperationHandle<Material> handle)
        {
            Addressables.Release(handle);
            handle.Completed -= OnLoadPrefabCompleted;

            gameObject.GetComponent<Renderer>().material = handle.Result;
        }

        public override void Load()
        {
            if (tileDataComponent.Data.tileState == Levels.TileState.Unused)
            {
                return;
            }

            base.Load();
        }
    }
}
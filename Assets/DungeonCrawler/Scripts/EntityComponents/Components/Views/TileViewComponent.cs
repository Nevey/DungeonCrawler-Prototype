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

        protected override void OnStart()
        {
            base.OnStart();

            tileDataComponent = GetComponent<TileDataComponent>(true);
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
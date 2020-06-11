using CardboardCore.EntityComponents;
using DG.Tweening;

namespace DungeonCrawler.EntityComponents.Components
{
    public class TileViewComponent : ViewComponent
    {
        [TweakableField] private float spawnAnimationDelay;

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

        private void OnLoadFinishedEvent(ViewComponent viewComponent)
        {
            PlaySpawnAnimation();

            LoadFinishedEvent -= OnLoadFinishedEvent;
        }

        private void PlaySpawnAnimation()
        {
            gameObject.transform.localScale = UnityEngine.Vector3.zero;

            Tween tween = gameObject.transform.DOScale(1f, 1f);
            tween.SetDelay(spawnAnimationDelay);
            tween.Play();
        }

        public void SetupSpawnAnimationOnViewLoaded(int index)
        {
            if (tileDataComponent.Data.tileState == Levels.TileState.Unused)
            {
                return;
            }

            spawnAnimationDelay = spawnAnimationDelay * index;
            LoadFinishedEvent += OnLoadFinishedEvent;
        }
    }
}

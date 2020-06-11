using CardboardCore.EntityComponents;
using DG.Tweening;

namespace DungeonCrawler.EntityComponents.Components
{
    public class TileViewComponent : ViewComponent
    {
        [TweakableField] private float spawnAnimationDelay;
        [TweakableField] private float spawnAnimationDuration;

        private TileDataComponent tileDataComponent;
        private Tween scaleTween;

        protected override void OnStart()
        {
            base.OnStart();

            tileDataComponent = GetComponent<TileDataComponent>(true);
        }

        public override void Load()
        {
            if (tileDataComponent.tileData.tileState == Levels.TileState.Unused)
            {
                return;
            }

            base.Load();
        }

        private void ScaleDownOnLoadFinished(ViewComponent viewComponent)
        {
            gameObject.transform.localScale = UnityEngine.Vector3.zero;
            LoadFinishedEvent -= ScaleDownOnLoadFinished;
        }

        private void PlayAnimationOnLoadFinished(ViewComponent viewComponent)
        {
            PlaySpawnAnimation();

            LoadFinishedEvent -= PlayAnimationOnLoadFinished;
        }

        public void PlaySpawnAnimation(TweenCallback callback = null)
        {
            if (tileDataComponent.tileData.tileState == Levels.TileState.Unused)
            {
                callback?.Invoke();
                return;
            }

            gameObject.transform.localScale = UnityEngine.Vector3.zero;

            scaleTween = gameObject.transform.DOScale(1f, spawnAnimationDuration);
            scaleTween.SetDelay(spawnAnimationDelay);
            scaleTween.OnComplete(callback);
            scaleTween.Play();
        }

        public void SetupSpawnAnimation(float index)
        {
            if (tileDataComponent.tileData.tileState == Levels.TileState.Unused)
            {
                return;
            }

            spawnAnimationDelay = spawnAnimationDelay * index;
            LoadFinishedEvent += ScaleDownOnLoadFinished;
        }

        public void SetupSpawnAnimationOnViewLoaded(float index)
        {
            if (tileDataComponent.tileData.tileState == Levels.TileState.Unused)
            {
                return;
            }

            spawnAnimationDelay = spawnAnimationDelay * index;
            LoadFinishedEvent += PlayAnimationOnLoadFinished;
        }
    }
}

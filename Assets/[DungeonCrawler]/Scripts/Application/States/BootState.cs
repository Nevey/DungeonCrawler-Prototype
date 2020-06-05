using CardboardCore.DI;
using CardboardCore.StateMachines;
using DungeonCrawler.Scenes;

namespace DungeonCrawler.Application.States
{
    public class BootState : State
    {
        [Inject] private SceneLoader sceneLoader;

        protected override void OnEnter()
        {
            sceneLoader.SceneLoadFinishedEvent += OnSceneLoadFinished;
            sceneLoader.LoadSceneAsyncSingle("GameplayScene");
        }

        protected override void OnExit()
        {
            sceneLoader.SceneLoadFinishedEvent -= OnSceneLoadFinished;
        }

        private void OnSceneLoadFinished()
        {
            owner.ToNextState();
        }
    }
}

using System;
using CardboardCore.DI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonCrawler.Scenes
{
    [Injectable(Singleton = true)]
    public class SceneLoader
    {
        public event Action SceneLoadFinishedEvent;

        private void OnSceneLoadFinished(AsyncOperation asyncOp)
        {
            asyncOp.completed -= OnSceneLoadFinished;

            SceneLoadFinishedEvent?.Invoke();
        }

        public void LoadSceneAsyncSingle(string sceneName)
        {
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            asyncOp.completed += OnSceneLoadFinished;
        }

        public void LoadSceneAsyncAdditive(string sceneName)
        {
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            asyncOp.completed += OnSceneLoadFinished;
        }

    }
}

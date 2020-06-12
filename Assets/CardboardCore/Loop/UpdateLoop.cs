using System.Collections.Generic;
using System.Timers;
using CardboardCore.DI;
using UnityEngine;

namespace CardboardCore.Loop
{
    [Injectable(Singleton = true)]
    public class UpdateLoop : MonoBehaviour
    {
        private const int TargetFPS = 60;

        private Timer timer;
        private long previousTime;
        private readonly List<IGameLoopable> gameLoopables = new List<IGameLoopable>();

        private void Awake()
        {
            Application.targetFrameRate = TargetFPS;
        }

        private void Update()
        {
            for (int i = 0; i < gameLoopables.Count; i++)
            {
                gameLoopables[i].Update(Time.deltaTime);
            }
        }

        public void RegisterGameLoopable(IGameLoopable gameLoopable)
        {
            if (gameLoopables.Contains(gameLoopable))
            {
                return;
            }

            gameLoopable.Start();
            gameLoopables.Add(gameLoopable);
        }

        public void UnregisterGameLoopable(IGameLoopable gameLoopable)
        {
            if (!gameLoopables.Contains(gameLoopable))
            {
                return;
            }

            gameLoopables.Remove(gameLoopable);
            gameLoopable.Stop();
        }
    }
}

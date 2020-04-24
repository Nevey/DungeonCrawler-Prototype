using System;
using System.Collections.Generic;
using System.Timers;
using CardboardCore.DI;
using CardboardCore.Utilities;

namespace CardboardCore.Loop
{
    [Injectable(Singleton = true)]
    public class UpdateLoop
    {
        private const int TargetFPS = 60;

        private Timer timer;
        private long previousTime;
        private readonly List<IGameLoopable> gameLoopables = new List<IGameLoopable>();

        public UpdateLoop()
        {
            StartLoop();
        }

        private void StartLoop()
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += OnLoop;
            timer.Interval = 1000 / TargetFPS;
            timer.Enabled = true;

            previousTime = DateTime.Now.Ticks;
        }

        public void StopLoop()
        {
            timer.Elapsed -= OnLoop;
            timer.Enabled = false;

            Log.Warn("UpdateLoop was stopped!");
        }

        private void OnLoop(object sender, ElapsedEventArgs e)
        {
            long deltaTime = e.SignalTime.Ticks - previousTime;
            deltaTime /= 1000;

            for (int i = 0; i < gameLoopables.Count; i++)
            {
                gameLoopables[i].Update(deltaTime);
            }

            previousTime = e.SignalTime.Ticks;
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

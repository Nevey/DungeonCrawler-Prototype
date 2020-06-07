using CardboardCore.EntityComponents;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents;

namespace DungeonCrawler.Gameplay.States
{
    public class SpawnCameraState : State
    {
        protected override void OnEnter()
        {
            Entity cameraEntity = new GameplayEntityFactory().Instantiate("GameplayCameraEntity");
        }

        protected override void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }
}
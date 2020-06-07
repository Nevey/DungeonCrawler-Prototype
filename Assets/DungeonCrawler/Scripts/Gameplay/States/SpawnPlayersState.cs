using CardboardCore.DI;
using CardboardCore.EntityComponents;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.Gameplay.States
{
    public class SpawnPlayersState : State
    {
        [Inject] private EntityRegister entityRegister;

        private Entity gameplayCameraEntity;
        private PlayerViewComponent playerViewComponent;

        protected override void OnEnter()
        {
            Entity initialRoomEntity = entityRegister.FindEntity("InitialRoomEntity");
            gameplayCameraEntity = entityRegister.FindEntity("GameplayCameraEntity");

            // TODO: Listen to spawns from other players, and let other players know we've spawned our player
            // OR
            // TODO: Let server decide spawn locations and send to players, wait for all players to have their player spawned before moving on

            Entity playerEntity = new GameplayEntityFactory().Instantiate("PlayerEntity");
            playerEntity.GetComponent<GridPositionComponent>().SetPosition(2, 2); // TODO: Set player on a free position based on initial room's grid

            playerViewComponent = playerEntity.GetComponent<PlayerViewComponent>();
            playerViewComponent.LoadFinishedEvent += OnPlayerViewLoadFinished;
            playerViewComponent.Load();
        }

        protected override void OnExit()
        {
        }

        private void OnPlayerViewLoadFinished()
        {
            playerViewComponent.LoadFinishedEvent -= OnPlayerViewLoadFinished;

            gameplayCameraEntity.GetComponent<CameraTargetComponent>().SetTarget(playerViewComponent);
            owner.ToNextState();
        }
    }
}
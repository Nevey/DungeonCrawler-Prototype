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

        private PlayerViewComponent playerViewComponent;

        protected override void OnEnter()
        {
            RoomBuilderComponent roomBuilderComponent = entityRegister.FindEntity("LevelEntity").GetComponent<RoomBuilderComponent>();
            RoomDataComponent roomDataComponent = roomBuilderComponent.rooms[0];

            // TODO: Listen to spawns from other players, and let other players know we've spawned our player
            // OR
            // TODO: Let server decide spawn locations and send to players, wait for all players to have their player spawned before moving on

            TileDataComponent randomFreeTile = roomDataComponent.GetFreeTile();

            Entity playerEntity = new GameplayEntityFactory().Instantiate("PlayerEntity");
            playerEntity.GetComponent<GridPositionComponent>().SetPosition(randomFreeTile.Data.x, randomFreeTile.Data.y);

            playerEntity.GetComponent<RoomAwarenessComponent>().EnterRoom(roomDataComponent.roomData);

            playerViewComponent = playerEntity.GetComponent<PlayerViewComponent>();
            playerViewComponent.LoadFinishedEvent += OnPlayerViewLoadFinished;
            playerViewComponent.Load();
        }

        protected override void OnExit()
        {

        }

        private void OnPlayerViewLoadFinished(ViewComponent viewComponent)
        {
            playerViewComponent.LoadFinishedEvent -= OnPlayerViewLoadFinished;

            owner.ToNextState();
        }
    }
}
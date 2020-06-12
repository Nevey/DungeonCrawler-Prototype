using CardboardCore.DI;
using CardboardCore.EntityComponents;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.RoomBuilding.States
{
    public class SpawnPlayersState : State
    {
        [Inject] private EntityRegister entityRegister;

        private PlayerViewComponent playerViewComponent;

        protected override void OnEnter()
        {
            Entity levelEntity = entityRegister.FindEntity("LevelEntity");
            RoomRegistryComponent roomRegistryComponent = levelEntity.GetComponent<RoomRegistryComponent>();
            RoomDataComponent roomDataComponent = roomRegistryComponent.rooms[0];

            // TODO: Listen to spawns from other players, and let other players know we've spawned our player
            // OR
            // TODO: Let server decide spawn locations and send to players, wait for all players to have their player spawned before moving on

            TileDataComponent randomFreeTile = roomDataComponent.GetFreeTile();

            Entity playerEntity = new GameplayEntityFactory().Instantiate("PlayerEntity");
            playerEntity.GetComponent<GridPositionComponent>().SetPosition(randomFreeTile.tileData.x, randomFreeTile.tileData.y);

            playerEntity.GetComponent<RoomAwarenessComponent>().EnterRoom(roomDataComponent);

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

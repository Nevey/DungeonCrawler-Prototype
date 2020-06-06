using CardboardCore.DI;
using CardboardCore.EntityComponents;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents.Components;

namespace DungeonCrawler.Gameplay.States
{
    public class SpawnPlayersState : State
    {
        [Inject] private EntityRegister entityRegister;

        private Entity initialRoomEntity;

        protected override void OnEnter()
        {
            initialRoomEntity = entityRegister.FindEntity("InitialRoomEntity");

            // TODO: Listen to spawns from other players, and let other players know we've spawned our player
            // OR
            // TODO: Let server decide spawn locations and send to players, wait for all players to have their player spawned before moving on

            initialRoomEntity.GetComponent<SpawnPlayerComponent>().Spawn();

            owner.ToNextState();
        }

        protected override void OnExit()
        {

        }
    }
}
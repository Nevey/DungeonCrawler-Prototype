using CardboardCore.DI;
using CardboardCore.EntityComponents;
using CardboardCore.StateMachines;
using DungeonCrawler.EntityComponents;
using DungeonCrawler.EntityComponents.Components;
using DungeonCrawler.Levels;

namespace DungeonCrawler.Gameplay.States
{
    public class PlaceCardsState : State
    {
        [Inject] private EntityRegister entityRegister;

        private GameplayEntityFactory gameplayEntityFactory;

        protected override void OnEnter()
        {
            gameplayEntityFactory = new GameplayEntityFactory();

            // Get last created room from level entity
            // Create and place room cards on doorway tiles

            LevelBuilderComponent levelBuilderComponent =
                entityRegister.FindEntity("LevelEntity").GetComponent<LevelBuilderComponent>();

            RoomDataComponent roomDataComponent =
                levelBuilderComponent.RoomEntities[levelBuilderComponent.RoomEntities.Count - 1]
                    .GetComponent<RoomDataComponent>();

            TileDataComponent[] doorwayTiles = roomDataComponent.GetTiles(TileState.Doorway);

            for (int i = 0; i < doorwayTiles.Length; i++)
            {
                CreateCardEntity(doorwayTiles[i]);
            }
        }

        protected override void OnExit()
        {
            gameplayEntityFactory = null;
        }

        private void CreateCardEntity(TileDataComponent tileDataComponent)
        {
            Entity cardEntity = gameplayEntityFactory.Instantiate("CardEntity");

            PositionComponent tilePositionComponent = tileDataComponent.GetComponent<PositionComponent>();
            PositionComponent cardPositionComponent = cardEntity.GetComponent<PositionComponent>();

            cardPositionComponent.SetPosition(tilePositionComponent.x, tilePositionComponent.y);

            CardViewComponent cardViewComponent = cardEntity.GetComponent<CardViewComponent>();
            cardViewComponent.Load();
        }
    }
}
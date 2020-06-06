using System.Collections.Generic;
using System.Linq;
using CardboardCore.DI;
using CardboardCore.EntityComponents;
using CardboardCore.StateMachines;
using DungeonCrawler.Cards;
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
                CreateRoomCards(doorwayTiles[i]);
            }
        }

        protected override void OnExit()
        {
            gameplayEntityFactory = null;
        }

        private void CreateRoomCards(TileDataComponent tileDataComponent)
        {
            Entity cardEntity = gameplayEntityFactory.Instantiate("RoomCardEntity");

            // TODO: Get random card from once card decks are built
            CardDataCollection<RoomCardData> collection = new CardDataLoader<RoomCardData, RoomCardDataConfig>().Load();
            List<RoomCardData> cards = collection.cards.ToList();
            cards.RemoveAt(0); // Remove initial room, temp solution...

            int index = UnityEngine.Random.Range(0, cards.Count);
            RoomCardData cardData = cards[index];

            cardEntity.GetComponent<RoomCardDataComponent>().SetData(cardData);

            PositionComponent tilePositionComponent = tileDataComponent.GetComponent<PositionComponent>();
            PositionComponent cardPositionComponent = cardEntity.GetComponent<PositionComponent>();

            cardPositionComponent.SetPosition(tilePositionComponent.x, tilePositionComponent.y);

            CardViewComponent cardViewComponent = cardEntity.GetComponent<CardViewComponent>();
            cardViewComponent.Load();
        }
    }
}
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

            // Get room builder component from level entity
            RoomBuilderComponent roomBuilderComponent = entityRegister.FindEntity("LevelEntity").GetComponent<RoomBuilderComponent>();

            // Get last indexed room data, this is the room that was most recently created by the room builder
            RoomDataComponent roomDataComponent = roomBuilderComponent.rooms[roomBuilderComponent.rooms.Count - 1];

            // Find all doorway tiles
            TileDataComponent[] doorwayTiles = roomDataComponent.GetTiles(TileState.Doorway);

            // Create all room cards based on doorway tiles
            for (int i = 0; i < doorwayTiles.Length; i++)
            {
                CreateRoomCards(roomDataComponent, doorwayTiles[i]);
            }

            // TODO: Create tile cards too...

            owner.ToNextState();
        }

        protected override void OnExit()
        {
            gameplayEntityFactory = null;
        }

        private void CreateRoomCards(RoomDataComponent roomDataComponent, TileDataComponent tileDataComponent)
        {
            // Create new card entity
            Entity cardEntity = gameplayEntityFactory.Instantiate("RoomCardEntity");

            // TODO: Get random card from once card decks are built
            // Get a random room card
            CardDataCollection<RoomCardData> collection = new CardDataLoader<RoomCardData, RoomCardDataConfig>().Load();
            List<RoomCardData> cards = collection.cards.ToList();

            // Get random index
            int index = UnityEngine.Random.Range(0, cards.Count);
            RoomCardData cardData = cards[index];

            // Set the room card data component's data
            RoomCardDataComponent roomCardDataComponent = cardEntity.GetComponent<RoomCardDataComponent>();
            roomCardDataComponent.SetData(cardData);

            // Set card's grid position values to the tile's position values it is placed on
            GridPositionComponent tileGridPositionComponent = tileDataComponent.GetComponent<GridPositionComponent>();
            GridPositionComponent cardGridPositionComponent = cardEntity.GetComponent<GridPositionComponent>();
            cardGridPositionComponent.SetPosition(tileGridPositionComponent.x, tileGridPositionComponent.y);

            // Store the room card data component in it's room's data
            roomDataComponent.AddRoomCard(roomCardDataComponent);

            // Load card visuals
            CardViewComponent cardViewComponent = cardEntity.GetComponent<CardViewComponent>();
            cardViewComponent.Load();
        }
    }
}
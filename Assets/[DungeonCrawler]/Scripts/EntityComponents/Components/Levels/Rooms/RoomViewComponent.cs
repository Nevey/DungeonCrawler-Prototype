using System.Collections.Generic;
using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    public class RoomViewComponent : Component
    {
        private GameplayEntityFactory gameplayEntityFactory;
        private RoomLayoutComponent roomLayoutComponent;

        private List<Entity> tileEntities;

        public RoomViewComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            gameplayEntityFactory = new GameplayEntityFactory();

            roomLayoutComponent = GetComponent<RoomLayoutComponent>();
            roomLayoutComponent.OnDataUpdatedEvent += OnRoomDataUpdated;

            tileEntities = new List<Entity>();
        }

        protected override void OnStop()
        {
            roomLayoutComponent.OnDataUpdatedEvent -= OnRoomDataUpdated;
        }

        private void OnRoomDataUpdated(RoomData roomData)
        {
            CreateTiles(roomData);
        }

        private void CreateTiles(RoomData roomData)
        {
            for (int x = 0; x < roomData.gridSizeX; x++)
            {
                for (int y = 0; y < roomData.gridSizeY; y++)
                {
                    tileEntities.Add(CreateTile(x, y));
                }
            }
        }

        private Entity CreateTile(int x, int y)
        {
            Entity tileEntity = gameplayEntityFactory.Instantiate("TileEntity");
            tileEntity.GetComponent<PositionComponent>().SetPosition(x, y);

            return tileEntity;
        }
    }
}
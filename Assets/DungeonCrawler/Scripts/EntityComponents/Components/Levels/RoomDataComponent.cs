using System.Collections.Generic;
using CardboardCore.EntityComponents;
using DungeonCrawler.Levels;

namespace DungeonCrawler.EntityComponents.Components
{
    /// <summary>
    /// Holds room data and grid information
    /// </summary>
    public class RoomDataComponent : Component
    {
        private GameplayEntityFactory gameplayEntityFactory;
        private RoomData roomData;
        private List<Entity> tileEntities;
        private List<TileDataComponent> tileDataComponents;

        public List<TileDataComponent> TileEntities => tileDataComponents;

        public RoomDataComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            gameplayEntityFactory = new GameplayEntityFactory();

            tileEntities = new List<Entity>();
            tileDataComponents = new List<TileDataComponent>();
        }

        private void CreateTiles(RoomData roomData)
        {
            for (int x = 0; x < roomData.gridSizeX; x++)
            {
                for (int y = 0; y < roomData.gridSizeY; y++)
                {
                    tileEntities.Add(CreateTile(roomData.tiles[x, y]));
                }
            }
        }

        private Entity CreateTile(TileData tileData)
        {
            Entity tileEntity = gameplayEntityFactory.Instantiate("TileEntity");

            TileDataComponent tileDataComponent = tileEntity.GetComponent<TileDataComponent>();
            tileDataComponent.SetData(tileData);
            tileDataComponents.Add(tileDataComponent);

            tileEntity.GetComponent<GridPositionComponent>().SetPosition(tileData.x, tileData.y);
            tileEntity.GetComponent<PositionComponent>().SetPosition(tileData.x, 0f, tileData.y);
            tileEntity.GetComponent<RotationComponent>().SetRotation(90f, 0f, 0f);
            tileEntity.GetComponent<TileViewComponent>().Load();

            return tileEntity;
        }

        public void Load(int id)
        {
            roomData = new RoomDataLoader().Load(id);
            CreateTiles(roomData);
        }

        public TileDataComponent[] GetTiles(TileState tileState)
        {
            List<TileDataComponent> foundTiles = new List<TileDataComponent>();

            for (int i = 0; i < tileDataComponents.Count; i++)
            {
                if (tileDataComponents[i].Data.tileState == tileState)
                {
                    foundTiles.Add(tileDataComponents[i]);
                }
            }

            return foundTiles.ToArray();
        }
    }
}
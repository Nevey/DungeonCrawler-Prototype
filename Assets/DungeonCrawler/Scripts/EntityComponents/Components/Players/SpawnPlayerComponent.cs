using System.Collections.Generic;
using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents.Components
{
    public class SpawnPlayerComponent : Component
    {
        private GameplayEntityFactory gameplayEntityFactory;

        private List<Entity> playerEntities;

        public SpawnPlayerComponent(Entity owner) : base(owner)
        {
        }

        protected override void OnStart()
        {
            gameplayEntityFactory = new GameplayEntityFactory();
            playerEntities = new List<Entity>();
        }

        public void Spawn(int x, int y)
        {
            Entity entity = gameplayEntityFactory.Instantiate("PlayerEntity");
            entity.GetComponent<PositionComponent>().SetPosition(x, y);

            playerEntities.Add(entity);
        }
    }
}
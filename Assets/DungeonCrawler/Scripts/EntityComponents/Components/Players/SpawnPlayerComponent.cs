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

        public void Spawn()
        {
            // TODO: Find eligible spot in current room to stand
            Entity entity = gameplayEntityFactory.Instantiate("PlayerEntity");
            entity.GetComponent<PositionComponent>().SetPosition(2, 2);
            entity.GetComponent<PlayerViewComponent>().Load();

            playerEntities.Add(entity);
        }
    }
}
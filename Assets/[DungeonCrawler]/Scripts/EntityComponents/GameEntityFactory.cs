using CardboardCore.EntityComponents;
using CardboardCore.DI;

namespace DungeonCrawler.EntityComponents
{
    [Injectable]
    public class GameEntityFactory : EntityFactory<GameEntityConfiguration>
    {
        public GameEntityFactory()
        {
            Initialize(new GameEntityConfiguration());
        }
    }
}

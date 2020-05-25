using CardboardCore.EntityComponents;
using CardboardCore.DI;

namespace DungeonCrawler.EntityComponents
{
    [Injectable]
    public class GameEntityFactory : EntityFactory<GameEntityLoadData>
    {
        public GameEntityFactory()
        {
            Initialize(new GameEntityLoadData());
        }
    }
}

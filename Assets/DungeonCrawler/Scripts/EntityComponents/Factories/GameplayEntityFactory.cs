using CardboardCore.EntityComponents;

namespace DungeonCrawler.EntityComponents
{
    public class GameplayEntityFactory : EntityFactory<GameplayEntityLoadData>
    {
        public GameplayEntityFactory()
        {
            Initialize(new GameplayEntityLoadData());
        }
    }
}

using CardboardCore.EC;

namespace DungeonCrawler.EC
{
    public class GameplayEntityFactory : EntityFactory<GameplayEntityLoadData>
    {
        public GameplayEntityFactory()
        {
            Initialize(new GameplayEntityLoadData());
        }
    }
}

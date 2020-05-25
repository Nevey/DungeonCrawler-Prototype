using CardboardCore.DI;

namespace DungeonCrawler.Cards
{
    [Injectable]
    public class RoomCardDataLoader : CardDataLoader<RoomCardData>
    {
        protected override string Path => "/[DungeonCrawler]/Configs/RoomCards.json";
    }
}
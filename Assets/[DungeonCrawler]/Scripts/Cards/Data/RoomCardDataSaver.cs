using CardboardCore.DI;

namespace DungeonCrawler.Cards
{
    [Injectable]
    public class RoomCardDataSaver : CardDataSaver<RoomCardData>
    {
        protected override string Path => "/[DungeonCrawler]/Configs/RoomCards.json";
    }
}
using DungeonCrawler.Levels;

namespace DungeonCrawler.Cards
{
    public class RoomCard : Card
    {
        private RoomData roomData;

        public RoomCard(int id, string name, RoomData roomData)
            : base(id, name)
        {
            this.roomData = roomData;
        }
    }
}
using DungeonCrawler.Levels;

namespace DungeonCrawler.Cards
{
    public class RoomCard : Card
    {
        private Room room;

        public RoomCard(int id, string name, Room room)
            : base(id, name)
        {
            this.room = room;
        }
    }
}
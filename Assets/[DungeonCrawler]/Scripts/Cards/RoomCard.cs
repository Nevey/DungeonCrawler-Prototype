using DungeonCrawler.Levels;

namespace DungeonCrawler.Cards
{
    public class RoomCard : Card
    {
        private RoomData room;

        public RoomCard(int id, string name, RoomData room)
            : base(id, name)
        {
            this.room = room;
        }
    }
}
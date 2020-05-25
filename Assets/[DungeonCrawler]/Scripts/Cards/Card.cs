namespace DungeonCrawler.Cards
{
    public abstract class Card
    {
        private int id;
        private string name;

        public Card(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
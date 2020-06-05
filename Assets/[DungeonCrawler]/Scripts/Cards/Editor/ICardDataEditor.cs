namespace DungeonCrawler.Cards
{
    public interface ICardDataEditor
    {
        void Load();
        void Save();
        void CreateCard();
        void DrawCardFields();
    }
}
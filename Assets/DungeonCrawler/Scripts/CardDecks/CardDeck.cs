using DungeonCrawler.Cards;

namespace DungeonCrawler.CardDecks
{
    /// <summary>
    /// Deck of a specific card type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CardDeck<T> where T : Card
    {
        private T[] cards;
    }
}
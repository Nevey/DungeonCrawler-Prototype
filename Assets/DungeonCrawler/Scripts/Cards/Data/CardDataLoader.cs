using System.IO;
using Newtonsoft.Json;

namespace DungeonCrawler.Cards
{
    public class CardDataLoader<TCardData, TCardDataConfig>
        where TCardData : CardData
        where TCardDataConfig : ICardDataConfig, new()
    {
        public CardDataCollection<TCardData> Load()
        {
            TCardDataConfig config = new TCardDataConfig();
            string path = UnityEngine.Application.dataPath + config.Path;

            if (!File.Exists(path))
            {
                return new CardDataCollection<TCardData>
                {
                    cards = new TCardData[0]
                };
            }

            string cardJson = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<CardDataCollection<TCardData>>(cardJson);
        }
    }
}
using System;
using System.IO;
using Newtonsoft.Json;

namespace DungeonCrawler.Cards
{
    public class CardDataSaver<TCardData, TCardDataConfig>
        where TCardData : CardData
        where TCardDataConfig : ICardDataConfig, new()
    {
        public void Save(CardDataCollection<TCardData> collection)
        {
            TCardDataConfig config = new TCardDataConfig();
            string path = UnityEngine.Application.dataPath + config.Path;
            string json = JsonConvert.SerializeObject(collection);

            File.WriteAllText(path, String.Empty);
            File.WriteAllText(path, json);
        }
    }
}
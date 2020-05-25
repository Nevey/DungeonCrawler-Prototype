using System.IO;
using Newtonsoft.Json;

namespace DungeonCrawler.Cards
{
    public abstract class CardDataLoader<T> where T : CardData
    {
        protected abstract string Path { get; }

        public CardDataCollection<T> Load()
        {
            string path = UnityEngine.Application.dataPath + Path;
            string cardJson = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<CardDataCollection<T>>(cardJson);
        }
    }
}
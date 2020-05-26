using System;
using System.IO;
using Newtonsoft.Json;

namespace DungeonCrawler.Cards
{
    public abstract class CardDataSaver<T> where T : CardData
    {
        protected abstract string Path { get; }

        public void Save(CardDataCollection<T> collection)
        {
            string path = UnityEngine.Application.dataPath + Path;
            string json = JsonConvert.SerializeObject(collection);

            File.WriteAllText(path, String.Empty);
            File.WriteAllText(path, json);
        }
    }
}
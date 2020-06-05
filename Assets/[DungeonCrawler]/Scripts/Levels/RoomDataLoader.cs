using System.IO;
using CardboardCore.DI;
using Newtonsoft.Json;

namespace DungeonCrawler.Levels
{
    [Injectable]
    public class RoomDataLoader
    {
        public RoomData Load(string path)
        {
            string entityJson = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<RoomData>(entityJson);
        }
    }
}
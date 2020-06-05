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
            // If file doesn't exist, return this as default data
            if (!File.Exists(path))
            {
                return new RoomData
                {
                    id = 0,
                    gridSizeX = 5,
                    gridSizeY = 5,
                    tiles = new TileData[5, 5]
                };
            }

            string entityJson = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<RoomData>(entityJson);
        }
    }
}
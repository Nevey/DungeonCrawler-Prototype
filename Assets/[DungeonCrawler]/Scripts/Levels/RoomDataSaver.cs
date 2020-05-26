using System;
using System.IO;
using CardboardCore.DI;
using Newtonsoft.Json;

namespace DungeonCrawler.Levels
{
    [Injectable]
    public class RoomDataSaver
    {
        private string Path => "/[DungeonCrawler]/Configs/";

        public void Save(string roomName, RoomData room)
        {
            string path = UnityEngine.Application.dataPath + Path + roomName + ".json";
            string json = JsonConvert.SerializeObject(room);

            if (!File.Exists(path))
            {
                File.Create(path);
            }

            File.WriteAllText(path, String.Empty);
            File.WriteAllText(path, json);
        }
    }
}
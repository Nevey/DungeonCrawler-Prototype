using System;
using System.IO;
using CardboardCore.DI;
using Newtonsoft.Json;

namespace DungeonCrawler.Levels
{
    [Injectable]
    public class RoomDataSaver
    {
        private string Path => "/[DungeonCrawler]/Configs/Levels/";

        private string GetSavePath(RoomData roomData)
        {
            string roomName = "Room-" + roomData.id;
            return UnityEngine.Application.dataPath + Path + roomName + ".json";
        }

        public void Save(RoomData roomData)
        {
            string path = GetSavePath(roomData);
            string json = JsonConvert.SerializeObject(roomData);

            FileMode fileMode = Exists(roomData) ? FileMode.Truncate : FileMode.Create;

            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(path, fileMode, FileAccess.ReadWrite, FileShare.ReadWrite);

                using (TextWriter textWriter = new StreamWriter(fileStream))
                {
                    fileStream = null;
                    textWriter.Flush();
                    textWriter.Write(String.Empty);
                    textWriter.Write(json);
                }
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
        }

        public bool Exists(RoomData roomData)
        {
            string path = GetSavePath(roomData);
            return File.Exists(path);
        }
    }
}
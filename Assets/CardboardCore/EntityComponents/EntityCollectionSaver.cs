using System;
using System.IO;
using CardboardCore.DI;
using Newtonsoft.Json;
using UnityEngine;

namespace CardboardCore.EC
{
    [Injectable]
    public class EntityCollectionSaver
    {
        public void Save(IEntityLoadData entityLoadData, EntityDataCollection entityDataCollection)
        {
            string path = Application.dataPath + entityLoadData.Path;
            string json = JsonConvert.SerializeObject(entityDataCollection);

            File.WriteAllText(path, String.Empty);
            File.WriteAllText(path, json);
        }
    }
}

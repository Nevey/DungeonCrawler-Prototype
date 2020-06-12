using System.IO;
using CardboardCore.DI;
using Newtonsoft.Json;
using UnityEngine;

namespace CardboardCore.EC
{
    [Injectable]
    public class EntityCollectionLoader
    {
        public EntityDataCollection Load(IEntityLoadData entityLoadData)
        {
            string path = Application.dataPath + entityLoadData.Path;
            string entityJson = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<EntityDataCollection>(entityJson);
        }
    }
}

using System.IO;
using CardboardCore.Loop;
using CardboardCore.DI;
using Newtonsoft.Json;
using UnityEngine;

namespace CardboardCore.EntityComponents
{
    public class EntityFactory<T> where T : IEntityLoadData
    {
        [Inject] private UpdateLoop updateLoop;

        private EntityDataCollection entityDataCollection;

        public EntityFactory()
        {
            Injector.Inject(this);
        }

        ~EntityFactory()
        {
            Injector.Dump(this);
        }

        protected void Initialize(T entityLoadData)
        {
            string path = Application.dataPath + entityLoadData.Path;
            string entityJson = File.ReadAllText(path);

            entityDataCollection = JsonConvert.DeserializeObject<EntityDataCollection>(entityJson);
        }

        public Entity Instantiate(string id)
        {
            EntityData entityData = entityDataCollection.GetEntityWithId(id);
            Entity entity = new Entity(entityData);

            updateLoop.RegisterGameLoopable(entity);

            return entity;
        }
    }
}

using CardboardCore.Utilities;

namespace CardboardCore.EntityComponents
{
    public class EntityDataCollection
    {
        public EntityData[] entities { get; set; }

        public EntityData GetEntityWithId(string id)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i].id == id)
                {
                    return entities[i];
                }
            }

            throw Log.Exception($"Cannot find <b>EntityData</b> with id <b>{id}</b>!");
        }
    }
}

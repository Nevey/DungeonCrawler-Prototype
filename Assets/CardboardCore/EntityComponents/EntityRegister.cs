using System.Collections.Generic;
using CardboardCore.DI;

namespace CardboardCore.EntityComponents
{
    [Injectable(Singleton = true)]
    public class EntityRegister
    {
        private List<Entity> entities = new List<Entity>();

        public void RegisterEntity(Entity entity)
        {
            if (entities.Contains(entity))
            {
                return;
            }

            entities.Add(entity);
        }

        public void UnregisterEntity(Entity entity)
        {
            if (!entities.Contains(entity))
            {
                return;
            }

            entities.Remove(entity);
        }

        public Entity FindEntity(string name)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].name == name)
                {
                    return entities[i];
                }
            }

            return null;
        }

        public Entity[] FindEntities(string name)
        {
            List<Entity> foundEntities = new List<Entity>();

            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].name == name)
                {
                    foundEntities.Add(entities[i]);
                }
            }

            return foundEntities.ToArray();
        }
    }
}
using CardboardCore.Utilities;

namespace CardboardCore.EntityComponents
{
    public class EntityData
    {
        public string id { get; set; }
        public ComponentData[] components { get; set; }

        public EntityData()
        {
            components = new ComponentData[0];
        }

        public ComponentData GetComponentDataWithId(string id)
        {
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].id == id)
                {
                    return components[i];
                }
            }

            return null;
        }
    }
}

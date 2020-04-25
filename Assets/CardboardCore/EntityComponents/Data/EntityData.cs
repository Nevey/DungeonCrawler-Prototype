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
    }
}

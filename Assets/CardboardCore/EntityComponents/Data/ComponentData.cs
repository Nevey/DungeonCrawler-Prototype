using CardboardCore.Utilities;

namespace CardboardCore.EntityComponents
{
    public class ComponentData
    {
        public string id { get; set; }
        public FieldData[] fields { get; set; }

        public FieldData GetFieldDataWithId(string fieldId)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].id == fieldId)
                {
                    return fields[i];
                }
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using CardboardCore.Loop;
using CardboardCore.Utilities;

namespace CardboardCore.EntityComponents
{
    public class Entity : IGameLoopable
    {
        public readonly string name;

        private readonly List<Component> components = new List<Component>();


        public Entity(EntityData entityData)
        {
            name = entityData.id;

            for (int i = 0; i < entityData.components.Length; i++)
            {
                ComponentData componentData = entityData.components[i];

                Type type = Reflection.GetType(componentData.id);

                Component component = Activator.CreateInstance(type, this) as Component;

                FieldInfo[] fieldInfos = Reflection.GetFieldsWithAttribute<TweakableFieldAttribute>(type);

                for (int k = 0; k < fieldInfos.Length; k++)
                {
                    FieldInfo fieldInfo = fieldInfos[k];

                    FieldData fieldData = componentData.GetFieldDataWithId(fieldInfo.Name);

                    if (fieldData == null)
                    {
                        continue;
                    }

                    // TODO: Fix this, now we cannot support long (or short)
                    // Newtonsoft Json library automatically converts numerics into long.
                    if (fieldData.value.GetType() == typeof(long))
                    {
                        fieldData.value = Convert.ToInt32(fieldData.value);
                    }

                    fieldInfo.SetValue(component, fieldData.value);
                }

                components.Add(component);
            }
        }

        public void Start()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Start();
            }
        }

        public void Stop()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Stop();
            }
        }

        public void Update(double deltaTime)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Update(deltaTime);
            }
        }

        public T AddComponent<T>() where T : Component, new()
        {
            Type type = typeof(T);
            T component = new T();

            component.Start();
            components.Add(component);

            return component;
        }

        public bool RemoveComponent<T>() where T : Component
        {
            Component component = GetComponent<T>();

            if (component == null)
            {
                return false;
            }

            component.Stop();

            components.Remove(component);

            return true;
        }

        public T GetComponent<T>(bool throwException = false) where T : Component
        {
            Type type = typeof(T);

            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].GetType() == type)
                {
                    return components[i] as T;
                }
            }

            if (throwException)
            {
                throw Log.Exception($"Component of type <b>{type.Name}</b> could not be found on Entity <b>{name}</b>!");
            }

            return null;
        }
    }
}

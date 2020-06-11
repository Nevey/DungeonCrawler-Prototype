using System;
using System.Collections.Generic;
using System.Reflection;
using CardboardCore.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CardboardCore.DI
{
    /// <summary>
    /// Handles the actual injection into and dumping from fields. Multiple of these layers can co-exist.
    /// Keeps track of references to specific injections and cleans them up when being dumped.
    /// </summary>
    public class InjectionLayer
    {
        private readonly Dictionary<Type, object> dependencies = new Dictionary<Type, object>();
        private readonly Dictionary<object, List<object>> references = new Dictionary<object, List<object>>();
        private readonly List<object> singletons = new List<object>();

        public void InjectIntoField(FieldInfo fieldInfo, InjectableAttribute injectableAttribute, object @object)
        {
            object injectedInstance;

            if (injectableAttribute.Singleton)
            {
                if (dependencies.ContainsKey(fieldInfo.FieldType))
                {
                    injectedInstance = dependencies[fieldInfo.FieldType];
                }
                else
                {
                    if (fieldInfo.FieldType.IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        injectedInstance = Object.FindObjectOfType(fieldInfo.FieldType);

                        if (injectedInstance == null)
                        {
                            injectedInstance = new GameObject().AddComponent(fieldInfo.FieldType);
                            MonoBehaviour.DontDestroyOnLoad(((MonoBehaviour)injectedInstance).gameObject);
                        }
                    }
                    else
                    {
                        injectedInstance = Activator.CreateInstance(fieldInfo.FieldType);
                    }
                }

                if (injectedInstance == null)
                {
                    throw Log.Exception(
                        $"Something went wrong while trying to assign " +
                        $"Service of type <b>{fieldInfo.FieldType}</b>");
                }

                dependencies[fieldInfo.FieldType] = injectedInstance;

                // Track all singletons
                if (!singletons.Contains(injectedInstance))
                {
                    singletons.Add(injectedInstance);
                }

                // Track references to this singleton injected instance
                if (references.ContainsKey(injectedInstance))
                {
                    if (references[injectedInstance].Contains(@object))
                    {
                        throw Log.Exception(
                            $"Trying to inject the same Object of type " +
                            $"<b>{fieldInfo.FieldType}</b> twice in <b>{@object}</b>");
                    }

                    references[injectedInstance].Add(@object);
                }
                else
                {
                    references[injectedInstance] = new List<object> { @object };
                }
            }
            else
            {
                if (fieldInfo.FieldType.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    throw Log.Exception($"Non-Singleton Monobehaviour support is not added yet!"
                        + $"Please change <b>{fieldInfo.FieldType.Name}</b> to be a <b>Singleton</b> Injectable");
                }

                // TODO: Add Monobehaviour support
                injectedInstance = Activator.CreateInstance(fieldInfo.FieldType);

                Log.Write($"Non-Singleton <b>{injectedInstance.GetType().Name}</b> was created");
            }

            fieldInfo.SetValue(@object, injectedInstance);

            if (injectableAttribute.Singleton)
            {
                Log.Write($"Injected <i>Singleton</i> instance <b>{injectedInstance.GetType().Name}</b> " +
                          $"into <b>{@object.GetType().Name}</b> -- " +
                          $"Has <b>{references[injectedInstance].Count}</b> reference(s)");
            }
            else
            {
                Log.Write($"Created instance <b>{injectedInstance.GetType().Name}</b> -- " +
                          $"Was injected into <b>{@object.GetType().Name}</b>");
            }
        }

        public void DumpDependencies(object @object)
        {
            List<object> instancesToDestroy = new List<object>();

            foreach (KeyValuePair<object, List<object>> keyValuePair in references)
            {
                object injectedInstance = keyValuePair.Key;

                List<object> objects = keyValuePair.Value;

                for (int i = 0; i < objects.Count; i++)
                {
                    if (objects[i] != @object)
                    {
                        continue;
                    }

                    objects.RemoveAt(i);

                    Log.Write($"<i>Singleton</i> instance <b>{injectedInstance.GetType().Name}</b> -- " +
                              $"Dumped by <b>{@object.GetType().Name}</b> -- " +
                              $"Has <b>{references[injectedInstance].Count}</b> reference(s)");

                    break;
                }

                // Injected instance has no more references, time to clean it up
                if (objects.Count == 0)
                {
                    Type type = injectedInstance.GetType();

                    // Only add non-singletons to list of instances to destroy
                    if (singletons.Contains(injectedInstance))
                    {
                        continue;
                    }

                    dependencies.Remove(type);
                    instancesToDestroy.Add(injectedInstance);
                }
            }

            // Finally remove references by key
            for (int i = 0; i < instancesToDestroy.Count; i++)
            {
                Log.Write(
                    $"Clearing instance of " +
                    $"<b>{instancesToDestroy[i].GetType().Name}</b> as it has no more references left");

                references.Remove(instancesToDestroy[i]);

                if (instancesToDestroy[i].GetType().IsSubclassOf(typeof(MonoBehaviour)))
                {
                    MonoBehaviour mb = (MonoBehaviour)instancesToDestroy[i];
                    if (mb == null)
                    {
                        continue;
                    }

                    GameObject gameObject = mb.gameObject;

                    MonoBehaviour.Destroy(mb);

                    if (gameObject.GetComponents<Component>().Length == 0)
                    {
                        MonoBehaviour.Destroy(gameObject);
                    }
                }
                else
                {
                    instancesToDestroy[i] = null;
                }
            }
        }
    }
}

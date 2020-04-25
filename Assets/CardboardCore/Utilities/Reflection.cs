using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CardboardCore.Utilities
{
    public static class Reflection
    {

        public static Type[] FindDerivedTypes<T>()
        {
            Type baseType = typeof(T);
            Assembly assembly = baseType.Assembly;

            return assembly.GetTypes().Where(t => t != baseType && baseType.IsAssignableFrom(t)).ToArray();
        }

        public static Type[] GetTypes<T>()
        {
            Type type = typeof(T);

            List<Type> typeList = new List<Type>();

            // TODO: Search through less assemblies, this is overkill...
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly appDomain in assemblies)
            {
                try
                {
                    Type[] types = appDomain.GetTypes().Where(t => type.IsAssignableFrom(t)).ToArray();
                    typeList.AddRange(types);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    throw Log.Exception($"Error while loading types for domain {appDomain.FullName}: {ex.Message}");
                }
            }

            return typeList.ToArray();
        }

        public static Type GetType(string typeString)
        {
            Type type = null;

            // TODO: Search through less assemblies, this is overkill...
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly appDomain in assemblies)
            {
                try
                {
                    Type[] types = appDomain.GetTypes();

                    for (int i = 0; i < types.Length; i++)
                    {
                        Type t = types[i];

                        // This can easily cause ambiguous cases...
                        if (t.Name == typeString)
                        {
                            type = t;
                            break;
                        }
                    }

                    if (type != null)
                    {
                        break;
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    throw Log.Exception($"Error while loading types for domain {appDomain.FullName}: {ex.Message}");
                }
            }

            return type;
        }

        public static MethodInfo[] GetMethodsWithCustomAttribute<T>() where T : Attribute
        {
            List<MethodInfo> methodList = new List<MethodInfo>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly appDomain in assemblies)
            {
                try
                {
                    Type[] types = appDomain.GetTypes();

                    foreach (Type type in types)
                    {
                        MethodInfo[] methods = type.GetMethods();

                        foreach (MethodInfo methodInfo in methods)
                        {
                            T attribute = methodInfo.GetCustomAttribute<T>();

                            if (attribute == null)
                            {
                                continue;
                            }

                            methodList.Add(methodInfo);
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    throw Log.Exception($"Error while loading types for domain {appDomain.FullName}: {ex.Message}");
                }
            }

            return methodList.ToArray();
        }

        public static FieldInfo[] GetFieldsWithCustomAttribute<T>() where T : Attribute
        {
            List<FieldInfo> fieldInfoList = new List<FieldInfo>();

            // TODO: Classes using this attribute could be contained in another assembly
            Type[] types = typeof(T).Assembly.GetTypes();

            foreach (Type type in types)
            {
                FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic |
                                                    BindingFlags.Instance | BindingFlags.Public);

                foreach (FieldInfo fieldInfo in fields)
                {
                    T attribute = fieldInfo.GetCustomAttribute<T>();

                    if (attribute == null)
                    {
                        continue;
                    }

                    fieldInfoList.Add(fieldInfo);
                }
            }

            return fieldInfoList.ToArray();
        }

        public static FieldInfo[] GetFieldsWithAttribute<T>(Type type) where T : Attribute
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic |
                                                BindingFlags.Instance | BindingFlags.Public);

            List<FieldInfo> fieldInfoList = new List<FieldInfo>();

            foreach (FieldInfo fieldInfo in fields)
            {
                T attribute = fieldInfo.GetCustomAttribute<T>();

                if (attribute == null)
                {
                    continue;
                }

                fieldInfoList.Add(fieldInfo);
            }

            if (type.BaseType != null)
            {
                fieldInfoList.AddRange(GetFieldsWithAttribute<T>(type.BaseType));
            }

            return fieldInfoList.ToArray();
        }
    }
}

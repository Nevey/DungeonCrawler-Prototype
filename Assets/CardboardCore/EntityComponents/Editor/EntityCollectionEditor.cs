using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CardboardCore.DI;
using DungeonCrawler.EntityComponents;
using CardboardCore.Utilities;
using System.Reflection;

namespace CardboardCore.EntityComponents
{
    public static class ValueTypedEditorGUILayout
    {
        public static object Draw(object value, params GUILayoutOption[] options)
        {
            Type type = value.GetType();

            if (type == typeof(int))
            {
                value = EditorGUILayout.IntField((int)value, options);
            }
            else if (type == typeof(long) || type == typeof(short))
            {
                value = Convert.ChangeType(value, typeof(int));
                value = EditorGUILayout.IntField((int)value, options);
            }
            else if (type == typeof(float))
            {
                value = EditorGUILayout.FloatField((float)value, options);
            }
            else if (type == typeof(string))
            {
                value = EditorGUILayout.TextField((string)value, options);
            }
            else if (type == typeof(bool))
            {
                value = EditorGUILayout.Toggle((bool)value, options);
            }

            return value;
        }
    }

    public class EntityCollectionEditor : EditorWindow
    {
        [Inject] private EntityCollectionLoader entityCollectionLoader;
        [Inject] private EntityCollectionSaver entityCollectionSaver;

        private EntityDataCollection entityDataCollection;
        private Vector2 scrollPosition;
        private IEntityLoadData entityLoadData;
        private int selectedIndex;

        [MenuItem("EntityCollection/Editor")]
        private static void ShowWindow()
        {
            EntityCollectionEditor window = GetWindow<EntityCollectionEditor>();
            window.titleContent = new GUIContent("EntityCollection");
            window.Show();
        }

        private void OnEnable()
        {
            Injector.Inject(this);

            // TODO: Find all IEntityLoadData and pick one
            entityLoadData = new GameEntityLoadData();

            entityDataCollection = entityCollectionLoader.Load(entityLoadData);
        }

        private void OnDisable()
        {
            Injector.Dump(this);
        }

        private void OnInspectorUpdate()
        {
            SaveToFile();
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.BeginVertical("box");

            DrawEntityDataCollection();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();

            DrawCreateEntityButton();
        }

        private void DrawEntityDataCollection()
        {
            EditorGUILayout.BeginVertical("box");

            for (int i = 0; i < entityDataCollection.entities.Length; i++)
            {
                DrawEntityData(entityDataCollection.entities[i]);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawEntityData(EntityData entityData)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            entityData.id = EditorGUILayout.TextField(entityData.id, GUILayout.Width(200));

            DrawRemoveEntityButton(entityData);

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < entityData.components.Length; i++)
            {
                DrawComponentData(entityData, entityData.components[i]);
            }

            DrawAddComponentButton(entityData);

            EditorGUILayout.EndVertical();
        }

        private void DrawComponentData(EntityData entityData, ComponentData componentData)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(componentData.id, GUILayout.Width(200));

            DrawRemoveComponentButton(entityData, componentData);

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < componentData.fields.Length; i++)
            {
                DrawFieldData(componentData.fields[i]);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawFieldData(FieldData fieldData)
        {
            EditorGUILayout.BeginHorizontal("box");

            EditorGUILayout.LabelField(fieldData.id, GUILayout.Width(100f));

            if (fieldData.value == null)
            {
                EditorGUILayout.LabelField("null");
            }
            else
            {
                fieldData.value = ValueTypedEditorGUILayout.Draw(fieldData.value);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawRemoveEntityButton(EntityData entityData)
        {
            if (GUILayout.Button("Remove Entity", GUILayout.Width(100)))
            {
                List<EntityData> entities = entityDataCollection.entities.ToList();

                entities.Remove(entityData);

                entityDataCollection.entities = entities.ToArray();
            }
        }

        private void DrawRemoveComponentButton(EntityData entityData, ComponentData componentData)
        {
            if (GUILayout.Button("Remove Component", GUILayout.Width(150f)))
            {
                List<ComponentData> components = entityData.components.ToList();

                components.Remove(componentData);

                entityData.components = components.ToArray();
            }
        }

        private void DrawAddComponentButton(EntityData entityData)
        {
            EditorGUILayout.BeginHorizontal();

            Type[] types = Reflection.FindDerivedTypes<Component>();

            string[] names = new string[types.Length];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = types[i].Name;
            }

            selectedIndex = EditorGUILayout.Popup(selectedIndex, names, GUILayout.Width(150));

            if (GUILayout.Button("Add Component", GUILayout.Width(100)))
            {
                ComponentData componentData = new ComponentData();
                componentData.id = names[selectedIndex];

                List<ComponentData> components = entityData.components.ToList();
                components.Add(componentData);
                entityData.components = components.ToArray();

                Type componentType = types[selectedIndex];

                FieldInfo[] fieldInfos = Reflection.GetFieldsWithAttribute<TweakableFieldAttribute>(componentType);

                FieldData[] fields = new FieldData[fieldInfos.Length];

                for (int i = 0; i < fields.Length; i++)
                {
                    FieldInfo fieldInfo = fieldInfos[i];
                    FieldData fieldData = new FieldData();

                    Type fieldType = fieldInfo.FieldType;

                    fieldData.id = fieldInfo.Name;
                    fieldData.value = fieldType == typeof(String) ? "" as string : Activator.CreateInstance(fieldType);

                    fields[i] = fieldData;
                }

                componentData.fields = fields;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawCreateEntityButton()
        {
            GUILayout.Space(25f);

            if (GUILayout.Button("Create Entity", GUILayout.Width(150f)))
            {
                List<EntityData> entities = entityDataCollection.entities.ToList();

                EntityData entityData = new EntityData();
                entityData.id = $"My Entity {entities.Count}";

                entities.Add(entityData);

                entityDataCollection.entities = entities.ToArray();
            }
        }

        private void SaveToFile()
        {
            entityCollectionSaver.Save(entityLoadData, entityDataCollection);
        }
    }
}

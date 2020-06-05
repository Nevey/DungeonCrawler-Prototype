using System;
using CardboardCore.DI;
using UnityEditor;
using UnityEngine;

namespace DungeonCrawler.Levels
{
    public class RoomPropertiesEditor : EditorWindow
    {
        [Inject] private RoomDataSaver roomDataSaver;

        private RoomData roomData;
        private static Action<RoomData> callback;

        public static void ShowWindow(Action<RoomData> cb)
        {
            RoomPropertiesEditor window = GetWindow<RoomPropertiesEditor>();
            window.minSize = new Vector2(400, 150);
            window.maxSize = new Vector2(400, 150);
            window.titleContent = new GUIContent("Room Creator");
            window.Show();

            callback = cb;
        }

        private void OnEnable()
        {
            Injector.Inject(this);

            roomData = new RoomData();
        }

        private void OnDisable()
        {
            Injector.Dump(this);
        }

        private void OnGUI()
        {
            DrawRoomDataProperties();
            DrawFinishButton();
        }

        private void DrawRoomDataProperties()
        {
            EditorGUILayout.BeginVertical("box");

            roomData.id = EditorGUILayout.IntField("Room ID", roomData.id, GUILayout.Width(200));

            EditorGUILayout.BeginHorizontal();
            roomData.gridSizeX = EditorGUILayout.IntField("Grid Size X", roomData.gridSizeX, GUILayout.Width(190));
            roomData.gridSizeY = EditorGUILayout.IntField("Grid Size Y", roomData.gridSizeY, GUILayout.Width(190));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            // TODO: Only do this on grid size value change
            roomData.tiles = new TileData[roomData.gridSizeX, roomData.gridSizeY];

            for (int x = 0; x < roomData.gridSizeX; x++)
            {
                for (int y = 0; y < roomData.gridSizeY; y++)
                {
                    roomData.tiles[x, y] = new TileData
                    {
                        x = x,
                        y = y,
                        walkableState = WalkableState.Walkable
                    };
                }
            }
        }

        private void DrawFinishButton()
        {
            EditorGUILayout.Space();

            if (GUILayout.Button("Finish", GUILayout.Width(200)))
            {
                if (roomDataSaver.Exists(roomData))
                {
                    if (EditorUtility.DisplayDialog("File Exists!", $"Room with ID {roomData.id} already exists!", "Overwrite!", "Go Back..."))
                    {
                        callback?.Invoke(roomData);
                        Close();
                    }
                }
                else
                {
                    callback?.Invoke(roomData);
                    Close();
                }

            }
        }
    }
}
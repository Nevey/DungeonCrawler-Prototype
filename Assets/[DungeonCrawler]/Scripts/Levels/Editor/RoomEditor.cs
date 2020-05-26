using CardboardCore.DI;
using UnityEditor;
using UnityEngine;

namespace DungeonCrawler.Levels
{
    public class RoomEditor : EditorWindow
    {
        [Inject] private RoomDataSaver roomDataSaver;

        private RoomData roomData;

        [MenuItem("DunngeonCrawler/Rooms")]
        private static void ShowWindow()
        {
            RoomEditor window = GetWindow<RoomEditor>();
            window.titleContent = new GUIContent("RoomEditor");
            window.Show();
        }

        private void OnEnable()
        {
            Injector.Inject(this);
        }

        private void OnDisable()
        {
            Injector.Dump(this);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("box");
            DrawCreateButton();
            DrawLoadButton();
            EditorGUILayout.EndHorizontal();

            DrawSaveButton();
        }

        private void DrawCreateButton()
        {
            if (GUILayout.Button("Create"))
            {
                // TODO: Room id should be based on amount of available rooms + 1
                roomData = new RoomData(0, 5, 5);
            }
        }

        private void DrawLoadButton()
        {
            if (GUILayout.Button("Load"))
            {

            }
        }

        private void DrawGrid()
        {

        }

        private void DrawSaveButton()
        {

        }
    }
}
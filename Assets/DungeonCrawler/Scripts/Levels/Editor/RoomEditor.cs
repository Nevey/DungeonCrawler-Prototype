using CardboardCore.DI;
using CardboardCore.Utilities;
using UnityEditor;
using UnityEngine;

namespace DungeonCrawler.Levels
{
    public class RoomEditor : EditorWindow
    {
        [Inject] private RoomDataSaver roomDataSaver;
        [Inject] private RoomDataLoader roomDataLoader;

        private RoomData roomData;

        private Texture tileSprite;
        private Texture tileDoorwaySprite;
        private Texture tileUnusedSprite;

        [MenuItem("DungeonCrawler/Rooms")]
        private static void ShowWindow()
        {
            RoomEditor window = GetWindow<RoomEditor>();
            window.titleContent = new GUIContent("Room Editor");
            window.Show();
        }

        private void OnEnable()
        {
            Injector.Inject(this);

            tileSprite = AssetUtility.LoadAssetAtPath<Texture>("Assets/DungeonCrawler/Textures/Editor/tile-room.jpg");
            tileDoorwaySprite = AssetUtility.LoadAssetAtPath<Texture>("Assets/DungeonCrawler/Textures/Editor/tile-room-doorway.jpg");
            tileUnusedSprite = AssetUtility.LoadAssetAtPath<Texture>("Assets/DungeonCrawler/Textures/Editor/tile-room-unused.jpg");
        }

        private void OnDisable()
        {
            Injector.Dump(this);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal("box");
            DrawCreateButton();
            DrawLoadButton();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            if (roomData == null)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal("box");
            DrawRoomName();
            EditorGUILayout.EndHorizontal();

            DrawGrid();
        }

        private void OnInspectorUpdate()
        {
            if (roomData == null)
            {
                return;
            }

            roomDataSaver.Save(roomData);
        }

        private void DrawCreateButton()
        {
            if (GUILayout.Button("Create"))
            {
                RoomCreatorEditor.ShowWindow(roomData =>
                {
                    this.roomData = roomData;
                    Repaint();
                });
            }
        }

        private void DrawLoadButton()
        {
            if (GUILayout.Button("Load"))
            {
                string path = EditorUtility.OpenFilePanel("", UnityEngine.Application.dataPath + "/[DungeonCrawler]/Configs/Levels/", "json");
                roomData = roomDataLoader.Load(path);
            }
        }

        private void DrawRoomName()
        {
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.white;

            EditorGUILayout.LabelField($"Room - {roomData.id}", style);
        }

        private void DrawGrid()
        {
            for (int x = 0; x < roomData.gridSizeX; x++)
            {
                for (int y = 0; y < roomData.gridSizeY; y++)
                {
                    // TODO: cleanup
                    int space = 30;

                    Rect rect = new Rect(
                        position.width / 2f - space * (roomData.gridSizeX / 2f) + space * x,
                        100 + space * y,
                        28,
                        28);

                    TileState tileState = roomData.tiles[x, y].tileState;
                    Texture texture = GetTexture(tileState);

                    GUI.DrawTexture(rect, texture);

                    TapOnTile(x, y, rect);
                }
            }
        }

        private void TapOnTile(int x, int y, Rect rect)
        {
            if (rect.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    if (Event.current.button == 0)
                    {
                        roomData.tiles[x, y].tileState = TileState.Default;
                    }

                    if (Event.current.button == 1)
                    {
                        roomData.tiles[x, y].tileState = TileState.Unused;
                    }

                    if (Event.current.button == 2)
                    {
                        roomData.tiles[x, y].tileState = TileState.Doorway;
                    }
                }

                Repaint();
            }
        }

        private Texture GetTexture(TileState tileState)
        {
            switch (tileState)
            {
                case TileState.Doorway:
                    return tileDoorwaySprite;

                case TileState.Unused:
                    return tileUnusedSprite;

                default:
                    return tileSprite;
            }
        }
    }
}
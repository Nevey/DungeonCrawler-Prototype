using CardboardCore.DI;
using UnityEditor;
using UnityEngine;

namespace DungeonCrawler.Cards
{
    public class CardEditor : EditorWindow
    {
        [Inject] private RoomCardDataLoader roomCardDataLoader;
        [Inject] private RoomCardDataSaver roomCardDataSaver;

        private CardDataCollection<RoomCardData> roomCardDataCollection;

        [MenuItem("DunngeonCrawler/Cards")]
        private static void ShowWindow()
        {
            CardEditor window = GetWindow<CardEditor>();
            window.titleContent = new GUIContent("CardEditor");
            window.Show();
        }

        private void OnEnable()
        {
            Injector.Inject(this);

            roomCardDataCollection = roomCardDataLoader.Load();
        }

        private void OnDisable()
        {
            Injector.Dump(this);
        }

        private void OnGUI()
        {
            // Draw tabs based on different CardData types

            DrawRoomCards();
        }

        private void OnInspectorUpdate()
        {
            SaveToFile();
        }

        private void DrawRoomCards()
        {
            EditorGUILayout.BeginVertical();

            for (int i = 0; i < roomCardDataCollection.cards.Length; i++)
            {
                RoomCardData card = roomCardDataCollection.cards[i];

                EditorGUILayout.BeginVertical();

                EditorGUILayout.LabelField(card.name);

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
        }

        private void SaveToFile()
        {
            roomCardDataSaver.Save(roomCardDataCollection);
        }
    }
}
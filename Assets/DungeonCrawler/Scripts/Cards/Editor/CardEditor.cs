using System;
using CardboardCore.DI;
using CardboardCore.Utilities;
using UnityEditor;
using UnityEngine;

namespace DungeonCrawler.Cards
{
    public class CardEditor : EditorWindow
    {
        private Type[] cardDataTypes;

        private Vector2 scrollPosition;

        private ICardDataEditor cardDataEditor;

        [MenuItem("DungeonCrawler/Cards")]
        private static void ShowWindow()
        {
            CardEditor window = GetWindow<CardEditor>();
            window.titleContent = new GUIContent("Card Editor");
            window.Show();
        }

        private void OnEnable()
        {
            Injector.Inject(this);

            cardDataTypes = Reflection.FindDerivedTypes<CardData>();
        }

        private void OnDisable()
        {
            Injector.Dump(this);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("box");
            DrawCardDataButtons();
            EditorGUILayout.EndHorizontal();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            cardDataEditor?.DrawCardFields();
            EditorGUILayout.EndScrollView();

            DrawCreateButton();
        }

        private void OnInspectorUpdate()
        {
            cardDataEditor?.Save();
        }

        private void DrawCardDataButtons()
        {
            for (int i = 0; i < cardDataTypes.Length; i++)
            {
                Type cardDataType = cardDataTypes[i];

                if (GUILayout.Button(cardDataType.Name))
                {
                    // TODO: If this list gets too big, revamp into something automated
                    if (cardDataType == typeof(RoomCardData))
                    {
                        cardDataEditor = new GenericCardDataEditor<RoomCardData, RoomCardDataConfig>();
                    }

                    if (cardDataType == typeof(TileCardData))
                    {
                        cardDataEditor = new GenericCardDataEditor<TileCardData, TileCardDataConfig>();
                    }

                    cardDataEditor.Load();
                }
            }
        }

        private void DrawCreateButton()
        {
            GUILayout.Space(25f);

            if (GUILayout.Button("Create Card", GUILayout.Width(150f)))
            {
                cardDataEditor?.CreateCard();
            }
        }
    }
}
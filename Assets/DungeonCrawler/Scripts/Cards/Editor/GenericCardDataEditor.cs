using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CardboardCore.EC;
using CardboardCore.Utilities;
using DungeonCrawler.Cards;
using UnityEditor;
using UnityEngine;

public class GenericCardDataEditor<TCardData, TCardDataConfig> : ICardDataEditor
        where TCardData : CardData, new()
        where TCardDataConfig : ICardDataConfig, new()
{
    private CardDataCollection<TCardData> collection;
    private CardDataLoader<TCardData, TCardDataConfig> loader = new CardDataLoader<TCardData, TCardDataConfig>();
    private CardDataSaver<TCardData, TCardDataConfig> saver = new CardDataSaver<TCardData, TCardDataConfig>();

    private Dictionary<CardData, FieldData[]> cardFieldsDictionary;

    private void SetupCardFields(CardData[] cards)
    {
        cardFieldsDictionary = new Dictionary<CardData, FieldData[]>();

        for (int i = 0; i < cards.Length; i++)
        {
            FieldInfo[] fields = Reflection.GetFields(cards[i].GetType());

            cardFieldsDictionary[cards[i]] = new FieldData[fields.Length];

            for (int k = 0; k < fields.Length; k++)
            {
                object value = fields[k].GetValue(cards[i]);
                cardFieldsDictionary[cards[i]][k] = new FieldData
                {
                    id = fields[k].Name,
                    value = value
                };
            }
        }
    }

    private void RemoveCard(int index)
    {
        List<TCardData> cards = collection.cards.ToList();
        cards.RemoveAt(index);

        collection.cards = cards.ToArray();

        SetupCardFields(collection.cards);
    }

    public void Load()
    {
        collection = loader.Load();
        SetupCardFields(collection.cards);
    }

    public void Save()
    {
        int cardIndex = 0;
        foreach (var cardWithFields in cardFieldsDictionary)
        {
            TCardData cardData = collection.cards[cardIndex];

            FieldInfo[] fields = Reflection.GetFields(cardData.GetType());

            for (int i = 0; i < cardWithFields.Value.Length; i++)
            {
                fields[i].SetValue(cardData, cardWithFields.Value[i].value);
            }

            collection.cards[cardIndex] = cardData;

            cardIndex++;
        }

        saver.Save(collection);
    }

    public void CreateCard()
    {
        int? freeId = null;

        for (int i = 0; i < collection.cards.Length; i++)
        {
            if (collection.cards[i].id != i)
            {
                freeId = i;
                break;
            }
        }

        if (freeId == null)
        {
            freeId = collection.cards.Length;
        }

        TCardData cardData = new TCardData
        {
            id = freeId.Value,
            name = $"New Card {freeId.Value}"
        };

        List<TCardData> cards = collection.cards.ToList();
        cards.Add(cardData);

        collection.cards = cards.ToArray();

        SetupCardFields(collection.cards);
    }

    public void DrawCardFields()
    {
        EditorGUILayout.BeginVertical();

        int cardIndex = 0;

        foreach (var cardWithFields in cardFieldsDictionary)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField($"{cardWithFields.Key.GetType().Name} - ID: {cardWithFields.Key.id}");

            for (int k = 0; k < cardWithFields.Value.Length; k++)
            {
                FieldData fieldData = cardWithFields.Value[k];

                EditorGUILayout.BeginHorizontal("box");

                EditorGUILayout.LabelField(fieldData.id, GUILayout.Width(100));
                fieldData.value = ValueTypedEditorGUILayout.Draw(fieldData.value, GUILayout.Width(150));

                cardWithFields.Value[k] = fieldData;

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Remove Card"))
            {
                RemoveCard(cardIndex);
            }

            cardIndex++;

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }
}

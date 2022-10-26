using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DBManager : MonoBehaviour
{
    public static string username;
    public static int level;
    public static string heroType;
    public static string heroName;
    public static int heroLevel;
    public static int userID;
    public static int enemyDefeated;
    public static int currency;
    public static List<Card> allCards;
    public static List<Card> cardCollection;
    public static List<Card> deck;
    public static Font belweb;

    public static bool loggedIn { get { return username != null; } }

    public static void LogOut()
    {
        username = null;
    }

    public static void cardsFromDB(string cardCollectionFromDB, string deckFromDB)
    {
        List<Card> cardCollectionTemp = new List<Card>();
        List<Card> deckTemp = new List<Card>();
        Debug.Log(cardCollectionFromDB + " " + deckFromDB);
        foreach (var item in cardCollectionFromDB.Split('.'))
        {
            for (int i = 0; i < int.Parse(item.Split('x')[1]); i++)
            {
                foreach (var card in allCards)
                {
                    if (card.id == int.Parse(item.Split('x')[0]))
                    {
                        cardCollectionTemp.Add(card);
                    }
                }
            }
        }

        foreach (var item in deckFromDB.Split('.'))
        {
            for (int i = 0; i < int.Parse(item.Split('x')[1]); i++)
            {
                foreach (var card in allCards)
                {
                    if (card.id == int.Parse(item.Split('x')[0]))
                    {
                        deckTemp.Add(card);
                    }
                }
            }
        }
        cardCollection = cardCollectionTemp;
        deck = deckTemp;
        cardCollection.OrderBy(t => t.id).ThenBy(t => t.cost);
        deck.OrderBy(t => t.id);
    }
}

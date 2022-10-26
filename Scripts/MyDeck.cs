using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MyDeck : MonoBehaviour
{
    public GameObject collCard;
    public GameObject container;
    public List<CollectionCards> collection;
    public List<CollectionCards> tempDeck;
    public float alpOfMissingCard = 0.3f;

    public class CollectionCards
    {
        public Card card { get; set; }
        public int cnt { get; set; }

        public CollectionCards(Card collCard)
        {
            cnt = 0;
            card = collCard;
        }
    }

    public void MyDeckStart()
    {
        container = GameObject.Find("DeckCollContent");
        collection = new List<CollectionCards>();
        tempDeck = new List<CollectionCards>();
        var deck = DBManager.deck;
        HashSet<Card> cardHash = new HashSet<Card>(DBManager.cardCollection);
        HashSet<Card> deckHash = new HashSet<Card>(DBManager.deck);

        foreach (var item in cardHash)
        {
            collection.Add(new CollectionCards(item));
        }

        foreach (var item in DBManager.cardCollection.GroupBy(g => g).Select(group => new { card = group.Key, cnt = group.Count() }))
        {
            foreach (var collitem in collection)
            {
                if (item.card.id == collitem.card.id)
                {
                    collitem.cnt = item.cnt;
                    break;
                }
            }
        }

        foreach (var item in deck)
        {
            foreach (var collitem in collection)
            {
                if (collitem.card.id == item.id)
                {
                    collitem.cnt -= 1;
                    break;
                }
            }
        }

        foreach (var item in collection)
        {
            NewCard(item.card, item.cnt);
        }

        container = GameObject.Find("DeckContent");

        foreach (var item in DBManager.deck.GroupBy(g => g).Select(g => new { card = g.Key, cnt = g.Count() }))
        {
            NewCard(item.card, item.cnt);
        }

        foreach (var item in deckHash)
        {
            tempDeck.Add(new CollectionCards(item));
        }

        foreach (var item in DBManager.deck.GroupBy(g => g).Select(g => new { card = g.Key, cnt = g.Count() }))
        {
            foreach (var deckitem in tempDeck)
            {
                if (item.card.id == deckitem.card.id)
                {
                    deckitem.cnt = item.cnt;
                    break;
                }
            }
        }
    }

    public void NewCard(Card card, int cnt)
    {
        var newCard = Instantiate(collCard, container.transform.position, Quaternion.identity);
        newCard.GetComponent<CollectionCardDisplay>().card = card;
        newCard.transform.SetParent(container.transform);
        newCard.GetComponent<RectTransform>().localScale = new Vector3(1.3f, 1.3f, 1);

        newCard.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().text = "x" + cnt;

        if (cnt == 0)
        {
            newCard.transform.Find("CardImage").GetComponent<Image>().color = new Color(newCard.transform.Find("CardImage").GetComponent<Image>().color.r
                , newCard.transform.Find("CardImage").GetComponent<Image>().color.g, newCard.transform.Find("CardImage").GetComponent<Image>().color.b, alpOfMissingCard);
            newCard.transform.Find("CollCount").GetComponent<Image>().color = new Color(newCard.transform.Find("CollCount").GetComponent<Image>().color.r
                , newCard.transform.Find("CollCount").GetComponent<Image>().color.g, newCard.transform.Find("CollCount").GetComponent<Image>().color.b, alpOfMissingCard);
            Color col = newCard.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color;
            newCard.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, alpOfMissingCard);
            col = newCard.transform.Find("CardNameText").GetComponent<Text>().color;
            newCard.transform.Find("CardNameText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, alpOfMissingCard);
            col = newCard.transform.Find("CardDescriptionText").GetComponent<Text>().color;
            newCard.transform.Find("CardDescriptionText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, alpOfMissingCard);
            col = newCard.transform.Find("ManaText").GetComponent<Text>().color;
            newCard.transform.Find("ManaText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, alpOfMissingCard);
            col = newCard.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>().color;
            newCard.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>().color = new Color(col.r, col.g, col.b, alpOfMissingCard);
        }
    }

    public void ClearCards()
    {
        for (int i = 0; i < GameObject.Find("DeckContent").transform.childCount; i++)
        {
            Destroy(GameObject.Find("DeckContent").transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < GameObject.Find("DeckCollContent").transform.childCount; i++)
        {
            Destroy(GameObject.Find("DeckCollContent").transform.GetChild(i).gameObject);
        }
        tempDeck.Clear();
        collection.Clear();
    }

    public void PutInDeck(int id)
    {
        //Láthatatan kártya létrehozása
        GameObject newCard = Instantiate(collCard);
        newCard.GetComponent<CollectionCardDisplay>().card = DBManager.allCards.Where(t => t.id == id).ToArray()[0];
        newCard.transform.SetParent(GameObject.Find("DeckContent").transform);
        newCard.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        float cardAlpha = 0;
        newCard.transform.Find("CardImage").GetComponent<Image>().color = new Color(newCard.transform.Find("CardImage").GetComponent<Image>().color.r
            , newCard.transform.Find("CardImage").GetComponent<Image>().color.g, newCard.transform.Find("CardImage").GetComponent<Image>().color.b, cardAlpha);
        newCard.transform.Find("CollCount").GetComponent<Image>().color = new Color(newCard.transform.Find("CollCount").GetComponent<Image>().color.r
            , newCard.transform.Find("CollCount").GetComponent<Image>().color.g, newCard.transform.Find("CollCount").GetComponent<Image>().color.b, cardAlpha);
        Color col = newCard.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color;
        newCard.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
        col = newCard.transform.Find("CardNameText").GetComponent<Text>().color;
        newCard.transform.Find("CardNameText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
        col = newCard.transform.Find("CardDescriptionText").GetComponent<Text>().color;
        newCard.transform.Find("CardDescriptionText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
        col = newCard.transform.Find("ManaText").GetComponent<Text>().color;
        newCard.transform.Find("ManaText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
        col = newCard.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>().color;
        newCard.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>().color = new Color(col.r, col.g, col.b, cardAlpha);
        col = newCard.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color;
        newCard.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
        newCard.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().text = "";
        GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().tempDeck.Add(new CollectionCards(DBManager.allCards.Where(t => t.id == id).ToArray()[0]));
        GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().tempDeck.Where(t => t.card.id == id).ToArray()[0].cnt = 1;

        int lastid = -1;
        int lastidIndex = -1;
        for (int i = 0; i < GameObject.Find("DeckContent").transform.childCount; i++)
        {
            if (GameObject.Find("DeckContent").transform.GetChild(i).gameObject.GetComponent<CollectionCardDisplay>().card.id < id)
            {
                lastid = GameObject.Find("DeckContent").transform.GetChild(i).gameObject.GetComponent<CollectionCardDisplay>().card.id;
                lastidIndex = i;
            }
        }

        if (lastid == -1 || lastidIndex == -1)
        {
            newCard.transform.SetAsFirstSibling();
        }
        else
        {
            newCard.transform.SetSiblingIndex(lastidIndex + 1);
        }

        Debug.Log("Láthatatlan kártyapos: " + newCard.transform.position);
    }
}

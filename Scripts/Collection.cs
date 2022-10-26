using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Collection : MonoBehaviour
{
    public Text enemyDefeatedCount;
    public Text currency;
    public Text usernameText;
    public GameObject container;
    public GameObject collCard;
    public bool inDeck;

    private void Start()
    {
        enemyDefeatedCount.text = DBManager.enemyDefeated.ToString();
        currency.text = DBManager.currency.ToString();
        usernameText.text = DBManager.username.ToString();
        container = GameObject.Find("Container");

        foreach (var item in DBManager.cardCollection.GroupBy(group => group).Select(group => new { card = group.Key, cnt = group.Count() }))
        {
            NewCard(item.card, item.cnt);
        }
        GameObject.Find("Scrollbar").GetComponent<Scrollbar>().value = 0;
    }

    private void Update()
    {
        if (inDeck)
        {
            GameObject.Find("CardsInDeck").GetComponent<Text>().text = $"Cards in deck: {GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().tempDeck.Sum(t => t.cnt)}/16";
            GameObject.Find("DeckPoints").GetComponent<Text>().text = $"Deck points: {CardWeights()}/{15 + DBManager.heroLevel}";
        }
    }

    public int CardWeights()
    {
        int weight = 0;
        string rarity;
        for (int i = 0; i < GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().tempDeck.Count; i++)
        {
            for (int j = 0; j < GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().tempDeck[i].cnt; j++)
            {
                rarity = GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().tempDeck[i].card.rarity;
                switch (rarity)
                {
                    case "Common":
                        weight += 1;
                        break;
                    case "Uncommon":
                        weight += 2;
                        break;
                    case "Rare":
                        weight += 3;
                        break;
                    case "Epic":
                        weight += 4;
                        break;
                    case "Legendary":
                        weight += 5;
                        break;
                    default:
                        break;
                }
            }

        }
        return weight;
    }

    public void MainMenuButtonClicked()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().goToMainMenu();
    }

    public void DeckButtonClicked()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        RectTransform trans = GameObject.Find("MyDeckPanel").GetComponent<RectTransform>();
        LeanTween.value(0, 1, 0.3f).setOnUpdate((float val) =>
        {
            trans.localScale = new Vector3(val, val, 1);
        }).setOnComplete(() => { GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().MyDeckStart(); inDeck = true;});
        

    }

    public void DeckBackButtonClicked()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        RectTransform trans = GameObject.Find("MyDeckPanel").GetComponent<RectTransform>();
        LeanTween.value(1, 0, 0.3f).setOnUpdate((float val) =>
        {
            trans.localScale = new Vector3(val, val, 1);
        }).setOnComplete(() => { GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().ClearCards(); inDeck = false;});
        

    }

    public void DeckSaveButtonClicked()
    {
        if (GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().tempDeck.Sum(t => t.cnt) == 16)
        {
            DBManager.deck.Clear();
            foreach (var item in GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().tempDeck)
            {
                for (int i = 0; i < item.cnt; i++)
                {
                    DBManager.deck.Add(item.card);
                }
            }
            DBManager.deck.OrderBy(t => t.id);
            DeckBackButtonClicked();
        }
        else if (GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().tempDeck.Sum(t => t.cnt) < 16)
        {

        }
    }

    public void NewCard(Card card, int cnt)
    {
        var newCard = Instantiate(collCard, container.transform.position, Quaternion.identity);
        newCard.GetComponent<CollectionCardDisplay>().card = card;
        newCard.transform.SetParent(container.transform);
        newCard.GetComponent<RectTransform>().localScale = new Vector3(1.9f, 1.9f, 1);

        Sprite[] sprites = Resources.LoadAll<Sprite>("Kartyak");
        Sprite[] cardCollSprites = Resources.LoadAll<Sprite>("CollCardCount");

        newCard.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().text = "x" + cnt;

        switch (card.rarity)
        {
            case "Common":
                newCard.transform.Find("CardImage").GetComponent<Image>().sprite = sprites[0];
                newCard.transform.Find("CollCount").GetComponent<Image>().sprite = cardCollSprites[0];
                break;
            case "Uncommon":
                newCard.transform.Find("CardImage").GetComponent<Image>().sprite = sprites[1];
                newCard.transform.Find("CollCount").GetComponent<Image>().sprite = cardCollSprites[1];
                break;
            case "Rare":
                newCard.transform.Find("CardImage").GetComponent<Image>().sprite = sprites[2];
                newCard.transform.Find("CollCount").GetComponent<Image>().sprite = cardCollSprites[2];
                break;
            case "Epic":
                newCard.transform.Find("CardImage").GetComponent<Image>().sprite = sprites[3];
                newCard.transform.Find("CollCount").GetComponent<Image>().sprite = cardCollSprites[3];
                break;
            case "Legendary":
                newCard.transform.Find("CardImage").GetComponent<Image>().sprite = sprites[4];
                newCard.transform.Find("CollCount").GetComponent<Image>().sprite = cardCollSprites[4];
                break;
            default:
                newCard.transform.Find("CardImage").GetComponent<Image>().sprite = sprites[0];
                newCard.transform.Find("CollCount").GetComponent<Image>().sprite = cardCollSprites[0];
                Debug.Log("!!CARD RARITY ERROR!!");
                break;
        }
    }
}

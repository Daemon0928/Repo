using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDraw : MonoBehaviour
{
    public List<Card> deck;
    public List<Card> shuffledDeck;
    public GameObject canvas;
    public GameObject card;
    public bool firstShuffle = true;
    public bool shuffleNeeded = false;
    public ManaManager m;
    public float time;
    public GameManager gameManager;

    public void Awake()
    {
        m = GameObject.Find("Mana").GetComponent<ManaManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        canvas = GameObject.Find("Canvas");
        deck = DBManager.deck;
        time = 2f;
        Debug.Log("Deck Count: " + deck.Count);
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            OnDraw();
            time = 2f;
        }
    }

    public void OnDraw()
    {
        if (firstShuffle)
        {
            ShuffleDeck();
            firstShuffle = false;
        }
        else if (shuffleNeeded)
        {
            StartCoroutine(Shuffle());
        }
        if (shuffledDeck.Count > 0)
        {
            Draw();
        }
    }

    public void Draw()
    {
        int indexOfCard = UnityEngine.Random.Range(0, shuffledDeck.Count);
        var newCard = Instantiate(card, this.transform.position, quaternion.identity);
        newCard.GetComponent<CardDisplay>().card = shuffledDeck[indexOfCard];
        newCard.GetComponent<CardPlay>().card = shuffledDeck[indexOfCard];
        newCard.transform.SetParent(canvas.transform);

        Sprite[] sprites = Resources.LoadAll<Sprite>("Kartyak");

        switch (newCard.GetComponent<CardDisplay>().card.rarity)
        {
            case "Common":
                newCard.transform.Find("CardImage").GetComponent<Image>().sprite = sprites[0];
                break;
            case "Uncommon":
                newCard.transform.Find("CardImage").GetComponent<Image>().sprite = sprites[1];
                break;
            case "Rare":
                newCard.transform.Find("CardImage").GetComponent<Image>().sprite = sprites[2];
                break;
            case "Epic":
                newCard.transform.Find("CardImage").GetComponent<Image>().sprite = sprites[3];
                break;
            case "Legendary":
                newCard.transform.Find("CardImage").GetComponent<Image>().sprite = sprites[4];
                break;
            default:
                newCard.transform.Find("CardImage").GetComponent<Image>().sprite = sprites[0];
                Debug.Log("!!CARD RARITY ERROR!!");
                break;
        }

        if (canvas.transform.Find("StunnedPanel"))
        {
            canvas.transform.Find("StunnedPanel").SetAsLastSibling();
        }
        newCard.transform.localScale = new Vector3(1.5f, 1.5f, 1);

        shuffledDeck.RemoveAt(indexOfCard);
        if (shuffledDeck.Count == 0)
        {
            Debug.Log("Shuffle was needed");
            shuffleNeeded = true;
        }
        Debug.Log("Cards in deck: " + shuffledDeck.Count);
    }

    public void ShuffleDeck()
    {
        List<Card> deckCopy = new List<Card>();
        deckCopy.Clear();
        foreach (var item in deck)
        {
            deckCopy.Add(Instantiate(item));
        }
        int randomIndex;
        for (int i = 0; i < deck.Count; i++)
        {
            randomIndex = UnityEngine.Random.Range((int)0, (int)deckCopy.Count - 1);
            shuffledDeck.Add(deckCopy[randomIndex]);
            deckCopy.RemoveAt(randomIndex);
        }
    }

    IEnumerator Shuffle()
    {
        gameManager.ShowText("Shuffling deck...");
        shuffleNeeded = false;
        yield return new WaitForSeconds(4f);
        ShuffleDeck();
        gameManager.ShowText("Deck Shuffled");
        Debug.Log($"Deck shuffled with {shuffledDeck.Count} card");
    }
}

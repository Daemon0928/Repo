using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class CollectionCard : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        StartCoroutine(Click());
    }

    public IEnumerator Click()
    {
        var tempDeck = GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().tempDeck;
        var collection = GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().collection;
        if (this.transform.parent.gameObject == GameObject.Find("DeckCollContent")
            && collection.Where(t => t.card.id == this.GetComponent<CollectionCardDisplay>().card.id).First().cnt != 0)
        {
            if (GameObject.Find("Canvas").GetComponent<Collection>().CardWeights() <= DBManager.heroLevel + 15)
            {
                //Ha a deck tele van
                if (tempDeck.Sum(t => t.cnt) >= 16)
                {
                    Debug.Log("Your deck is full");
                }
                //Ha van már ilyen kártya a deckben
                else if (tempDeck.Where(t => t.card.id == this.gameObject.GetComponent<CollectionCardDisplay>().card.id).ToArray().Length > 0)
                {
                    for (int i = 0; i < GameObject.Find("DeckContent").transform.childCount; i++)
                    {
                        if (this.gameObject.GetComponent<CollectionCardDisplay>().card.id == GameObject.Find("DeckContent").transform.GetChild(i).gameObject.GetComponent<CollectionCardDisplay>().card.id)
                        {
                            tempDeck.Where(t => t.card.id == this.gameObject.GetComponent<CollectionCardDisplay>().card.id).ToArray()[0].cnt++;
                            collection.Where(t => t.card.id == this.gameObject.GetComponent<CollectionCardDisplay>().card.id).ToArray()[0].cnt--;
                            MoveToDeck(GameObject.Find("DeckContent").transform.GetChild(i).gameObject);
                        }
                    }
                }
                //Ha még nincs ilyen kártya a deckben
                else
                {
                    GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().PutInDeck(this.GetComponent<CollectionCardDisplay>().card.id);
                    collection.Where(t => t.card.id == this.gameObject.GetComponent<CollectionCardDisplay>().card.id).ToArray()[0].cnt--;
                    yield return new WaitForSeconds(0.0001f);
                    for (int i = 0; i < GameObject.Find("DeckContent").transform.childCount; i++)
                    {
                        if (this.gameObject.GetComponent<CollectionCardDisplay>().card.id == GameObject.Find("DeckContent").transform.GetChild(i).gameObject.GetComponent<CollectionCardDisplay>().card.id)
                        {
                            MoveToDeckFirst(GameObject.Find("DeckContent").transform.GetChild(i).gameObject);
                        }
                    }

                    for (int i = 0; i < GameObject.Find("DeckContent").transform.childCount; i++)
                    {
                        Debug.Log($"{GameObject.Find("DeckContent").transform.GetChild(i).GetComponent<CollectionCardDisplay>().card.name} {GameObject.Find("DeckContent").transform.GetChild(i).gameObject.transform.position}");
                    }
                }
            }
            else
            {
                Debug.Log("You don't have enough deck points to fit that card into your deck!");
            }
        }
        else if (this.transform.parent.gameObject == GameObject.Find("DeckContent"))
        {
            //Ha több mint egy kártya van a deckben, de csak egynek kell visszamenni
            if (tempDeck.Where(t => t.card.id == this.GetComponent<CollectionCardDisplay>().card.id).First().cnt > 1)
            {
                tempDeck.Where(t => t.card.id == this.GetComponent<CollectionCardDisplay>().card.id).First().cnt--;
                collection.Where(t => t.card.id == this.GetComponent<CollectionCardDisplay>().card.id).First().cnt++;
                for (int i = 0; i < GameObject.Find("DeckCollContent").transform.childCount; i++)
                {
                    if (this.gameObject.GetComponent<CollectionCardDisplay>().card.id == GameObject.Find("DeckCollContent").transform.GetChild(i).gameObject.GetComponent<CollectionCardDisplay>().card.id)
                    {
                        MoveCopyTo(GameObject.Find("DeckCollContent").transform.GetChild(i).gameObject);
                    }
                }
            }
            //Ha csak egy ilyen kártya van a deckbe, amit vissza kell küldeni
            else if (tempDeck.Where(t => t.card.id == this.GetComponent<CollectionCardDisplay>().card.id).First().cnt == 1)
            {
                for (int i = 0; i < GameObject.Find("DeckCollContent").transform.childCount; i++)
                {
                    if (this.gameObject.GetComponent<CollectionCardDisplay>().card.id == GameObject.Find("DeckCollContent").transform.GetChild(i).gameObject.GetComponent<CollectionCardDisplay>().card.id)
                    {
                        collection.Where(t => t.card.id == this.gameObject.GetComponent<CollectionCardDisplay>().card.id).ToArray()[0].cnt++;
                        MoveTo(GameObject.Find("DeckCollContent").transform.GetChild(i).gameObject);
                        tempDeck.Remove(tempDeck.Where(t => t.card.id == this.gameObject.GetComponent<CollectionCardDisplay>().card.id).ToArray()[0]);
                    }
                }
            }

        }
    }

    public void MoveTo(GameObject target)
    {
        Vector3 wspacePos = this.gameObject.transform.position;
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
        this.gameObject.transform.position = wspacePos;
        LeanTween.move(this.gameObject, target.transform.position, 0.2f).setOnComplete(() => { target.GetComponent<CollectionCard>().RefreshThis(); this.gameObject.transform.SetParent(target.transform.parent); Destroy(this.gameObject); });
    }

    public void MoveCopyTo(GameObject target)
    {
        RefreshThis();
        GameObject goCopy = Instantiate(this.gameObject);
        goCopy.transform.SetParent(GameObject.Find("DeckCollContent").transform);
        goCopy.transform.position = this.transform.position;
        goCopy.transform.localScale = this.transform.localScale;
        Vector3 wspacePos = goCopy.transform.position;
        goCopy.transform.SetParent(GameObject.Find("Canvas").transform);
        goCopy.transform.position = wspacePos;
        LeanTween.move(goCopy, target.transform.position, 0.2f).setOnComplete(() => { target.GetComponent<CollectionCard>().RefreshThis(); goCopy.transform.SetParent(target.transform.parent); Destroy(goCopy.gameObject); });
    }

    public void MoveToDeck(GameObject target)
    {
        GameObject goCopy = Instantiate(this.gameObject);
        RefreshThis();
        goCopy.transform.SetParent(GameObject.Find("DeckCollContent").transform);
        goCopy.transform.position = this.transform.position;
        goCopy.transform.localScale = this.transform.localScale;
        Vector3 wspacePos = goCopy.transform.position;
        goCopy.transform.SetParent(GameObject.Find("Canvas").transform);
        goCopy.transform.position = wspacePos;
        Debug.Log(target.transform.position);
        LeanTween.move(goCopy, target.transform.position, 0.2f).setOnComplete(() =>
        {
            target.GetComponent<CollectionCard>().RefreshThis();
            goCopy.transform.SetParent(target.transform.parent);
            Destroy(goCopy.gameObject);
        });
    }

    public void MoveToDeckFirst(GameObject target)
    {
        GameObject goCopy = Instantiate(this.gameObject);
        RefreshThis();
        goCopy.transform.SetParent(GameObject.Find("DeckCollContent").transform);
        goCopy.transform.position = this.transform.position;
        goCopy.transform.localScale = this.transform.localScale;
        Vector3 wspacePos = goCopy.transform.position;
        goCopy.transform.SetParent(GameObject.Find("Canvas").transform);
        goCopy.transform.position = wspacePos;
        Debug.Log(target.transform.position);
        LeanTween.move(goCopy, target.transform.position, 0.2f).setOnComplete(() =>
        {
            target.GetComponent<CollectionCard>().RefreshThis();
            goCopy.transform.SetParent(target.transform.parent);
            Destroy(goCopy.gameObject);
            target.GetComponent<CollectionCard>().Pikaboo();
        });
    }

    public void RefreshThis()
    {
        float cardAlpha;
        if (this.transform.parent.gameObject == GameObject.Find("DeckContent"))
        {
            this.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().text = $"x{GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().tempDeck.Where(t => t.card.id == this.GetComponent<CollectionCardDisplay>().card.id).ToArray()[0].cnt}";
        }
        else if (this.transform.parent.gameObject == GameObject.Find("DeckCollContent"))
        {
            if (GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().collection.Where(t => t.card.id == this.GetComponent<CollectionCardDisplay>().card.id).ToArray()[0].cnt == 0)
            {
                cardAlpha = Instantiate(GameObject.Find("MyDeckPanel")).GetComponent<MyDeck>().alpOfMissingCard;
                this.transform.Find("CardImage").GetComponent<Image>().color = new Color(this.transform.Find("CardImage").GetComponent<Image>().color.r
                    , this.transform.Find("CardImage").GetComponent<Image>().color.g, this.transform.Find("CardImage").GetComponent<Image>().color.b, cardAlpha);
                this.transform.Find("CollCount").GetComponent<Image>().color = new Color(this.transform.Find("CollCount").GetComponent<Image>().color.r
                    , this.transform.Find("CollCount").GetComponent<Image>().color.g, this.transform.Find("CollCount").GetComponent<Image>().color.b, cardAlpha);
                Color col = this.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color;
                this.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
                col = this.transform.Find("CardNameText").GetComponent<Text>().color;
                this.transform.Find("CardNameText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
                col = this.transform.Find("CardDescriptionText").GetComponent<Text>().color;
                this.transform.Find("CardDescriptionText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
                col = this.transform.Find("ManaText").GetComponent<Text>().color;
                this.transform.Find("ManaText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
                col = this.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>().color;
                this.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>().color = new Color(col.r, col.g, col.b, cardAlpha);
                this.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().text = "x0";
                Debug.Log("0: " + GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().collection.Where(t => t.card.id == this.GetComponent<CollectionCardDisplay>().card.id).ToArray()[0].cnt);
            }
            else if (GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().collection.Where(t => t.card.id == this.GetComponent<CollectionCardDisplay>().card.id).ToArray()[0].cnt > 0)
            {
                cardAlpha = 1;
                this.transform.Find("CardImage").GetComponent<Image>().color = new Color(this.transform.Find("CardImage").GetComponent<Image>().color.r
                    , this.transform.Find("CardImage").GetComponent<Image>().color.g, this.transform.Find("CardImage").GetComponent<Image>().color.b, cardAlpha);
                this.transform.Find("CollCount").GetComponent<Image>().color = new Color(this.transform.Find("CollCount").GetComponent<Image>().color.r
                    , this.transform.Find("CollCount").GetComponent<Image>().color.g, this.transform.Find("CollCount").GetComponent<Image>().color.b, cardAlpha);
                Color col = this.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color;
                this.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
                col = this.transform.Find("CardNameText").GetComponent<Text>().color;
                this.transform.Find("CardNameText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
                col = this.transform.Find("CardDescriptionText").GetComponent<Text>().color;
                this.transform.Find("CardDescriptionText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
                col = this.transform.Find("ManaText").GetComponent<Text>().color;
                this.transform.Find("ManaText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
                col = this.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>().color;
                this.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>().color = new Color(col.r, col.g, col.b, cardAlpha);
                this.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().text = $"x{GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().collection.Where(t => t.card.id == this.GetComponent<CollectionCardDisplay>().card.id).ToArray()[0].cnt}";
                Debug.Log("Nagyobb mint 0: " + GameObject.Find("MyDeckPanel").GetComponent<MyDeck>().collection.Where(t => t.card.id == this.GetComponent<CollectionCardDisplay>().card.id).ToArray()[0].cnt);
            }
        }
    }

    public void Pikaboo()
    {
        float cardAlpha = 1;
        this.transform.Find("CardImage").GetComponent<Image>().color = new Color(this.transform.Find("CardImage").GetComponent<Image>().color.r
            , this.transform.Find("CardImage").GetComponent<Image>().color.g, this.transform.Find("CardImage").GetComponent<Image>().color.b, cardAlpha);
        this.transform.Find("CollCount").GetComponent<Image>().color = new Color(this.transform.Find("CollCount").GetComponent<Image>().color.r
            , this.transform.Find("CollCount").GetComponent<Image>().color.g, this.transform.Find("CollCount").GetComponent<Image>().color.b, cardAlpha);
        Color col = this.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color;
        this.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
        col = this.transform.Find("CardNameText").GetComponent<Text>().color;
        this.transform.Find("CardNameText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
        col = this.transform.Find("CardDescriptionText").GetComponent<Text>().color;
        this.transform.Find("CardDescriptionText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
        col = this.transform.Find("ManaText").GetComponent<Text>().color;
        this.transform.Find("ManaText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
        col = this.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>().color;
        this.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>().color = new Color(col.r, col.g, col.b, cardAlpha);
        col = this.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color;
        this.transform.Find("CollCount").transform.Find("CollCountText").GetComponent<Text>().color = new Color(col.r, col.g, col.b, cardAlpha);
    }
}

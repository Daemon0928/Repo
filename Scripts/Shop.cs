using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Shop : MonoBehaviour
{
    public GameObject PackPanel;
    public GameObject ClickAnywhereText;
    public List<GameObject> CardsInPack;
    public GameObject CollCard;
    public GameObject NotEnoughGold;
    public GameObject WhitePanel;
    public Text CurrencyCount;
    private bool packOpened = false;
    private bool animComplete = false;

    private void Start()
    {
        CurrencyCount.text = DBManager.currency.ToString();
    }

    IEnumerator OpenPack(string rarity)
    {
        List<Card> cards = new List<Card>();
        int r;

        Sprite[] packSprites = Resources.LoadAll<Sprite>("Cardpacks");

        if (rarity == "Common")
        {
            r = Random.Range(1, 1000);
            //Első kártya (garantált)
            if (r <= 2)
            {
                cards.Add(DBManager.allCards.Where(t => t.rarity == "Legendary").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Legendary").Count() - 1)]);
            }
            else if (r <= 10)
            {
                cards.Add(DBManager.allCards.Where(t => t.rarity == "Epic").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Epic").Count() - 1)]);
            }
            else if (r <= 50)
            {
                cards.Add(DBManager.allCards.Where(t => t.rarity == "Rare").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Rare").Count() - 1)]);
            }
            else
            {
                cards.Add(DBManager.allCards.Where(t => t.rarity == "Uncommon").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Uncommon").Count() - 1)]);
            }
            //Többi kártya
            for (int i = 0; i < 4; i++)
            {
                r = Random.Range(1, 1000);
                if (r <= 2)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Legendary").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Legendary").Count() - 1)]);
                }
                else if (r <= 10)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Epic").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Epic").Count() - 1)]);
                }
                else if (r <= 30)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Rare").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Rare").Count() - 1)]);
                }
                else if (r <= 50)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Uncommon").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Uncommon").Count() - 1)]);
                }
                else
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Common").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Common").Count() - 1)]);
                }
            }

            for (int i = 0; i < CardsInPack.Count; i++)
            {
                CardsInPack[i].GetComponent<Image>().sprite = packSprites[0];
            }
        }
        else if (rarity == "Uncommon")
        {
            //Első két kártya (garantált)
            for (int i = 0; i < 2; i++)
            {
                r = Random.Range(1, 1000);
                if (r <= 20)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Legendary").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Legendary").Count() - 1)]);
                }
                else if (r <= 50)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Epic").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Epic").Count() - 1)]);
                }
                else if (r <= 120)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Rare").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Rare").Count() - 1)]);
                }
                else
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Uncommon").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Uncommon").Count() - 1)]);
                }
            }
            //Többi kártya
            for (int i = 0; i < 3; i++)
            {
                r = Random.Range(1, 1000);
                if (r <= 10)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Legendary").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Legendary").Count() - 1)]);
                }
                else if (r <= 30)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Epic").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Epic").Count() - 1)]);
                }
                else if (r <= 60)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Rare").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Rare").Count() - 1)]);
                }
                else if (r <= 150)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Uncommon").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Uncommon").Count() - 1)]);
                }
                else
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Common").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Common").Count() - 1)]);
                }
            }

            for (int i = 0; i < CardsInPack.Count; i++)
            {
                CardsInPack[i].GetComponent<Image>().sprite = packSprites[1];
            }
        }
        else if (rarity == "Rare")
        {
            r = Random.Range(1, 1000);
            //Első kártya (garantált)
            if (r <= 50)
            {
                cards.Add(DBManager.allCards.Where(t => t.rarity == "Legendary").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Legendary").Count() - 1)]);
            }
            else if (r <= 130)
            {
                cards.Add(DBManager.allCards.Where(t => t.rarity == "Epic").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Epic").Count() - 1)]);
            }
            else
            {
                cards.Add(DBManager.allCards.Where(t => t.rarity == "Rare").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Rare").Count() - 1)]);
            }
            //Többi kártya
            for (int i = 0; i < 4; i++)
            {
                r = Random.Range(1, 1000);
                if (r <= 30)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Legendary").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Legendary").Count() - 1)]);
                }
                else if (r <= 100)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Epic").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Epic").Count() - 1)]);
                }
                else if (r <= 200)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Rare").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Rare").Count() - 1)]);
                }
                else if (r <= 400)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Uncommon").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Uncommon").Count() - 1)]);
                }
                else
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Common").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Common").Count() - 1)]);
                }
            }

            for (int i = 0; i < CardsInPack.Count; i++)
            {
                CardsInPack[i].GetComponent<Image>().sprite = packSprites[2];
            }
        }
        else if (rarity == "Epic")
        {
            r = Random.Range(1, 1000);
            //Első kártya (garantált)
            if (r <= 120)
            {
                cards.Add(DBManager.allCards.Where(t => t.rarity == "Legendary").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Legendary").Count() - 1)]);
            }
            else
            {
                cards.Add(DBManager.allCards.Where(t => t.rarity == "Epic").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Epic").Count() - 1)]);
            }
            //Többi kártya
            for (int i = 0; i < 4; i++)
            {
                r = Random.Range(1, 1000);
                if (r <= 100)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Legendary").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Legendary").Count() - 1)]);
                }
                else if (r <= 200)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Epic").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Epic").Count() - 1)]);
                }
                else if (r <= 400)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Rare").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Rare").Count() - 1)]);
                }
                else if (r <= 800)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Uncommon").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Uncommon").Count() - 1)]);
                }
                else
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Common").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Common").Count() - 1)]);
                }
            }

            for (int i = 0; i < CardsInPack.Count; i++)
            {
                CardsInPack[i].GetComponent<Image>().sprite = packSprites[3];
            }
        }
        else if (rarity == "Legendary")
        {
            r = Random.Range(1, 1000);

            //Első kártya (garantált)
            cards.Add(DBManager.allCards.Where(t => t.rarity == "Legendary").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Legendary").Count() - 1)]);

            //Többi kártya
            for (int i = 0; i < 4; i++)
            {
                r = Random.Range(1, 1000);
                if (r <= 200)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Legendary").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Legendary").Count() - 1)]);
                }
                else if (r <= 400)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Epic").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Epic").Count() - 1)]);
                }
                else if (r <= 700)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Rare").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Rare").Count() - 1)]);
                }
                else if (r <= 900)
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Uncommon").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Uncommon").Count() - 1)]);
                }
                else
                {
                    cards.Add(DBManager.allCards.Where(t => t.rarity == "Common").ToArray()[Random.Range(0, DBManager.allCards.Where(t => t.rarity == "Common").Count() - 1)]);
                }
            }

            for (int i = 0; i < CardsInPack.Count; i++)
            {
                CardsInPack[i].GetComponent<Image>().sprite = packSprites[4];
            }
        }

        cards = ShuffleCards(cards);

        GameObject ObjToMove;

        if (rarity == "Common")
        {
            ObjToMove = GameObject.Find("CommonPack");
        }
        else if (rarity == "Uncommon")
        {
            ObjToMove = GameObject.Find("UncommonPack");
        }
        else if (rarity == "Rare")
        {
            ObjToMove = GameObject.Find("RarePack");
        }
        else if (rarity == "Epic")
        {
            ObjToMove = GameObject.Find("EpicPack");
        }
        else if (rarity == "Legendary")
        {
            ObjToMove = GameObject.Find("LegendaryPack");
        }
        else
        {
            ObjToMove = GameObject.Find("CommonPack");
        }

        Color col;
        GameObject tempChild;
        for (int i = 0; i < ObjToMove.transform.childCount; i++)
        {
            tempChild = ObjToMove.transform.GetChild(i).gameObject;
            tempChild.transform.SetParent(GameObject.Find("Canvas").transform);
            tempChild.transform.SetAsFirstSibling();
        }
        for (int i = 0; i < ObjToMove.transform.childCount; i++)
        {
            tempChild = ObjToMove.transform.GetChild(i).gameObject;
            tempChild.transform.SetParent(GameObject.Find("Canvas").transform);
            tempChild.transform.SetAsFirstSibling();
        }

        LeanTween.move(ObjToMove, GameObject.Find("Canvas").transform.position, 0.8f).setOnStart(() =>
        {
            WhitePanel.transform.SetAsLastSibling();
            ObjToMove.transform.SetAsLastSibling();
            WhitePanel.SetActive(true);
            col = WhitePanel.GetComponent<Image>().color;
            LeanTween.value(0, 1, 1f).setOnUpdate((float val) =>
            {
                WhitePanel.GetComponent<Image>().color = new Color(col.r, col.g, col.b, val);
            }).setOnComplete(() =>
            {
                col = ObjToMove.GetComponent<Image>().color;
                LeanTween.value(1, 0, 0.7f).setOnUpdate((float val) =>
                {
                    ObjToMove.GetComponent<Image>().color = new Color(col.r, col.g, col.b, val);
                }).setOnComplete(() =>
                {
                    Destroy(ObjToMove);
                    PackPanel.SetActive(true);
                    col = WhitePanel.GetComponent<Image>().color;
                    LeanTween.value(1, 0, 0.7f).setOnUpdate((float val) =>
                    {
                        WhitePanel.GetComponent<Image>().color = new Color(col.r, col.g, col.b, val);
                    }).setOnComplete(() =>
                    {
                        animComplete = true;
                    });
                });
            });
        });

        while (animComplete == false)
        {
            yield return new WaitForEndOfFrame();
        }

        WhitePanel.SetActive(false);

        yield return new WaitForSeconds(1f);
        for (int i = 0; i < CardsInPack.Count; i++)
        {
            GameObject collCard = Instantiate(CollCard);
            collCard.GetComponent<PackCardDisplay>().card = cards[i];
            DBManager.cardCollection.Add(cards[i]);
            collCard.transform.SetParent(PackPanel.transform);
            collCard.transform.position = CardsInPack[i].transform.position;
            collCard.transform.localScale = CardsInPack[i].transform.localScale;
            Debug.Log(i + ". is finished");
            yield return new WaitForSeconds(1f);
        }
        col = ClickAnywhereText.GetComponent<Text>().color;
        ClickAnywhereText.GetComponent<Text>().color = new Color(col.r, col.g, col.b, 0);
        ClickAnywhereText.SetActive(true);
        packOpened = true;
        LeanTween.value(0, 1, 1).setOnUpdate((float val) =>
        {
            ClickAnywhereText.GetComponent<Text>().color = new Color(col.r, col.g, col.b, val);
        });
    }

    public List<Card> ShuffleCards(List<Card> cards)
    {
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n - 1);
            Card value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
        return cards;
    }

    public void CommonButtonPressed()
    {
        if (DBManager.currency >= 750)
        {
            DBManager.currency -= 750;
            StartCoroutine(OpenPack("Common"));
        }
        else
        {
            Color col = NotEnoughGold.GetComponent<Text>().color;
            NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, 0);
            NotEnoughGold.SetActive(true);
            LeanTween.value(0, 1, 1).setOnUpdate((float val) =>
            {
                NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, val);
            }).setOnComplete(() =>
            {
                LeanTween.value(1, 0, 1).setOnUpdate((float val) =>
                {
                    NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, val);
                }).setOnComplete(() => { NotEnoughGold.SetActive(false); });
            });
        }
    }

    public void OpenedPackClicked()
    {
        if (packOpened)
        {
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().goToShop();
        }
    }

    public void UncommonButtonPressed()
    {
        if (DBManager.currency >= 1200)
        {
            DBManager.currency -= 1200;
            StartCoroutine(OpenPack("Uncommon"));
        }
        else
        {
            Color col = NotEnoughGold.GetComponent<Text>().color;
            NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, 0);
            NotEnoughGold.SetActive(true);
            LeanTween.value(0, 1, 1).setOnUpdate((float val) =>
            {
                NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, val);
            }).setOnComplete(() =>
            {
                LeanTween.value(1, 0, 1).setOnUpdate((float val) =>
                {
                    NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, val);
                }).setOnComplete(() => { NotEnoughGold.SetActive(false); });
            });
        }
    }

    public void RareButtonPressed()
    {
        if (DBManager.currency >= 2000)
        {
            DBManager.currency -= 2000;
            StartCoroutine(OpenPack("Rare"));
        }
        else
        {
            Color col = NotEnoughGold.GetComponent<Text>().color;
            NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, 0);
            NotEnoughGold.SetActive(true);
            LeanTween.value(0, 1, 1).setOnUpdate((float val) =>
            {
                NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, val);
            }).setOnComplete(()=> 
            {
                LeanTween.value(1, 0, 1).setOnUpdate((float val) =>
                {
                    NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, val);
                }).setOnComplete(() => { NotEnoughGold.SetActive(false); });
            });
        }
    }

    public void EpicButtonPressed()
    {
        if (DBManager.currency >= 3500)
        {
            DBManager.currency -= 3500;
            StartCoroutine(OpenPack("Epic"));
        }
        else
        {
            Color col = NotEnoughGold.GetComponent<Text>().color;
            NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, 0);
            NotEnoughGold.SetActive(true);
            LeanTween.value(0, 1, 1).setOnUpdate((float val) =>
            {
                NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, val);
            }).setOnComplete(() =>
            {
                LeanTween.value(1, 0, 1).setOnUpdate((float val) =>
                {
                    NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, val);
                }).setOnComplete(() => { NotEnoughGold.SetActive(false); });
            });
        }
    }

    public void LegendaryButtonPressed()
    {
        if (DBManager.currency >= 5000)
        {
            DBManager.currency -= 5000;
            StartCoroutine(OpenPack("Legendary"));
        }
        else
        {
            Color col = NotEnoughGold.GetComponent<Text>().color;
            NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, 0);
            NotEnoughGold.SetActive(true);
            LeanTween.value(0, 1, 1).setOnUpdate((float val) =>
            {
                NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, val);
            }).setOnComplete(() =>
            {
                LeanTween.value(1, 0, 1).setOnUpdate((float val) =>
                {
                    NotEnoughGold.GetComponent<Text>().color = new Color(col.r, col.g, col.b, val);
                }).setOnComplete(() => { NotEnoughGold.SetActive(false); });
            });
        }
    }

    public void BackButtonPressed()
    {
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().goToMainMenu();
    }

    public void CollectionButtonPressed()
    {
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().goToCollection();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackCardDisplay : MonoBehaviour
{
    public Card card;

    public Text nameText;
    public Text manaCostText;
    public Text description;
    public Image artwork;

    void Start()
    {
        nameText.text = card.cardName;
        manaCostText.text = card.cost.ToString();
        description.text = card.description;
        artwork.sprite = card.artwork;

        Sprite[] sprites = Resources.LoadAll<Sprite>("Kartyak");

        switch (card.rarity)
        {
            case "Common":
                transform.Find("CardImage").GetComponent<Image>().sprite = sprites[0];
                break;
            case "Uncommon":
                transform.Find("CardImage").GetComponent<Image>().sprite = sprites[1];
                break;
            case "Rare":
                transform.Find("CardImage").GetComponent<Image>().sprite = sprites[2];
                break;
            case "Epic":
                transform.Find("CardImage").GetComponent<Image>().sprite = sprites[3];
                break;
            case "Legendary":
                transform.Find("CardImage").GetComponent<Image>().sprite = sprites[4];
                break;
            default:
                transform.Find("CardImage").GetComponent<Image>().sprite = sprites[1];
                Debug.Log("!!CARD RARITY ERROR!!");
                break;
        }
    }
}

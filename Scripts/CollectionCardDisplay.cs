using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionCardDisplay : MonoBehaviour
{
    public Card card;

    public Text nameText;
    public Text manaCostText;
    public Text description;
    public Text collCountText;
    public Image artwork;

    void Start()
    {
        nameText.text = card.cardName;
        manaCostText.text = card.cost.ToString();
        description.text = card.description;
        artwork.sprite = card.artwork;

        Sprite[] sprites = Resources.LoadAll<Sprite>("Kartyak");
        Sprite[] cardCollSprites = Resources.LoadAll<Sprite>("CollCardCount");

        switch (card.rarity)
        {
            case "Common":
                collCountText.color = new Color32(64, 64, 64, 255);
                transform.Find("CardImage").GetComponent<Image>().sprite = sprites[0];
                transform.Find("CollCount").GetComponent<Image>().sprite = cardCollSprites[0];
                break;
            case "Uncommon":
                collCountText.color = new Color32(9, 57, 22, 255);
                transform.Find("CardImage").GetComponent<Image>().sprite = sprites[1];
                transform.Find("CollCount").GetComponent<Image>().sprite = cardCollSprites[1];
                break;
            case "Rare":
                collCountText.color = new Color32(22, 27, 80, 255);
                transform.Find("CardImage").GetComponent<Image>().sprite = sprites[2];
                transform.Find("CollCount").GetComponent<Image>().sprite = cardCollSprites[2];
                break;
            case "Epic":
                collCountText.color = new Color32(62, 10, 71, 255);
                transform.Find("CardImage").GetComponent<Image>().sprite = sprites[3];
                transform.Find("CollCount").GetComponent<Image>().sprite = cardCollSprites[3];
                break;
            case "Legendary":
                collCountText.color = new Color32(85, 66, 0, 255);
                transform.Find("CardImage").GetComponent<Image>().sprite = sprites[4];
                transform.Find("CollCount").GetComponent<Image>().sprite = cardCollSprites[4];
                break;
            default:
                collCountText.color = new Color32(64, 64, 64, 255);
                transform.Find("CardImage").GetComponent<Image>().sprite = sprites[1];
                transform.Find("CollCount").GetComponent<Image>().sprite = cardCollSprites[1];
                Debug.Log("!!CARD RARITY ERROR!!");
                break;
        }
    }
}

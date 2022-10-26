using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPlay : MonoBehaviour
{
    public Card card;
    public ManaManager m;
    public Material DissolveMat;
    public Material DissolveArtworkMat;
    public Material DissolveManaMat;
    private GameManager gameManager;
    public bool stolen = false;

    void OnMouseDown()
    {
        this.gameObject.tag = "PlayedCard";
        m = GameObject.Find("Mana").GetComponent<ManaManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager.isStunned == false)
        {
            if (m.enoughMana(card.cost))
            {
                this.gameObject.transform.Find("CardImage").GetComponent<Image>().material = Instantiate<Material>(this.gameObject.transform.Find("CardImage").GetComponent<Image>().material);
                DissolveMat = this.gameObject.transform.Find("CardImage").GetComponent<Image>().material;
                this.gameObject.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>().material = Instantiate<Material>(this.gameObject.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>().material);
                DissolveArtworkMat = this.gameObject.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>().material;

                EffectHandler e = new EffectHandler();

                e.GetEffect(card);
                m.manaSpent(card.cost);

                LeanTween.move(this.gameObject, new Vector3(0,
                    1.5f, 0), 0.4f).setOnComplete(() =>
                    {
                        Text descT = this.gameObject.transform.Find("CardDescriptionText").GetComponent<Text>();
                        Text manaT = this.gameObject.transform.Find("ManaText").GetComponent<Text>();
                        Text cnameT = this.gameObject.transform.Find("CardNameText").GetComponent<Text>();
                        LeanTween.value(this.gameObject, 0, 1, 0.8f).setOnUpdate((float val) =>
                        {
                            descT.color = new Color(descT.color.r, descT.color.g, descT.color.b, 1 - val);
                            manaT.color = new Color(manaT.color.r, manaT.color.g, manaT.color.b, 1 - val);
                            cnameT.color = new Color(cnameT.color.r, cnameT.color.g, cnameT.color.b, 1 - val);
                            DissolveMat.SetFloat("_Amount", val);
                            DissolveArtworkMat.SetFloat("_Amount", val);
                        }).setOnComplete(() =>
                        {
                            Destroy(this.gameObject);
                        });
                    });
            }
            else
            {
                gameManager.ShowText($"Not enough mana({card.cost})");
            }
        }
        else
        {
            gameManager.ShowText("You are stunned!");
        }
    }

    public void CardStolen()
    {
        //this.gameObject.GetComponent<CardDisplay>().enabled = false;
        stolen = true;
        this.gameObject.GetComponent<CanvasGroup>().interactable = false;
        Image cImg = this.gameObject.transform.Find("CardImage").GetComponent<Image>();
        Image aImg = this.gameObject.transform.Find("CardImage").transform.Find("Artwork").GetComponent<Image>();
        Text descT = this.gameObject.transform.Find("CardDescriptionText").GetComponent<Text>();
        Text manaT = this.gameObject.transform.Find("ManaText").GetComponent<Text>();
        Text cnameT = this.gameObject.transform.Find("CardNameText").GetComponent<Text>();

        LeanTween.move(this.gameObject, this.gameObject.transform.position + new Vector3(0, 2, 0), 0.5f).setEaseOutExpo().setOnComplete(() =>
        {
            LeanTween.value(this.gameObject, 1, 0, 0.5f).setOnUpdate((float val) =>
                    {
                        cImg.color = new Color(cImg.color.r, cImg.color.g, cImg.color.b, val);
                        aImg.color = new Color(aImg.color.r, aImg.color.g, aImg.color.b, val);
                        descT.color = new Color(descT.color.r, descT.color.g, descT.color.b, val);
                        manaT.color = new Color(manaT.color.r, manaT.color.g, manaT.color.b, val);
                        cnameT.color = new Color(cnameT.color.r, cnameT.color.g, cnameT.color.b, val);

                    }).setOnComplete(() =>
                    {
                        Destroy(this.gameObject);
                    });
        });

    }
}

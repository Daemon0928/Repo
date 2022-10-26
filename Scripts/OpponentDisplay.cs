using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OpponentDisplay : MonoBehaviour
{
    public Opponent opponent;

    public Text opponentHealth;
    public Text opponentName;
    public Text opponentAttack;
    public Image opponentArtwork;
    public GameObject sliderObject;
    public Image fillImage;
    public GameManager gameManager;
    public float currentHealth;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        opponentHealth.text = opponent.health.ToString();
        currentHealth = opponent.health;
        opponentName.text = opponent.opponentName;
        opponentAttack.text = opponent.attack.ToString();
        opponentArtwork.sprite = opponent.opponentArtwork;
        sliderObject = GameObject.Find("OpponentHealthBarFill");
        fillImage = sliderObject.GetComponent<Image>();
    }

    public void SetHealth()
    {
        if (currentHealth != float.Parse(opponentHealth.text))
        {
            gameManager.ShowDamage(Mathf.Abs(Mathf.RoundToInt(currentHealth - float.Parse(opponentHealth.text))), false, currentHealth - float.Parse(opponentHealth.text) > 0 ? false : true);
        }
        currentHealth = float.Parse(opponentHealth.text);
        LeanTween.value(sliderObject, sliderObject.GetComponent<Image>().fillAmount, currentHealth / opponent.health, 0.4f)
            .setOnUpdate((float val) =>
            {
                fillImage.fillAmount = val;
            }).setEaseOutBack();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroDisplay : MonoBehaviour
{
    public Hero hero;

    public Text heroHealth;
    public Text heroName;
    public Text manaText;
    public Image heroArtwork;
    public float currentHealth;
    public GameObject sliderObject;
    public Image fillImage;
    public GameManager gameManager;

    void Start()
    {
        hero.health = hero.baseHealth + (DBManager.heroLevel * 2);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        heroHealth.text = hero.health.ToString();
        currentHealth = hero.health;
        heroName.text = DBManager.heroName;
        heroArtwork.sprite = hero.heroArtwork;
        sliderObject = GameObject.Find("HeroHealthBarFill");
        fillImage = sliderObject.GetComponent<Image>();
    }

    public void SetHealth()
    {
        if (currentHealth != float.Parse(heroHealth.text))
        {
            gameManager.ShowDamage(Mathf.Abs(Mathf.RoundToInt(currentHealth - float.Parse(heroHealth.text))), true, currentHealth - float.Parse(heroHealth.text) > 0 ? false : true);
        }
        currentHealth = float.Parse(heroHealth.text);
        LeanTween.value(sliderObject, sliderObject.GetComponent<Image>().fillAmount, currentHealth / hero.health, 0.4f)
            .setOnUpdate((float val) =>
            {
                fillImage.fillAmount = val;
            }).setEaseOutBack();
    }
}

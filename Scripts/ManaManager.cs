using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ManaManager : MonoBehaviour
{
    public int mana = 1;
    public Text manaText;
    public Image fillImage;
    public GameManager gameManager;
    public Hero hero;
    public float manaRegen;
    public float time;

    void Start()
    {
        hero = GameObject.Find("Hero").GetComponent<HeroDisplay>().hero;
        fillImage = GameObject.Find("ManaImageFill").GetComponent<Image>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manaRegen = hero.manaRegen;
        time = manaRegen;
    }

    public void manaIncrement()
    {
        if (gameManager.gameHasEnded)
        {
            fillImage.fillAmount = 1;
            return;
        }
        if (mana > 9)
        {
            return;
        }
        else
        {
            mana += 1;
        }
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (mana != 10)
        {
            fillImage.fillAmount = time / manaRegen / (gameManager.enemyPassives.Where(t => t.passive == "SpiderSlow").ToArray()[0].active ? 2 : 1);
        }
        if (time < 0)
        {
            if (gameManager.enemyPassives.Where(t => t.passive == "SpiderSlow").ToArray()[0].active)
            {
                time = manaRegen * 2;
                manaIncrement();
            }
            else
            {
                time = manaRegen;
                manaIncrement();
            }
        }
        manaText.text = mana.ToString();
    }

    public void manaSpent(int amount)
    {
        mana -= amount;
    }

    public bool enoughMana(int amount)
    {
        if (amount > mana)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

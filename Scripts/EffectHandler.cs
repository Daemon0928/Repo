using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class EffectHandler
{
    public Text opponentHealth = GameObject.Find("OpponentHealthText").GetComponent<Text>();
    public Text heroHealth = GameObject.Find("HeroHealthText").GetComponent<Text>();
    public float startingHeroHealth = GameObject.Find("Hero").GetComponent<HeroDisplay>().currentHealth;
    public OpponentDisplay opponentDisplay = GameObject.Find("Opponent").GetComponent<OpponentDisplay>();
    public HeroDisplay heroDisplay = GameObject.Find("Hero").GetComponent<HeroDisplay>();
    public GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    public ManaManager manaManager = GameObject.Find("Mana").GetComponent<ManaManager>();

    public void GetEffect(Card card)
    {
        List<string> effect = new List<string>();
        foreach (var item in card.effect.Split(','))
        {
            if (item.Contains("Deal"))
            {
                //Mirror Ready
                Deal(int.Parse(item.Split(';')[1]));
            }
            else if (item.Contains("Heal"))
            {
                //Mirror Ready
                Heal(int.Parse(item.Split(';')[1]));
            }
            else if (item.Contains("Mana"))
            {
                //Mirror Ready
                Mana(int.Parse(item.Split(';')[1]));
            }
            else if (item.Contains("MRegen"))
            {
                //Mirror Ready
                ManaRegen(int.Parse(item.Split(';')[1].Split('x')[0]), int.Parse(item.Split(';')[1].Split('x')[1]));
            }
            else if (item.Contains("HOvertime"))
            {
                //Mirror Ready
                HealOvertime(int.Parse(item.Split(';')[1].Split('x')[0]), int.Parse(item.Split(';')[1].Split('x')[1]));
            }
            else if (item.Contains("Amaterasu"))
            {
                //Mirror Ready
                Amaterasu(int.Parse(item.Split(';')[1].Split('x')[0]), int.Parse(item.Split(';')[1].Split('x')[1]));
            }
            else if (item.Contains("Stun"))
            {
                //Mirror Ready
                Stun(int.Parse(item.Split(';')[1]));
            }
            else if (item.Contains("Evade"))
            {
                //Mirror Ready
                Evade(int.Parse(item.Split(';')[1]));
            }
            else if (item.Contains("Shield"))
            {
                //Mirror Ready
                Shield(int.Parse(item.Split(';')[1].Split('x')[0]), int.Parse(item.Split(';')[1].Split('x')[1]));
            }
            else if (item.Contains("Kamikaze"))
            {
                //Lazy, sort of Mirror ready
                Kamikaze(int.Parse(item.Split(';')[1].Split('x')[0]), int.Parse(item.Split(';')[1].Split('x')[1]));
            }
            else if (item.Contains("SpellD"))
            {
                //Lazy Mirror Ready
                SpellDamage(int.Parse(item.Split(';')[1].Split('x')[0]), int.Parse(item.Split(';')[1].Split('x')[1]));
            }
            else if (item.Contains("Sacrifice"))
            {
                //Mirror Ready
                Sacrifice(int.Parse(item.Split(';')[1]));
                if (gameManager.enemyPassives.Where(t => t.passive == "Mirror").ToArray()[0].active == true && UnityEngine.Random.Range(1, 100) <= 25)
                {
                    gameManager.ShowText("Sacrifice Mirrored.");
                    Sacrifice(int.Parse(item.Split(';')[1]));
                }
            }
        }
    }

    public void Sacrifice(int damage)
    {
        if (!gameManager.playerEvadeActive)
        {
            heroHealth.text = (float.Parse(heroHealth.text) - (damage + gameManager.playerSpellDamage - gameManager.playerShield)).ToString();
            heroDisplay.SetHealth();
            if ((damage + gameManager.playerSpellDamage - gameManager.playerShield) > 0)
            {
                gameManager.CallEffect(GameObject.Find("HeroArtwork"), "AmaterasuEffect", true);
            }
        }
        else
        {
            gameManager.ShowText("Attack evaded");
        }


        opponentHealth.text = (float.Parse(opponentHealth.text) - (damage + gameManager.playerSpellDamage - gameManager.enemyShield)).ToString();
        opponentDisplay.SetHealth();
        if (damage + gameManager.playerSpellDamage - gameManager.enemyShield > 0)
        {
            gameManager.CallEffect(GameObject.Find("OpponentArtwork"), "AmaterasuEffect", true);
        }
    }

    public void SpellDamage(int amount, int seconds)
    {
        gameManager.CallSpellDamage(amount, seconds);
    }

    public void Kamikaze(int dmg, int times)
    {
        gameManager.CallKamikaze(dmg, times);
    }

    public void Shield(int amount, int seconds)
    {
        gameManager.CallShield(amount, seconds);
    }

    public void Amaterasu(int dmg, int seconds)
    {
        gameManager.CallAmaterasu(dmg, seconds);
    }

    public void Evade(int seconds)
    {
        gameManager.CallEvade(seconds);
    }

    public void Stun(int seconds)
    {
        gameManager.CallStun(seconds);
    }

    public void HealOvertime(int amount, int seconds)
    {
        gameManager.CallHealOvertime(amount, seconds);
    }

    public void Mana(int count)
    {
        if (manaManager.mana + count >= 10)
        {
            if (manaManager.mana != 10)
            {
                gameManager.ShowText($"{10 - manaManager.mana} mana restored.");
            }
            manaManager.mana = 10;
            //gameManager.ShineMana();
        }
        else
        {
            manaManager.mana += count;
            gameManager.ShowText($"{count} mana restored.");
            //gameManager.ShineMana();
        }
    }

    public void ManaRegen(int rate, int seconds)
    {
        manaManager.manaRegen /= rate;
        gameManager.ShowText($"Mana is regenerating faster for {seconds} seconds.");
        LeanTween.value(0, 1, seconds).setOnComplete(() =>
        {
            gameManager.ShowText($"Mana regeneration is back to normal.");
            manaManager.manaRegen *= rate;
        });
    }

    public void Deal(int damage)
    {
        if (gameManager.enemyPassives.Where(t => t.passive == "Evade").ToArray()[0].active)
        {
            if (UnityEngine.Random.Range(1, 100) > 30)
            {
                if (!gameManager.enemyEvade)
                {
                    opponentHealth.text = (float.Parse(opponentHealth.text) - (damage + gameManager.playerSpellDamage - gameManager.enemyShield)).ToString();
                    opponentDisplay.SetHealth();
                    if (damage + gameManager.playerSpellDamage - gameManager.enemyShield > 0)
                    {
                        gameManager.CallEffect(GameObject.Find("OpponentArtwork"), "DamageEffect", true);
                    }

                    if (gameManager.enemyPassives.Where(t => t.passive == "Thorn").ToArray()[0].active)
                    {
                        if (damage + gameManager.playerSpellDamage - gameManager.enemyShield > 0)
                        {
                            gameManager.ThornDamage(damage + gameManager.playerSpellDamage - gameManager.enemyShield);
                        }
                    }
                    Debug.Log("Did not evade");
                }

                if (gameManager.enemyPassives.Where(t => t.passive == "Mirror").ToArray()[0].active)
                {
                    if (UnityEngine.Random.Range(1, 100) <= 25)
                    {
                        heroHealth.text = (float.Parse(heroHealth.text) - (damage - gameManager.playerShield)).ToString();
                        heroDisplay.SetHealth();
                        if (damage - gameManager.playerShield > 0)
                        {
                            gameManager.CallEffect(GameObject.Find("HeroArtwork"), "DamageEffect", false);
                        }
                        gameManager.ShowText("Attack mirrored");
                    }
                    else
                    {
                        Debug.Log("Did not mirror");
                    }
                }
            }
            else
            {
                //Evade
                gameManager.ShowText("Attack evaded...");
            }
        }
        else
        {
            if (!gameManager.enemyEvade)
            {
                opponentHealth.text = (float.Parse(opponentHealth.text) - (damage + gameManager.playerSpellDamage - gameManager.enemyShield)).ToString();
                opponentDisplay.SetHealth();
                if (damage + gameManager.playerSpellDamage - gameManager.enemyShield > 0)
                {
                    gameManager.CallEffect(GameObject.Find("OpponentArtwork"), "DamageEffect", true);
                }

                if (gameManager.enemyPassives.Where(t => t.passive == "Thorn").ToArray()[0].active)
                {
                    if (damage + gameManager.playerSpellDamage - gameManager.enemyShield > 0)
                    {
                        gameManager.ThornDamage(damage + gameManager.playerSpellDamage - gameManager.enemyShield);
                    }
                }
            }

            if (gameManager.enemyPassives.Where(t => t.passive == "Mirror").ToArray()[0].active)
            {
                if (UnityEngine.Random.Range(1, 100) <= 25)
                {
                    heroHealth.text = (float.Parse(heroHealth.text) - (damage - gameManager.playerShield)).ToString();
                    heroDisplay.SetHealth();
                    if (damage - gameManager.playerShield > 0)
                    {
                        gameManager.CallEffect(GameObject.Find("HeroArtwork"), "DamageEffect", false);
                    }
                    gameManager.ShowText("Attack mirrored");
                }
                else
                {
                    Debug.Log("Did not mirror");
                }
            }
        }
    }

    public void Heal(int amount)
    {
        if (float.Parse(heroHealth.text) + (amount + gameManager.playerSpellDamage) >= heroDisplay.hero.health)
        {
            heroHealth.text = heroDisplay.hero.health.ToString();
            heroDisplay.SetHealth();
            gameManager.CallEffect(GameObject.Find("HeroArtwork"), "HealEffect", true);
        }
        else if (float.Parse(heroHealth.text) + (amount + gameManager.playerSpellDamage) < heroDisplay.hero.health)
        {
            heroHealth.text = (float.Parse(heroHealth.text) + (amount + gameManager.playerSpellDamage)).ToString();
            heroDisplay.SetHealth();
            gameManager.CallEffect(GameObject.Find("HeroArtwork"), "HealEffect", true);

        }

        if (gameManager.enemyPassives.Where(t => t.passive == "Mirror").ToArray()[0].active)
        {
            if (UnityEngine.Random.Range(1, 100) <= 25)
            {
                if (float.Parse(opponentHealth.text) + amount >= opponentDisplay.opponent.health)
                {
                    opponentHealth.text = opponentDisplay.opponent.health.ToString();
                    opponentDisplay.SetHealth();
                    gameManager.ShowText("Attack mirrored");
                    gameManager.CallEffect(GameObject.Find("OpponentArtwork"), "HealEffect", true);

                }
                else if (float.Parse(opponentHealth.text) + amount < opponentDisplay.opponent.health)
                {
                    opponentHealth.text = (float.Parse(opponentHealth.text) + amount).ToString();
                    opponentDisplay.SetHealth();
                    gameManager.ShowText("Attack mirrored");
                    gameManager.CallEffect(GameObject.Find("OpponentArtwork"), "HealEffect", true);
                }
            }
            else
            {
                Debug.Log("Did not mirror");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class OpponentAttack : MonoBehaviour
{
    public float attackDamage;
    public Text heroHealth;
    public float attackSpeed;
    public float time;
    public HeroDisplay heroDisplay;
    public OpponentDisplay opponentDisplay;
    public GameManager gameManager;
    public Image fillImage;
    private CardDraw cardDraw;
    private Sprite vampireBiteSprite;

    void Start()
    {
        cardDraw = GameObject.Find("StartingPoint").GetComponent<CardDraw>();
        opponentDisplay = GameObject.Find("Opponent").GetComponent<OpponentDisplay>();
        attackDamage = opponentDisplay.opponent.attack;
        heroHealth = GameObject.Find("HeroHealthText").GetComponent<Text>();
        attackSpeed = opponentDisplay.opponent.attackSpeed;
        heroDisplay = GameObject.Find("Hero").GetComponent<HeroDisplay>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        fillImage = GameObject.Find("OpponentAttackFill").GetComponent<Image>();
        time = attackSpeed;
        vampireBiteSprite = Resources.Load<Sprite>("VampireBite");
        if (gameManager.enemyPassives.Where(t => t.passive == "Burn").ToArray()[0].active) InvokeRepeating("Burning", attackSpeed, 1f);
    }

    //Ellenség támadása
    void Attack()
    {
        if (gameManager.playerEvadeActive == false)
        {
            heroHealth.text = (int.Parse(heroHealth.text) - (attackDamage - gameManager.playerShield < 0 ? 0 : attackDamage - gameManager.playerShield)).ToString();
            heroDisplay.SetHealth();
            if (attackDamage - gameManager.playerShield > 0)
            {
                gameManager.CallEffect(GameObject.Find("HeroArtwork"), "DamageEffect", false);
            }

            if (gameManager.enemyPassives.Where(t => t.passive == "Stun").ToArray()[0].active && gameManager.isStunned == false)
            {
                if (UnityEngine.Random.Range(1, 100) <= 25)
                {
                    StartCoroutine(StunHero());
                }
                else
                {
                    Debug.Log("Stun Missed");
                }
            }

            if (gameManager.enemyPassives.Where(t => t.passive == "Vampire").ToArray()[0].active)
            {
                int heal;
                if (((attackDamage - gameManager.playerShield < 0 ? 0 : attackDamage - gameManager.playerShield) / 2) % 0.5f == 0)
                {
                    heal = Mathf.RoundToInt(((attackDamage - gameManager.playerShield < 0 ? 0 : attackDamage - gameManager.playerShield) / 2) + 0.01f);
                }
                else
                {
                    heal = Mathf.RoundToInt((attackDamage - gameManager.playerShield < 0 ? 0 : attackDamage - gameManager.playerShield) / 2);
                }
                if (float.Parse(opponentDisplay.opponentHealth.text) + heal >= opponentDisplay.opponent.health)
                {
                    if (opponentDisplay.opponent.health - float.Parse(opponentDisplay.opponentHealth.text) > 0)
                    {
                        gameManager.ShowText($"Opponent healed for {opponentDisplay.opponent.health - float.Parse(opponentDisplay.opponentHealth.text)}hp");
                    }
                    opponentDisplay.opponentHealth.text = opponentDisplay.opponent.health.ToString();
                    opponentDisplay.SetHealth();
                }
                else
                {
                    opponentDisplay.opponentHealth.text = (float.Parse(opponentDisplay.opponentHealth.text) + heal).ToString();
                    opponentDisplay.SetHealth();
                    gameManager.ShowText($"Opponent healed for {heal}hp");
                }
                GameObject vampireBite = new GameObject("VampireBite");
                RectTransform trans = vampireBite.AddComponent<RectTransform>();
                trans.SetParent(GameObject.Find("Opponent").GetComponent<RectTransform>());
                trans.SetAsLastSibling();
                trans.localScale = new Vector3(0.5f, 0.5f, 1);
                trans.anchoredPosition = new Vector2(0, 0);
                Image img = vampireBite.AddComponent<Image>();
                img.sprite = vampireBiteSprite;
                img.material = Resources.Load<Material>("VampireOutline");
                LeanTween.value(vampireBite, 1, 0, 1f).setOnUpdate((float val) =>
                 {
                     vampireBite.GetComponent<Image>().color = new Color(vampireBite.GetComponent<Image>().color.r, vampireBite.GetComponent<Image>().color.g, vampireBite.GetComponent<Image>().color.b, val);
                 }).setOnComplete(() => { Object.Destroy(vampireBite); });
            }

            if (gameManager.enemyPassives.Where(t => t.passive == "Thief").ToArray()[0].active)
            {
                if (UnityEngine.Random.Range(1, 100) <= 20)
                {
                    if (GameObject.FindGameObjectsWithTag("Card").ToList().Count > 0)
                    {
                        var cards = GameObject.FindGameObjectsWithTag("Card").ToList();
                        cards[UnityEngine.Random.Range(0, cards.Count - 1)].GetComponent<CardPlay>().CardStolen();
                        gameManager.ShowText("Card Stolen!");
                    }
                    else
                    {
                        Debug.Log("Could not steal");
                    }
                }
                else
                {
                    Debug.Log("Did not steal");
                }
            }
        }
    }

    public void Burning()
    {
        int burnDamage;
        if ((attackDamage / 2) % 0.5f == 0)
        {
            burnDamage = Mathf.RoundToInt((attackDamage / 2) + 0.01f);
        }
        else
        {
            burnDamage = Mathf.RoundToInt(attackDamage / 2);
        }
        if (gameManager.playerEvadeActive == false)
        {
            heroDisplay.heroHealth.text = (int.Parse(heroDisplay.heroHealth.text) - (burnDamage - gameManager.playerShield < 0 ? 0 : burnDamage - gameManager.playerShield)).ToString();
            heroDisplay.SetHealth();
            if (burnDamage - gameManager.playerShield > 0)
            {
                gameManager.CallEffect(GameObject.Find("HeroArtwork"), "DamageEffect", false);
            }
        }
    }

    public IEnumerator StunHero()
    {
        GameObject handTrans = GameObject.Find("Hand");
        gameManager.isStunned = true;
        GameObject StunnedPanel = new GameObject("StunnedPanel");
        RectTransform stunnedTrans = StunnedPanel.AddComponent<RectTransform>();
        stunnedTrans.SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
        stunnedTrans.position = handTrans.GetComponent<Transform>().position;
        stunnedTrans.SetAsLastSibling();
        stunnedTrans.localScale = handTrans.GetComponent<Transform>().localScale;
        stunnedTrans.sizeDelta = handTrans.GetComponent<RectTransform>().sizeDelta;
        Image img = StunnedPanel.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0);
        gameManager.ShowText("Stunned...");

        LeanTween.value(StunnedPanel, 0, 0.5f, 0.2f).setOnUpdate((float val) =>
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, val);
        });
        yield return new WaitForSeconds(2f);
        LeanTween.value(StunnedPanel, StunnedPanel.GetComponent<Image>().color.a, 0, 0.5f).setOnUpdate((float val) =>
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, val);
        });
        yield return new WaitForSeconds(0.5f);
        Destroy(StunnedPanel);
        gameManager.isStunned = false;
    }

    public void CallStunHero(int seconds)
    {
        StartCoroutine(StunHero(seconds));
    }

    public IEnumerator StunHero(int seconds)
    {
        GameObject handTrans = GameObject.Find("Hand");
        gameManager.isStunned = true;
        GameObject StunnedPanel = new GameObject("StunnedPanel");
        RectTransform stunnedTrans = StunnedPanel.AddComponent<RectTransform>();
        stunnedTrans.SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
        stunnedTrans.position = handTrans.GetComponent<Transform>().position;
        stunnedTrans.SetAsLastSibling();
        stunnedTrans.localScale = handTrans.GetComponent<Transform>().localScale;
        stunnedTrans.sizeDelta = handTrans.GetComponent<RectTransform>().sizeDelta;
        Image img = StunnedPanel.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0);
        gameManager.ShowText("Stunned...");

        LeanTween.value(StunnedPanel, 0, 0.5f, 0.2f).setOnUpdate((float val) =>
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, val);
        });
        yield return new WaitForSeconds(seconds - 0.5f);
        LeanTween.value(StunnedPanel, StunnedPanel.GetComponent<Image>().color.a, 0, 0.5f).setOnUpdate((float val) =>
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, val);
        });
        yield return new WaitForSeconds(0.5f);
        Destroy(StunnedPanel);
        gameManager.isStunned = false;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        fillImage.fillAmount = time / attackSpeed;
        if (time < 0)
        {
            time = attackSpeed;
            Attack();
        }
    }
}

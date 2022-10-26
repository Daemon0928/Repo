using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public bool gameHasEnded;
    public GameObject endPanel;
    public GameObject youWon;
    public GameObject youLost;
    public ManaManager manaManager;
    public OpponentAttack opponentAttack;
    public OpponentDisplay opponentDisplay;
    public EffectHandler effectHandler;
    public HeroDisplay heroDisplay;
    public Image winScreenHeroImage;
    public Image LoseScreenHeroImage;
    public Hero hero;
    public GameObject canvas;
    public CardDraw cardDraw;
    public bool playerEvadeActive;
    public bool opponnentStunned;
    public bool playerWon;
    public bool endClickable;
    public bool enemyEvade;
    public bool isStunned;
    public int playerShield;
    public int playerSpellDamage;
    private List<string> triforcedPassives;
    public Font belweb;
    public Text heroHealth;
    public int enemyShield;
    public OpponentTier oppTier;
    public List<EnemyPassive> enemyPassives;

    public void Start()
    {
        
        playerShield = 0;
        playerSpellDamage = 0;
        enemyShield = 0;
        endClickable = false;
        enemyEvade = false;
        gameHasEnded = false;
        opponnentStunned = false;
        playerEvadeActive = false;
        triforcedPassives = new List<string>();
        manaManager = GameObject.Find("Mana").GetComponent<ManaManager>();
        canvas = GameObject.Find("Canvas");
        cardDraw = GameObject.Find("StartingPoint").GetComponent<CardDraw>();
        opponentAttack = GameObject.Find("Opponent").GetComponent<OpponentAttack>();
        opponentDisplay = GameObject.Find("Opponent").GetComponent<OpponentDisplay>();
        heroDisplay = GameObject.Find("Hero").GetComponent<HeroDisplay>();
        effectHandler = new EffectHandler();
        heroHealth = GameObject.Find("HeroHealthText").GetComponent<Text>();
        endPanel = canvas.transform.Find("GameEndScreen").gameObject;
        youWon = endPanel.transform.Find("YouWon").gameObject;
        youLost = endPanel.transform.Find("YouLost").gameObject;
        winScreenHeroImage = youWon.transform.Find("WinScreenHeroImage").GetComponent<Image>();
        LoseScreenHeroImage = youLost.transform.Find("LoseScreenHeroImage").GetComponent<Image>();
        hero = GameObject.Find("Hero").GetComponent<HeroDisplay>().hero;
        winScreenHeroImage.sprite = hero.heroArtwork;
        LoseScreenHeroImage.sprite = hero.heroArtwork;
        youWon.SetActive(false);
        youLost.SetActive(false);
        endPanel.SetActive(false);
        CheckEnemyPassive();
        SetTier();
        if (enemyPassives.Where(t => t.passive == "SpiderSlow").ToArray()[0].active) SpiderPassiveWebShow();
        foreach (var item in enemyPassives.Where(t => t.active == true).Select(t => t.passive))
        {
            ShowEnemyPassives(item);
        }
    }

    public void SetTier()
    {
        switch (GameObject.Find("Opponent").GetComponent<OpponentDisplay>().opponent.opponentName)
        {
            case "Rat":
                oppTier = OpponentTier.tier_1;
                break;
            case "Vampire":
                oppTier = OpponentTier.tier_2;
                break;
            case "Spider":
                oppTier = OpponentTier.tier_3;
                break;
            case "Unknown":
                oppTier = OpponentTier.tier_4;
                break;
            case "Stunning Kobra":
                oppTier = OpponentTier.tier_5;
                break;
            case "Vodoo Mage":
                oppTier = OpponentTier.tier_6;
                break;
            case "Hell Beast":
                oppTier = OpponentTier.tier_7;
                break;
            case "Bandit":
                oppTier = OpponentTier.tier_8;
                break;
            case "Thorn Horror":
                oppTier = OpponentTier.tier_9;
                break;
            case "Drakeness":
                oppTier = OpponentTier.tier_10;
                break;
            default:
                break;
        }
    }

    public enum OpponentTier
    {
        tier_1,
        tier_2,
        tier_3,
        tier_4,
        tier_5,
        tier_6,
        tier_7,
        tier_8,
        tier_9,
        tier_10
    }

    public void Update()
    {
        if (!gameHasEnded)
        {
            if (float.Parse(heroDisplay.heroHealth.text) <= 0 && float.Parse(opponentDisplay.opponentHealth.text) <= 0)
            {
                EndGame(false);
            }
            else if (float.Parse(heroDisplay.heroHealth.text) <= 0)
            {
                EndGame(false);
            }
            else if (float.Parse(opponentDisplay.opponentHealth.text) <= 0)
            {
                EndGame(true);
            }
        }

    }

    public void EndGame(bool p)
    {
        playerWon = p;
        manaManager.enabled = false;
        opponentAttack.CancelInvoke();
        opponentAttack.enabled = false;
        gameHasEnded = true;
        cardDraw.enabled = false;
        for (int i = 0; i < enemyPassives.Count; i++)
        {
            enemyPassives[i].active = false;
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("Card"))
        {
            item.GetComponent<CardPlay>().enabled = false;
            item.GetComponent<CardDisplay>().enabled = false;
        }
        endPanel.GetComponent<RectTransform>().SetAsLastSibling();
        StartCoroutine(Time());
    }

    IEnumerator Time()
    {
        yield return new WaitForSecondsRealtime(1);
        EndGameScreen();
    }

    public void EndGameScreen()
    {
        CancelInvoke();
        endPanel.SetActive(true);

        if (playerWon)
        {
            youWon.SetActive(true);
            LeanTween.scale(youWon, new Vector3(2.4f, 2.4f, 1), 0.5f).setEaseOutBack().setOnComplete(() => { endClickable = true; DBManager.enemyDefeated++; PayTime(); });
        }
        else
        {
            youLost.SetActive(true);
            LeanTween.scale(youLost, new Vector3(2.4f, 2.4f, 1), 0.5f).setEaseOutBack().setOnComplete(() => { endClickable = true; });
        }
    }

    public void endScreenPlayerClicked()
    {
        GameObject.Find("LevelLoader").transform.SetAsLastSibling();
        if (playerWon && SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1)
        {
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().goToNextLevel();
        }
        else if (playerWon && SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().goToMainMenu();
        }
        else
        {
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().goToMainMenu();
        }
    }

    public void ThornDamage(int damage)
    {
        heroDisplay.heroHealth.text = (float.Parse(heroDisplay.heroHealth.text) - Mathf.RoundToInt((damage / 2) + 0.01f) - playerShield).ToString();
        heroDisplay.SetHealth();
        if (Mathf.RoundToInt((damage / 2) + 0.01f) - playerShield > 0)
        {
            CallEffect(GameObject.Find("HeroArtwork"), "DamageEffect", true);
        }
    }

    public void PayTime() 
    {
        DBManager.currency += int.Parse(oppTier.ToString().Split('_')[1]) * Random.Range(70, 100);
    }

    public void SpiderPassiveWebShow()
    {
        Sprite spiderWebSprite = Resources.Load<Sprite>("SpiderWebWhite");
        GameObject spiderWebImg = new GameObject("SpiderWeb");
        RectTransform trans = spiderWebImg.AddComponent<RectTransform>();
        trans.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
        trans.SetAsLastSibling();
        trans.localScale = new Vector3(0f, 0f, 1);
        trans.anchoredPosition = new Vector2(0, 0);
        Image img = spiderWebImg.AddComponent<Image>();
        spiderWebImg.GetComponent<Image>().sprite = spiderWebSprite;

        LeanTween.scale(spiderWebImg, new Vector3(4f, 4f, 1), 2f).setOnComplete(() =>
        {
            LeanTween.value(spiderWebImg, 1, 0, 2f).setOnUpdate((float val) =>
{
    spiderWebImg.GetComponent<Image>().color = new Color(spiderWebImg.GetComponent<Image>().color.r, spiderWebImg.GetComponent<Image>().color.g, spiderWebImg.GetComponent<Image>().color.b, val);
}).setOnComplete(() => { Object.Destroy(spiderWebImg); ShowText("Mana regeneration slowed"); });
        });

        GameObject.Find("ManaImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("ManaWebbed");
    }

    public void CheckEnemyPassive()
    {
        enemyPassives = new List<EnemyPassive>()
        {
            new EnemyPassive("Vampire"),
            new EnemyPassive("SpiderSlow"),
            new EnemyPassive("Evade"),
            new EnemyPassive("Stun"),
            new EnemyPassive("Mirror"),
            new EnemyPassive("Burn"),
            new EnemyPassive("Thief"),
            new EnemyPassive("Thorn"),
            new EnemyPassive("Triforce")
        };

        foreach (var item in enemyPassives)
        {
            if (opponentDisplay.opponent.passive.Contains(item.passive))
            {
                item.active = true;
            }
        }

        if (enemyPassives.Where(t => t.passive == "Triforce").ToArray()[0].active)
        {
            if (enemyPassives.Where(t => t.active == false).Count() >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, enemyPassives.Count - 1);
                    while (enemyPassives[randomIndex].active == true)
                    {
                        randomIndex = UnityEngine.Random.Range(0, enemyPassives.Count - 1);
                    }
                    enemyPassives[randomIndex].active = true;
                    triforcedPassives.Add(enemyPassives[randomIndex].passive);
                }
            }
            else
            {
                for (int i = 0; i < enemyPassives.Where(t => t.active == false).Count(); i++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, enemyPassives.Count - 1);
                    while (enemyPassives[randomIndex].active == true)
                    {
                        randomIndex = UnityEngine.Random.Range(0, enemyPassives.Count - 1);
                    }
                    enemyPassives[randomIndex].active = true;
                    triforcedPassives.Add(enemyPassives[randomIndex].passive);
                }
            }
            foreach (var item in triforcedPassives)
            {
                Debug.Log($"passive added: {item}");
            }
        }
    }

    public void ShowText(string text)
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("DisplayText"))
        {
            item.GetComponent<RectTransform>().anchoredPosition = new Vector2(item.GetComponent<RectTransform>().anchoredPosition.x,
                item.GetComponent<RectTransform>().anchoredPosition.y - 30);
        }

        GameObject displayText = new GameObject(text);
        displayText.tag = "DisplayText";
        RectTransform trans = displayText.AddComponent<RectTransform>();
        trans.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
        trans.SetAsLastSibling();
        trans.localScale = new Vector3(0.2f, 0.2f, 1);
        trans.anchoredPosition = new Vector2(0f, -50f);
        Text t = displayText.AddComponent<Text>();
        t.font = DBManager.belweb;
        t.fontSize = 200;
        t.horizontalOverflow = HorizontalWrapMode.Overflow;
        t.verticalOverflow = VerticalWrapMode.Overflow;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = Color.white;
        t.text = text;
        t.raycastTarget = false;

        LeanTween.value(displayText, 1, 0, 1.5f).setOnUpdate((float val) =>
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, val);
        }).setOnComplete(() =>
        {
            Destroy(displayText);
        });
    }

    public void ShowDamage(int amount, bool hero, bool heal)
    {
        if (GameObject.FindGameObjectWithTag(hero ? "HeroDamageText" : "EnemyDamageText"))
        {
            Destroy(GameObject.FindGameObjectWithTag(hero ? "HeroDamageText" : "EnemyDamageText"));
        }

        GameObject DamageText = new GameObject("DamageText");
        DamageText.tag = hero ? "HeroDamageText" : "EnemyDamageText";
        RectTransform trans = DamageText.AddComponent<RectTransform>();
        trans.SetParent(hero ? GameObject.Find("HeroHealthBar").GetComponent<RectTransform>() : GameObject.Find("OpponentHealthBar").GetComponent<RectTransform>());
        trans.SetAsLastSibling();
        trans.localScale = new Vector3(0.2f, 0.2f, 1);
        trans.anchoredPosition = new Vector2(0f, 1f);
        Text t = DamageText.AddComponent<Text>();
        t.font = DBManager.belweb;
        t.fontSize = 75;
        t.horizontalOverflow = HorizontalWrapMode.Overflow;
        t.verticalOverflow = VerticalWrapMode.Overflow;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = Color.white;
        t.text = (heal ? "+" : "-") + amount.ToString();
        t.raycastTarget = false;

        LeanTween.value(DamageText, 1, 0, 1.5f).setOnUpdate((float val) =>
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, val);
        }).setOnComplete(() =>
        {
            Destroy(DamageText);
        });
    }

    public void ShowEnemyPassives(string name)
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("PassiveText"))
        {
            item.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -20);
        }

        GameObject PassiveText = new GameObject(name);
        PassiveText.tag = "PassiveText";
        RectTransform trans = PassiveText.AddComponent<RectTransform>();
        trans.SetParent(GameObject.Find("Opponent").GetComponent<RectTransform>());
        trans.SetAsLastSibling();
        trans.localScale = new Vector3(0.2f, 0.2f, 1);
        trans.anchoredPosition = new Vector2(-100f, 30f);
        Text t = PassiveText.AddComponent<Text>();
        t.font = DBManager.belweb;
        t.fontSize = 50;
        t.horizontalOverflow = HorizontalWrapMode.Overflow;
        t.verticalOverflow = VerticalWrapMode.Overflow;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = Color.white;
        t.text = name;
        t.raycastTarget = false;
    }

    IEnumerator CreateEffect(GameObject go, string effectName, bool wasCard)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition += new Vector2(2000f, 2000f);
        }
        GameObject Effect = (GameObject)Instantiate(Resources.Load(effectName));
        RectTransform trans = Effect.GetComponent<RectTransform>();
        Image img = Effect.transform.Find("Image").GetComponent<Image>();
        img.material = Instantiate(img.material);
        Material mat = img.material;
        mat.SetColor("_OutlineColor", new Color(img.color.r, img.color.g, img.color.b, 0) * 2f);
        mat.SetColor("_Color", new Color(img.color.r, img.color.g, img.color.b, 0) * 2f);
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        trans.SetParent(go.transform);
        trans.SetAsLastSibling();
        trans.anchoredPosition = new Vector2(0, 0);
        trans.localScale = new Vector3(1f, 1f, 1f);
        if (effectName == "HealEffect")
        {
            Effect.transform.Find("Particle System").GetComponent<ParticleSystem>().GetComponent<Renderer>().material = mat;
        }

        LeanTween.value(Effect, 0, 1, 0.3f).setOnUpdate((float val) =>
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, val);
            mat.SetColor("_OutlineColor", new Color(mat.GetColor("_OutlineColor").r, mat.GetColor("_OutlineColor").g, mat.GetColor("_OutlineColor").b, val));
            mat.SetColor("_Color", new Color(mat.GetColor("_Color").r, mat.GetColor("_Color").g, mat.GetColor("_Color").b, val));
        });
        yield return new WaitForSeconds(0.8f);
        LeanTween.value(Effect, 1, 0, 0.3f).setOnUpdate((float val) =>
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, val);
            mat.SetColor("_OutlineColor", new Color(mat.GetColor("_OutlineColor").r, mat.GetColor("_OutlineColor").g, mat.GetColor("_OutlineColor").b, val));
            mat.SetColor("_Color", new Color(mat.GetColor("_Color").r, mat.GetColor("_Color").g, mat.GetColor("_Color").b, val));
        }).setOnComplete(() => { Destroy(Effect); });
    }


    public void CallEffect(GameObject go, string effectName, bool wasCard)
    {
        StartCoroutine(CreateEffect(go, effectName, wasCard));
    }

    public void CallAmaterasu(int dmg, int seconds)
    {
        StartCoroutine(Amaterasu(dmg, seconds));
    }

    public IEnumerator Amaterasu(int dmg, int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            if (!gameHasEnded)
            {
                yield return new WaitForSeconds(1f);
                if (!enemyEvade)
                {

                    CallEffect(GameObject.Find("OpponentArtwork"), "AmaterasuEffect", false);
                    opponentDisplay.opponentHealth.text = (float.Parse(opponentDisplay.opponentHealth.text) - Mathf.RoundToInt(((dmg + playerSpellDamage) / seconds) + 0.01f)).ToString();
                    opponentDisplay.SetHealth();
                }
                else
                {
                    ShowText("Enemy evaded.");
                }

            }
            if (enemyPassives.Where(t => t.passive == "Mirror").ToArray()[0].active == true && UnityEngine.Random.Range(1, 100) <= 25)
            {
                if (!gameHasEnded)
                {
                    yield return new WaitForSeconds(1f);
                    if (!playerEvadeActive)
                    {
                        CallEffect(GameObject.Find("HeroArtwork"), "AmaterasuEffect", false);
                        heroHealth.text = (float.Parse(heroHealth.text) - Mathf.RoundToInt((dmg / seconds) + 0.01f)).ToString();
                        heroDisplay.SetHealth();
                    }
                    else
                    {
                        ShowText("Attack evaded.");
                    }
                }
            }

        }
    }

    public void CallStun(int seconds)
    {
        StartCoroutine(Stun(seconds));
    }

    public IEnumerator Stun(int seconds)
    {
        opponentAttack.enabled = false;
        opponnentStunned = true;
        ShowText($"Opponent stunned for {seconds} seconds.");

        if (enemyPassives.Where(t => t.passive == "Mirror").ToArray()[0].active == true && UnityEngine.Random.Range(1, 100) <= 25)
        {
            opponentAttack.CallStunHero(seconds);
        }

        yield return new WaitForSeconds(seconds);
        opponentAttack.enabled = true;
        opponnentStunned = false;
        ShowText($"The opponent is not stunned anymore.");
    }

    public void CallKamikaze(int dmg, int times)
    {
        StartCoroutine(Kamikaze(dmg, times));
    }

    public IEnumerator Kamikaze(int dmg, int times)
    {
        effectHandler.Deal(dmg);
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < times - 2; i++)
        {
            yield return new WaitForSeconds(0.2f);
            effectHandler.Deal(dmg);
        }
        yield return new WaitForSeconds(0.3f);
        effectHandler.Deal(dmg);
    }

    public void CallShield(int amount, int seconds)
    {
        StartCoroutine(Shield(amount, seconds));
    }

    public IEnumerator Shield(int amount, int seconds)
    {
        playerShield += amount;
        bool enemy = false;
        if (enemyPassives.Where(t => t.passive == "Mirror").ToArray()[0].active == true && UnityEngine.Random.Range(1, 100) <= 25)
        {
            enemyShield += amount;
            enemy = true;
        }
        ShowText($"Your shield blocks {amount} damage for {seconds} seconds.");
        ShowText("Shield Mirrored");
        yield return new WaitForSeconds(seconds);
        playerShield -= amount;
        if (enemy)
        {
            enemyShield -= amount;
        }
        ShowText($"Your shield is no longer active.");
    }

    public void CallSpellDamage(int amount, int seconds)
    {
        StartCoroutine(SpellDamage(amount, seconds));
    }

    public IEnumerator SpellDamage(int amount, int seconds)
    {
        playerSpellDamage += amount;
        ShowText($"You have {amount} spell damage for {seconds} seconds.");
        yield return new WaitForSeconds(seconds);
        playerSpellDamage -= amount;
        ShowText($"Spell damage is no longer active.");
    }

    public void CallEvade(int seconds)
    {
        StartCoroutine(Evade(seconds));
    }

    public IEnumerator Evade(int seconds)
    {
        playerEvadeActive = true;
        ShowText($"Evade is active for {seconds} seconds.");
        if (enemyPassives.Where(t => t.passive == "Mirror").ToArray()[0].active == true && UnityEngine.Random.Range(1, 100) <= 25)
        {
            ShowText("Evade Mirrored.");
            enemyEvade = true;
        }
        yield return new WaitForSeconds(seconds);
        playerEvadeActive = false;
        enemyEvade = false;
        ShowText($"Evade is not active anymore.");
    }

    public void CallHealOvertime(int amount, int seconds)
    {
        StartCoroutine(HealOvertime(amount, seconds));
        if (enemyPassives.Where(t => t.passive == "Mirror").ToArray()[0].active == true && UnityEngine.Random.Range(1, 100) <= 25)
        {
            StartCoroutine(HealOvertimeOpponent(amount, seconds));
        }
    }

    public IEnumerator HealOvertime(int amount, int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            if (float.Parse(heroHealth.text) == heroDisplay.hero.health)
            {
                yield return new WaitForSeconds(1f);
            }
            else if (float.Parse(heroHealth.text) + Mathf.RoundToInt(((amount + playerSpellDamage) / seconds) + 0.01f) > heroDisplay.hero.health)
            {
                heroHealth.text = heroDisplay.hero.health.ToString();
                heroDisplay.SetHealth();
                CallEffect(GameObject.Find("HeroArtwork"), "HealEffect", true);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                heroHealth.text = (float.Parse(heroHealth.text) + Mathf.RoundToInt(((amount + playerSpellDamage) / seconds) + 0.01f)).ToString();
                heroDisplay.SetHealth();
                CallEffect(GameObject.Find("HeroArtwork"), "HealEffect", true);
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public IEnumerator HealOvertimeOpponent(int amount, int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            if (float.Parse(opponentDisplay.opponentHealth.text) == opponentDisplay.opponent.health)
            {
                yield return new WaitForSeconds(1f);
            }
            else if (float.Parse(opponentDisplay.opponentHealth.text) + Mathf.RoundToInt((amount / seconds) + 0.01f) > opponentDisplay.opponent.health)
            {
                opponentDisplay.opponentHealth.text = opponentDisplay.opponent.health.ToString();
                opponentDisplay.SetHealth();
                CallEffect(GameObject.Find("OpponentArtwork"), "HealEffect", true);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                opponentDisplay.opponentHealth.text = (float.Parse(opponentDisplay.opponentHealth.text) + Mathf.RoundToInt((amount / seconds) + 0.01f)).ToString();
                opponentDisplay.SetHealth();
                CallEffect(GameObject.Find("OpponentArtwork"), "HealEffect", true);
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void ExitPressed() 
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        EndGame(false);
    }
}

public class EnemyPassive : MonoBehaviour
{
    public string passive;
    public bool active;

    public EnemyPassive(string s)
    {
        passive = s;
        active = false;
    }
}

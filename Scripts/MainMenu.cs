using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Text usernameText;
    public Text heroName;
    public Image heroImage;
    public GameObject nameChanging;
    public Button nameEdit;
    public Button nameEditOk;
    public InputField NameEditInput;
    public Text enemyDefeatedCount;
    public Text currency;
    public Text levelUpPriceText;
    public Text heroHealthText;
    public Text Power;
    public Font gameFont;
    public GameObject NotEnoughGold;

    public void Start()
    {
        CallUpdateDb();
        CallUpdateStat();
        usernameText.text = DBManager.username;
        heroName.text = DBManager.heroName + $"({DBManager.heroLevel})";
        heroImage.sprite = Resources.Load<Sprite>("Mage");
        enemyDefeatedCount.text = DBManager.enemyDefeated.ToString();
        currency.text = DBManager.currency.ToString();
        DBManager.belweb = gameFont;
        foreach (var item in DBManager.cardCollection.Select(t => t.cardName))
        {
            Debug.Log("collection card: " + item);
        }
        foreach (var item in DBManager.deck.Select(t => t.cardName))
        {
            Debug.Log("collection card: " + item);
        }

        if (DBManager.heroLevel == 1)
        {
            levelUpPriceText.text = "Level Up(300)";
        }
        else
        {
            levelUpPriceText.text = $"Level Up({Mathf.RoundToInt(Mathf.Pow(1.1f, DBManager.heroLevel - 1) * 300)})";
        }
        heroHealthText.text = $"Hero health bonus: {DBManager.heroLevel * 2}";
        Power.text = $"Power: {15 + DBManager.heroLevel}";
    }

    public void NameEditProcess()
    {
        nameChanging.SetActive(true);
    }

    public void VerifyHeroNameChangeInput()
    {
        nameEditOk.interactable = NameEditInput.text.Length > 2 && NameEditInput.text.Length <= 50;
    }

    public void UpdateHeroName()
    {
        DBManager.heroName = NameEditInput.text;
        heroName.text = DBManager.heroName+$"({DBManager.heroLevel})";
        nameChanging.SetActive(false);
        CallUpdateDb();
    }

    public void CallUpdateStat()
    {
        StartCoroutine(UpdateStat());
    }

    public void CallUpdateDb()
    {
        StartCoroutine(UpdateDb());
    }

    public IEnumerator UpdateDb()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", DBManager.username);
        form.AddField("userlevel", DBManager.level);
        form.AddField("heroname", DBManager.heroName);
        form.AddField("herolevel", DBManager.heroLevel);
        form.AddField("cardcollection", ConvertBacktoCollectionString(DBManager.cardCollection));
        form.AddField("deck", ConvertBacktoCollectionString(DBManager.deck));
        WWW www = new WWW("http://localhost/CardCasterDungeon/updateUserData.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Update succeded");
        }
        else
        {
            Debug.Log("Update failed. msg: " + www.text);
        }
    }

    public IEnumerator UpdateStat()
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", DBManager.userID);
        form.AddField("enemyDefeated", DBManager.enemyDefeated);
        form.AddField("currency", DBManager.currency);
        WWW www = new WWW("http://localhost/CardCasterDungeon/updatePlayerStat.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Update succeded");
        }
        else
        {
            Debug.Log("Update failed. msg: " + www.text);
        }
    }

    public string ConvertBacktoCollectionString(List<Card> cardList)
    {
        string cardListString = "";
        foreach (var item in cardList.OrderBy(t => t.id).GroupBy(g => g).Select(g => new { card = g.Key, cnt = g.Count() }))
        {
            cardListString += $"{item.card.id}x{item.cnt}.";
        }
        return cardListString.Remove(cardListString.Length - 1);
    }

    public void PlayButtonClicked()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().goToNextLevel();
    }

    public void CollectionButtonClicked()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().goToCollection();
    }

    public void ShopButtonClicked()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().goToShop();
    }

    public void HowToButtonClicked()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }

    public void LevelUpClicked()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        int levelUpPrice = Mathf.RoundToInt(Mathf.Pow(1.1f, DBManager.heroLevel - 1) * 300);
        if (DBManager.currency < levelUpPrice)
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
        else
        {
            DBManager.currency -= levelUpPrice;
            DBManager.heroLevel += 1;
            heroName.text = DBManager.heroName + $"({DBManager.heroLevel})";
            currency.text = DBManager.currency.ToString();
            levelUpPriceText.text = $"Level Up({Mathf.RoundToInt(Mathf.Pow(1.1f, DBManager.heroLevel - 1) * 300)})";
            heroHealthText.text = $"Hero health bonus: {DBManager.heroLevel * 2}";
            Power.text = $"Power: {15 + DBManager.heroLevel}";
        }
    }

    public void ExitButtonClicked()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        Application.Quit();
    }
}

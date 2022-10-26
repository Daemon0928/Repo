using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public List<Card> allCards;
    public int level;

    public Button submit;

    public void CallLogin()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        StartCoroutine(LoginPlayer());
    }

    IEnumerator LoginPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username.text);
        form.AddField("password", password.text);
        WWW www = new WWW("http://localhost/CardCasterDungeon/login.php", form);
        yield return www;

        if (www.text.Split(';')[0] == "0")
        {
            DBManager.allCards = this.allCards;
            DBManager.username = username.text;
            DBManager.level = int.Parse(www.text.Split(';')[1]);
            DBManager.heroType = www.text.Split(';')[2];
            DBManager.heroName = www.text.Split(';')[3];
            DBManager.heroLevel = int.Parse(www.text.Split(';')[4]);
            DBManager.userID = int.Parse(www.text.Split(';')[5]);
            DBManager.enemyDefeated = int.Parse(www.text.Split(';')[6]);
            DBManager.currency = int.Parse(www.text.Split(';')[7]);
            DBManager.cardsFromDB(www.text.Split(';')[8], www.text.Split(';')[9]);
            Debug.Log($"Logged in to: {DBManager.username}({DBManager.level}) successfully");
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().goToMainMenu();
        }
        else
        {
            if (www.text == "")
            {
                GameObject.Find("ErrorText").GetComponent<Text>().text = "Connection error.";
            }
            else
            {
                GameObject.Find("ErrorText").GetComponent<Text>().text = $"Error: {www.text}";
            }

        }
    }

    public void VerifyInputs()
    {
        submit.interactable = (username.text.Length > 5 && password.text.Length > 7);
    }

    public void ExitClicked()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        Application.Quit();
    }
}

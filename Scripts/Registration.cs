using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class Registration : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public InputField email;
    public List<Card> allCards;
    public Button submit;

    public void CallRegister()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {

        WWWForm form = new WWWForm();
        form.AddField("username", username.text);
        form.AddField("password", password.text);
        form.AddField("email", email.text);
        WWW www = new WWW("http://localhost/CardCasterDungeon/register.php", form);
        yield return www;

        if (www.text.Split(';')[0] == "0")
        {
            DBManager.allCards = this.allCards;
            DBManager.username = username.text;
            DBManager.level = 1;
            DBManager.heroType = www.text.Split(';')[1];
            DBManager.heroName = www.text.Split(';')[2];
            DBManager.heroLevel = int.Parse(www.text.Split(';')[3]);
            DBManager.userID = int.Parse(www.text.Split(';')[4]);
            DBManager.enemyDefeated = int.Parse(www.text.Split(';')[5]);
            DBManager.currency = int.Parse(www.text.Split(';')[6]);
            DBManager.cardsFromDB(www.text.Split(';')[7], www.text.Split(';')[8]);
            Debug.Log("User created successfully");
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
                GameObject.Find("ErrorText").GetComponent<Text>().text = "Error while creating user: #" + www.text;
            }
        }
    }

    public void VerifyInputs()
    {
        submit.interactable = (username.text.Length > 5 && password.text.Length > 7 && email.text.Length > 4 && email.text.Contains("@") && email.text.Contains("."));
    }

    public void ExitClicked()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        Application.Quit();
    }
}

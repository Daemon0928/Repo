using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    private void Start()
    {
        transition = this.transform.Find("Crossfade").GetComponent<Animator>();
    }

    public void goToLogin()
    {
        StartCoroutine(LoadLevel("LoginMenu"));
    }

    public void goToRegister()
    {
        StartCoroutine(LoadLevel("RegisterMenu"));
    }

    public void goToShop()
    {
        StartCoroutine(LoadLevel("Shop"));
    }

    public void goToMainMenu()
    {
        StartCoroutine(LoadLevel("MainMenu"));    
    }

    public void goToFirstLevel()
    {
        StartCoroutine(LoadLevel("FirstLevel"));
    }

    public void goToCollection()
    {
        StartCoroutine(LoadLevel("Collection"));
    }

    public void goToNextLevel()
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(levelName);
    }

    IEnumerator LoadNextLevel()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}

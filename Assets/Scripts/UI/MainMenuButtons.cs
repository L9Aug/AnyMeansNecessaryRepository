using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuButtons : MonoBehaviour {

    public static bool continueGame;

    public void newGame()
    {

    }

    public void Continue()
    {
        XMLManager.instance.LoadEnemy();
        SceneManager.LoadScene(XMLManager.instance.enemyDB.currentScene);
        continueGame = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

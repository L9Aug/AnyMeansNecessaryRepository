using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuButtons : MonoBehaviour {

    public static bool continueGame;

    public Button continueButton;

    void Start()
    {
        if(System.IO.File.Exists(Application.persistentDataPath + "/enemData.xml"))
        {
            XMLManager.instance.LoadEnemy();
        }
        else
        {
            continueButton.interactable = false;
        }
    }

    public void newGame()
    {
        SceneManager.LoadScene("HubWorldBlockoutV2");
    }

    public void Continue()
    {
        
        SceneManager.LoadScene(XMLManager.instance.enemyDB.currentScene);
        continueGame = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;

public class MainMenuButtons : MonoBehaviour {

    public static bool continueGame;

    public Button continueButton;

    void Start()
    {
        if(File.Exists(Application.persistentDataPath + "/enemData.xml"))
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
        // delete Everything in app data.
        if(File.Exists(Application.persistentDataPath + "\\InventoryData.txt")) File.Delete(Application.persistentDataPath + "\\InventoryData.txt");
        if(File.Exists(Application.persistentDataPath + "\\SkillsData.txt")) File.Delete(Application.persistentDataPath + "\\SkillsData.txt");
        if(File.Exists(Application.persistentDataPath + "\\enemData.xml")) File.Delete(Application.persistentDataPath + "\\enemData.xml");

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

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dialog : MonoBehaviour {


   
    void OnTriggerEnter()
    {
        UIElements.ContextText.GetComponent<Text>().text = "Press 'F' to talk";
        PauseMenu.MenuOfPause.inDialogRange = true;
    }

    void OnTriggerExit()
    {
        UIElements.ContextText.GetComponent<Text>().text = " ";
        PauseMenu.MenuOfPause.inDialogRange = false;
    }
}

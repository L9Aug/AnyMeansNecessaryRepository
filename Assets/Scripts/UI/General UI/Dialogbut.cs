using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dialogbut : MonoBehaviour {

    public static Text staticDialogButton1;
    public static Text staticDialogButton2;
    public static Text staticDialogButton3;
    public static Text staticDialogButton4;
    public static Text staticNPCResponse;
    public DialogOptions[] dialogOptions;
    public Text[] DialogButtons;
    public Text NPCResponse;

    bool informationRequested = false;

    // Use this for initialization
    void Start () {
        staticDialogButton1 = DialogButtons[0];
        staticDialogButton2 = DialogButtons[1];
        staticDialogButton3 = DialogButtons[2];
        staticDialogButton4 = DialogButtons[3];
        staticNPCResponse = NPCResponse;
        dialogText();
	}



    public void Option1()
    {
        if (!Mission_Giver.missionAccepted)
        {
            Mission_Giver.missionAccepted = true;
            dialogText();
            PauseMenu.MenuOfPause.SetTransitionTarget();
        }
        if(Mission_Giver.missionAccepted && !Mission_Giver.missionCompleted)
        {

        }
        
    }

    public void Option2()
    {
        informationRequested = true;
        dialogText();
    }

    public void Option3()
    {

    }

    public void Option4()
    {
        PauseMenu.MenuOfPause.SetTransitionTarget();
    }

    void dialogText()
    {
        print("dialog");
        if (!Mission_Giver.missionAccepted)
        {
            staticNPCResponse.text = dialogOptions[0].NPCResponse;
            staticDialogButton1.text = dialogOptions[0].PlayerResponses[0];
            staticDialogButton2.text = dialogOptions[0].PlayerResponses[1];
            staticDialogButton3.text = dialogOptions[0].PlayerResponses[2];
            staticDialogButton4.text = dialogOptions[0].PlayerResponses[3];
        }
        if(Mission_Giver.missionAccepted)
        {
            staticNPCResponse.text = dialogOptions[1].NPCResponse;
            staticDialogButton1.text = dialogOptions[1].PlayerResponses[0];
            staticDialogButton2.text = dialogOptions[1].PlayerResponses[1];
            staticDialogButton3.text = dialogOptions[1].PlayerResponses[2];
            staticDialogButton4.text = dialogOptions[1].PlayerResponses[3];
        }

        if(informationRequested)
        {
            //extra info screen
            staticNPCResponse.text = dialogOptions[2].NPCResponse;
            staticDialogButton1.text = dialogOptions[2].PlayerResponses[0];
            staticDialogButton2.text = dialogOptions[2].PlayerResponses[1];
            staticDialogButton3.text = dialogOptions[2].PlayerResponses[2];
            staticDialogButton4.text = dialogOptions[2].PlayerResponses[3];
            informationRequested = false;
        }
        if(Mission_Giver.missionCompleted)
        {

        }
    }
}

[System.Serializable]
public class DialogOptions
{
    public string[] PlayerResponses = new string[4];
    public string NPCResponse;
}
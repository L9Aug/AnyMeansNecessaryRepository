using UnityEngine;
using System.Collections;

public class Mission_Giver : MonoBehaviour {
    public string QuestDialog;
    public string Answer1;
    public string Answer2;
    public string Answer3;

    public string QuestCompleteDialog;
    public string CompeteionAnswer1;
    public string CompeteionAnswer2;
    public string CompeteionAnswer3;

    public GameObject Player;
    public GameObject npcAgent;
    public GameObject TargetAgentIcon;
    public GameObject _TargetEnemy;
    private bool talkingToNpc;
    public bool missionAccepted;
    public bool missionDetailsGiven;
    public bool missionCompleted;
    private bool missionCompletedDialog;

    // Use this for initialization
    void Start () {
	    Player = GameObject.Find("Player");
        _TargetEnemy = (GameObject)Resources.Load("Target");
        TargetAgentIcon = (GameObject)Resources.Load("Target");
        spawnTarget(npcAgent, TargetAgentIcon);
    }

    // Update is called once per frame
    public void callMission () {
        CheckPlayerDistance();
    }

    private void CheckPlayerDistance()
    {
        Vector3 distToPlayer = npcAgent.transform.position - Player.transform.position;
        if (distToPlayer.magnitude > 4)
        {

        }
        else
        {
            Vector3 PlayerLookPos = new Vector3(Player.transform.position.x, npcAgent.transform.position.y, Player.transform.position.z);
            npcAgent.transform.LookAt(PlayerLookPos);
            if (distToPlayer.magnitude < 2)
            {
                if (!missionCompleted)
                {
                    CallMission();
                }else
                {
                    CompleteMissionDialog();
                }
            }
        }
    }

    private void CallMission()
    {
        if (Input.GetButtonDown("Interact") && talkingToNpc == false && missionAccepted == false)
        {
            Debug.Log(QuestDialog);
            talkingToNpc = true;
        }
        else if (talkingToNpc == true && missionAccepted == false)
        {
            if (Input.GetKeyDown("u"))
            {
                Debug.Log(Answer1);
                talkingToNpc = false;
                missionAccepted = true;
            }
            if (Input.GetKeyDown("i"))
            {
                Debug.Log(CompeteionAnswer2);
                talkingToNpc = false;
                missionAccepted = true;
            }
            if (Input.GetKeyDown("o"))
            {
                Debug.Log(CompeteionAnswer3);
                talkingToNpc = false;
                missionAccepted = true;
            }
            if (Input.GetKeyDown("p"))
            {
                Debug.Log("declined mission");
                talkingToNpc = false;
                missionAccepted = false;
            }
        }
    }
    private void CompleteMissionDialog()
    {
        if (Input.GetButtonDown("Interact") && talkingToNpc == false && missionCompletedDialog == false)
        {
            Debug.Log(QuestCompleteDialog);
            talkingToNpc = true;
        }
        else if (talkingToNpc == true && missionCompletedDialog == false)
        {
            if (Input.GetKeyDown("u"))
            {
                Debug.Log(CompeteionAnswer1);
                talkingToNpc = false;
                missionCompletedDialog = true;
            }
            if (Input.GetKeyDown("i"))
            {
                Debug.Log(CompeteionAnswer2);
                talkingToNpc = false;
                missionCompletedDialog = true;
            }
            if (Input.GetKeyDown("o"))
            {
                Debug.Log(CompeteionAnswer3);
                talkingToNpc = false;
                missionCompletedDialog = true;
            }
        }
    }

    ///spawning icon above head indicator
    public void spawnTarget(GameObject target, GameObject _Target)
    {
      //  _Target = (GameObject)Resources.Load("Target");
        _Target = (GameObject)Instantiate(_Target, Vector3.zero, Quaternion.identity);
        _Target.transform.parent = target.gameObject.transform;
        _Target.transform.localPosition = new Vector3(0, 1.8f, 0);
    }
}

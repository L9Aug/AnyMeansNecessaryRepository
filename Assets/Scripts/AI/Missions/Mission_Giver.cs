using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Mission_Giver : MonoBehaviour {

  

    public GameObject Player;
    public GameObject npcAgent;
    public GameObject TargetAgentIcon;
    public GameObject _TargetEnemy;
    public static bool talkingToNpc;
    public static bool missionAccepted;
    public static bool missionDetailsGiven;
    public static bool missionCompleted;
    public static bool missionCompletedDialog;

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
        if (distToPlayer.magnitude < 4)
        {
            Vector3 PlayerLookPos = new Vector3(Player.transform.position.x, npcAgent.transform.position.y, Player.transform.position.z);
            npcAgent.transform.LookAt(PlayerLookPos);
            if (distToPlayer.magnitude < 2)
            {
                if (!missionCompleted)
                {
                    MissionInfo();
                }
                else
                {
                    CompleteMissionDialog();
                }
            }
        }
    }

    private void MissionInfo()
    {
        if (Input.GetButtonDown("Interact") && talkingToNpc == false && missionAccepted == false)
        {
           
            talkingToNpc = true;
        }
        else if (talkingToNpc == true && missionAccepted == false)
        {
            if (Input.GetKeyDown("u"))
            {
             
                talkingToNpc = false;
                missionAccepted = true;
            }
            if (Input.GetKeyDown("i"))
            {
                
                talkingToNpc = false;
                missionAccepted = true;
            }
            if (Input.GetKeyDown("o"))
            {
                
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
          
            talkingToNpc = true;
        }
        else if (talkingToNpc == true && missionCompletedDialog == false)
        {
            if (Input.GetKeyDown("u"))
            {
              
                talkingToNpc = false;
                missionCompletedDialog = true;
            }
            if (Input.GetKeyDown("i"))
            {
              
                talkingToNpc = false;
                missionCompletedDialog = true;
            }
            if (Input.GetKeyDown("o"))
            {
               
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



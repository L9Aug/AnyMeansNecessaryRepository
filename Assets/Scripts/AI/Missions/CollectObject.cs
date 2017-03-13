using UnityEngine;
using System.Collections;

public class CollectObject : Base_Mission {

    public GameObject Collect;
    public string text;
    private GameObject Player;
	// Use this for initialization
	void Start () {
        Player = GameObject.Find("Player");
        TargetAgentIcon = (GameObject)Resources.Load("Target");
        spawnTarget(npcAgent, TargetAgentIcon);
    }
    private void intaliseMissionHud()
    {
        MissonDetails(text, questNumber);
    }


    // Update is called once per frame
    void Update () {


        if (missionAccepted && missionDetailsGiven)
        {
            Vector3 distToObject = Collect.transform.position - Player.transform.position;
            if (distToObject.magnitude < 2)
            {
                Destroy(Collect);
                giveXP(50);
                MissionComplete();
                Destroy(GetComponent<CollectObject>());

            }
        }
        else if (missionAccepted && !missionDetailsGiven)
        {
            missionDetailsGiven = true;
            intaliseMissionHud();
            // Destroy(TargetAgentIcon); // bugged doesnt destroy it :S
        }
	}
}

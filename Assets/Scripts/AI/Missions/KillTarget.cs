using UnityEngine;
using System.Collections;

public class KillTarget : Base_Mission
{

    public GameObject enemyTarget;
    void Start()
    {
        Player = GameObject.Find("Player");
        _TargetEnemy = (GameObject)Resources.Load("Target");
        TargetAgentIcon = (GameObject)Resources.Load("Target");
        spawnTarget(npcAgent, TargetAgentIcon);
    }
    private void intaliseMissionHud()
    {

        MissonDetails("Kill: " + enemyTarget.gameObject.name, questNumber);
        spawnTarget(enemyTarget, _TargetEnemy);
        
    }

    void Update()
    {
        callMission();
        if (missionAccepted && missionDetailsGiven)
        {
            Completed(CheckComplete());

        }else if (missionAccepted && !missionDetailsGiven)
        {
            missionDetailsGiven = true;
            intaliseMissionHud();
           // DestroyImmediate(TargetAgentIcon);

        }
    }

    private bool CheckComplete()
    {
            if (enemyTarget.GetComponent<Base_Enemy>()._state == Base_Enemy.State.Dead)
            {
                return true;
            }
        return false;

    }

    private void Completed(bool _hasCompleted)
    {
        if (_hasCompleted && !rewardGiven)
        {
            questCompletedAmount++;
            MissionComplete();
            giveXP(xpReward);
            rewardGiven = true;
            Debug.Log("test");
          //DestroyImmediate(_TargetEnemy, true);
        }
    }
}

using UnityEngine;
using System.Collections;

public class KillxEnemies : Base_Mission
{
    /// <summary>
    /// //////////UNFINISHED
    /// </summary>

	// Use this for initialization
        
    public enum enemyTarget
    {
        standard,
        armored,
        sniper,
        hunter,
        all
    }
    public enemyTarget _target;
    public int targetAmount;
    public string text;

    private int remainingTargets;
    private int _remainingTargets;

    void Start () {
        if(_target == enemyTarget.standard)
        {
            remainingTargets = Base_Enemy.StandardKills + targetAmount;
        }
        else if (_target == enemyTarget.sniper)
        {
            remainingTargets = Base_Enemy.sniperKills + targetAmount;
        }
        else if (_target == enemyTarget.armored)
        {
            remainingTargets = Base_Enemy.armoredKills + targetAmount;
        }
        else if (_target == enemyTarget.hunter)
        {
            remainingTargets = Base_Enemy.hunterKills + targetAmount;
        }
        else if(_target == enemyTarget.all)
        {
            remainingTargets = Base_Enemy.killCount + targetAmount;
        }

        MissonDetails("Kill " + remainingTargets + " " + text, questNumber);
	}
	
	void FixedUpdate () {
        //Debug.Log(_remainingTargets + " _remaining - " + remainingTargets);
        CheckKills();
        Completed(CheckComplete());
    }

    private void CheckKills() 
    {
        if (_target == enemyTarget.standard)
        {
            _remainingTargets = Base_Enemy.StandardKills;
        }
        else if (_target == enemyTarget.sniper)
        {
            _remainingTargets = Base_Enemy.sniperKills;
        }
        else if (_target == enemyTarget.armored)
        {
            _remainingTargets = Base_Enemy.armoredKills;
        }
        else if (_target == enemyTarget.hunter)
        {
            _remainingTargets = Base_Enemy.hunterKills;
        }
        else if (_target == enemyTarget.all)
        {
            _remainingTargets = Base_Enemy.killCount;
        }
        UpdateMissionText("Kill " + (targetAmount - _remainingTargets) + " " + text);
    }
    private bool CheckComplete()
    {
        if(_remainingTargets >= remainingTargets)
        {
            return true;
        }
     return  false;
    }

    private void Completed(bool _hasCompleted)
    {
        if (_hasCompleted)
        {
            questCompletedAmount++;
            MissionComplete();
            giveXP(xpReward);
            Destroy(GetComponent<KillxEnemies>());
        }
    }
}

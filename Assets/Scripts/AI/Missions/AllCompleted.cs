using UnityEngine;
using System.Collections;

public class AllCompleted : Base_Mission {
    public int totalQuestAmount;
    public string text;
    private bool completed;

    void Start()
    {
        completed = false;
    }

    void Update()
    {
        if(totalQuestAmount == questCompletedAmount && !completed)
        {
            giveXP(xpReward);
            MissonDetails(text, questNumber);
            MissionComplete();
            completed = true;
        }
    }

}

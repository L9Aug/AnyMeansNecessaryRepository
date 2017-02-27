using UnityEngine;
using System.Collections;

public class QuestController : MonoBehaviour {

public void addQuest(string type)
    {
        if(type == "KillTarget")
        {
            gameObject.AddComponent<KillTarget>();
        }
        else if(type == "KillXTargets")
        {
            gameObject.AddComponent<KillxEnemies>();

        }
        else if(type == "CollectObject")
        {
            gameObject.AddComponent<CollectObject>();

        }

    }
}

using UnityEngine;
using System.Collections;

public class AIRemoveVisual : MonoBehaviour {

   

    GameObject AI;
   
	// Use this for initialization
	void Start () {
        AI = transform.parent.gameObject;
  
        
        
	}
	
	// Update is called once per frame
	void Update () {

        if (AI.GetComponent<Base_Enemy>()._state == Base_Enemy.State.Dead)
        {
            Destroy(gameObject);
        }
	}

    public static void removeMiniMapVisual()
    {
        
    }
}

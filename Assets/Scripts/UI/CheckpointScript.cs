using UnityEngine;
using System.Collections;

public class CheckpointScript : MonoBehaviour {

    bool isActivated;
    public static float storedHealth;
    public static int storedXp;
    public static GameObject[] checkPointList;
    public static Vector3 []EnemyPosition ;

   
    // Use this for initialization
    void Start () {
        checkPointList = GameObject.FindGameObjectsWithTag("Checkpoint"); //Finds all the checkpoints in the level by searching for the tag	
        EnemyPosition =  new Vector3[GameObject.FindGameObjectsWithTag("Enemy").Length];

       // XMLManager.instance.enemyDB.enemList = new System.Collections.Generic.List<EnemDataToSave>(GameObject.FindGameObjectsWithTag("Enemy").Length);
    }

    private void ActivateCheckpoint()//creates an active chekpoint, active chekpoint position data is the data that will be stored
    {
        Debug.Log("Active checkpoint called");
        foreach (GameObject checkpoint in checkPointList)
        {
            checkpoint.GetComponent<CheckpointScript>().isActivated = false;           
        }
        isActivated = true;        
    }

    public static Vector3 GetCheckpointPosition()//gathers the active checkpoints position 
    {
        Vector3 posValue = new Vector3(0.0f, 0.418f, 0.0f);
        if (checkPointList != null)
        {
            foreach (GameObject checkpoint in checkPointList)
            {
                if(checkpoint.GetComponent<CheckpointScript>().isActivated == true)
                {
                    posValue = checkpoint.transform.position;                    
                    break;
                }
            }
        }
        return posValue;
    }

    void OnTriggerEnter(Collider checkpointCollider)//stores all required data on collision with the checkpoints trigger area
    {
        if (!checkpointCollider.isTrigger)
        {
            if (checkpointCollider.tag == "Player")
            {
                ActivateCheckpoint();
                XMLManager.instance.xmlstoredata();
                XMLManager.instance.saveEnemy();

            }
        }
    }

}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour {

    public static XMLManager instance;
    public DataBase enemyDB;
    // Use this for initialization
    void Awake ()
    {
        instance = this;        
	}  

    public void saveEnemy()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DataBase));
        FileStream stream = new FileStream(Application.dataPath + "/DataTest/enemData.xml",FileMode.Create);
        serializer.Serialize(stream, enemyDB);
        stream.Close();
    }

    public void LoadEnemy()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DataBase));
        FileStream stream = new FileStream(Application.dataPath + "/DataTest/enemData.xml", FileMode.Open);
        enemyDB = serializer.Deserialize(stream) as DataBase;
        stream.Close();

    }

    public void xmlstoredata()
    {        
		int arrayLength = FindObjectsOfType<Base_Enemy>().Length;
        Base_Enemy[] tempEnemies = FindObjectsOfType<Base_Enemy>();

        enemyDB.enemList.Clear();

        if (enemyDB.enemList.Count <= arrayLength)
        {
            for (int i = 0; i < arrayLength; i++)
            {
                enemyDB.enemList.Add(new EnemDataToSave(tempEnemies[i].transform.position,
                                                        tempEnemies[i].transform.rotation,
                                                        tempEnemies[i].GetComponent<HealthComp>().GetHealth(),
                                                        tempEnemies[i].GetComponent<FieldOfView>().detectedtimer,
                                                        tempEnemies[i].GetComponent<Base_Enemy>()._state,
                                                        Enemy_Patrol.detected
                                                        ));
            }  
        }
        instance.enemyDB.PlayerHealth = PlayerController.PC.GetComponent<HealthComp>().GetHealth();
        instance.enemyDB.PlayerXP = UIElements.xp;
        instance.enemyDB.PlayerPos = PlayerController.PC.transform.position;
        instance.enemyDB.PlayerRot = PlayerController.PC.transform.rotation;
    }

}

    [System.Serializable]
public class EnemDataToSave
{
    
    public Vector3 enemPos;
    public Quaternion enemyRot;    
    public float enemHealth;
    public float detectionTimer;
    public Base_Enemy.State enemyState;
    public bool detected;

    public EnemDataToSave(Vector3 pos,Quaternion rot,float health,float timer, Base_Enemy.State state, bool Detected)
    {
        enemPos = pos;
        enemyRot = rot;
        enemHealth = health;
        detectionTimer = timer;
        enemyState = state;
        detected = Detected;
    }

    public EnemDataToSave()
    {

    }
}

[System.Serializable]
public class DataBase
{
    
    public List<EnemDataToSave> enemList = new List<EnemDataToSave>();

    public float PlayerHealth;
    public int PlayerXP;
    public int PlayerLevel;
    public int ammoCount;
    public Vector3 PlayerPos;
    public Quaternion PlayerRot;

    public DataBase()
    {

    }
    
}




using UnityEngine;
using System.Collections;

public class CollectObject : Base_Mission {

    public GameObject Collect;
    public string text;
    private GameObject Player;
	// Use this for initialization
	void Start () {
        Player = GameObject.Find("Player");
        MissonDetails(text, questNumber);


    }
	
	// Update is called once per frame
	void Update () {
        Vector3 distToObject = Collect.transform.position - Player.transform.position;
	if(distToObject.magnitude < 2)
        {
            Destroy(Collect);
            giveXP(50);
            MissionComplete();
            Destroy(GetComponent<CollectObject>());
            
        }
	}
}

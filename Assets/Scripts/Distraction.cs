using UnityEngine;
using System.Collections;

public class Distraction : MonoBehaviour {
    public static float objectDestroyInSeconds = 10;
    private float objectDestroyTimer = 0;
	
	// Update is called once per frame
	void Update () {
        objectDestroyTimer += Time.deltaTime;

        if(objectDestroyTimer >= objectDestroyInSeconds)
        {
            Destroy(gameObject);
        }
	}
}

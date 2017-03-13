using UnityEngine;
using System.Collections;

public class SceneTransitionTrigger : MonoBehaviour
{
    public PauseMenu PM;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PM.SetTransitionTarget(true, PauseMenu.PausedMachine.SceneTransPrompt);
        }
    }
}

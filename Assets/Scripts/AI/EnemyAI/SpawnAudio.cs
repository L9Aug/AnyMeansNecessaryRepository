using UnityEngine;
using System.Collections;

public class SpawnAudio : MonoBehaviour {

    private GameObject audiosphere;

    // Use this for initialization
    void Start () {
        audiosphere = (GameObject)Resources.Load("AudioSphere");

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void spawnAudio(Vector3 Location, float radius)
    {
        GameObject sphere = (GameObject)Instantiate((GameObject)Resources.Load("AudioSphere"), Location, Quaternion.identity);
        sphere.GetComponent<SphereCollider>().radius = radius;
    }
}

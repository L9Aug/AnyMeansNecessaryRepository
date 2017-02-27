using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnableObject : MonoBehaviour {

    public bool EnableOnAwake;

    public bool DestroyThisAfterEnable;

    public List<GameObject> GameObjectsToEnable = new List<GameObject>();

    void Awake()
    {
        if (EnableOnAwake)
        {
            EnableObjects();
        }
    }

	// Use this for initialization
	void Start () {
        if (!EnableOnAwake)
        {
            EnableObjects();
        }
	}
	
	void EnableObjects()
    {
        foreach(GameObject go in GameObjectsToEnable)
        {
            go.SetActive(true);
        }
        if (DestroyThisAfterEnable)
        {
            Destroy(gameObject);
        }
    }
}

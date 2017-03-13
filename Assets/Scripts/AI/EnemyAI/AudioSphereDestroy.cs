using UnityEngine;
using System.Collections;

public class AudioSphereDestroy : MonoBehaviour {
    public static float objectDestroyInSeconds = 0.01f;
    private float objectDestroyTimer = 0;

    // Update is called once per frame
    void Update()
    {
        objectDestroyTimer += Time.deltaTime;

        if (objectDestroyTimer >= objectDestroyInSeconds)
        {
            Destroy(gameObject);
        }
    }
}

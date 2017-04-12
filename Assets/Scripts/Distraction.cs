using UnityEngine;
using System.Collections;

public class Distraction : MonoBehaviour
{
    public static float objectDestroyInSeconds = 10;
    private float objectDestroyTimer = 0;
    public AudioEffectController myDistAEC;

    // Update is called once per frame
    void Update () {
        objectDestroyTimer += Time.deltaTime;

        if(objectDestroyTimer >= objectDestroyInSeconds)
        {
            Destroy(gameObject);
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        myDistAEC.PlayAudio();
    }
}

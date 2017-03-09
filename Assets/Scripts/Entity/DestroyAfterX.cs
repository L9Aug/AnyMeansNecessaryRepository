using UnityEngine;
using System.Collections;

public class DestroyAfterX : MonoBehaviour
{
    public float Lifetime;

    float Timer;

    private void Start()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while(Timer < Lifetime)
        {
            Timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

}

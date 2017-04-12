using UnityEngine;
using System.Collections;

public class Explosive : MonoBehaviour
{
    public static float FuseLength = 3;
    public static float BaseFuseLength = 3;
    public static float BaseExplosionRange = 10;
    public static float ExplosiveRange = 10;
    public float ExplosiveDamage;
    public GameObject ExplosionEffect;

    public AudioEffectController Beeps;

    float Timer;
    int layersToHit = (1 << 10);

    private void Start()
    {
        Timer = 0;
        Beeps.PlayAudio();
        StartCoroutine(CountDown());
    }

    private void Detonate()
    {
        // Spawn Explosion Effect
        Instantiate(ExplosionEffect, transform.position, ExplosionEffect.transform.rotation, transform.parent);

        // Checksphere for colliders hit        
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosiveRange, layersToHit, QueryTriggerInteraction.Ignore);

        // for each collider hit get it's distance from this obj and test line of site.
        for (int i = 0; i < colliders.Length; ++i)
        {
            if (!Physics.Linecast(transform.position, colliders[i].transform.position, 1 << 9, QueryTriggerInteraction.Ignore))
            {
                // send hit through based upon this distance ie. (dmg / dist)
                colliders[i].SendMessage("Hit", ExplosiveDamage / (Vector3.Distance(transform.position, colliders[i].transform.position)), SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private IEnumerator CountDown()
    {
        while(Timer < FuseLength)
        {
            Timer += Time.deltaTime;
            yield return null;
        }
        Detonate();
        Destroy(gameObject);
    }

}

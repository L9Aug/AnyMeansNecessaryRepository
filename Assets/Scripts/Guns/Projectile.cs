using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour {

    public float ProjectileLife;
    public bool UseGravity;
    public float Damage;

    public delegate void CollisionCallback(Collider other);
    public List<CollisionCallback> CollisionCallbacks = new List<CollisionCallback>();

    Vector3 Velocity;
    Vector3 PreviousPosition;

    void FixedUpdate()
    {
        transform.Translate(Velocity * Time.deltaTime, Space.World);
        if (UseGravity) Velocity += Physics.gravity * Time.deltaTime;

        RaycastHit hit;
        int layerMask = 1 << 13;
        layerMask = ~layerMask;
        if(Physics.Linecast(PreviousPosition, transform.position, out hit, layerMask, QueryTriggerInteraction.Ignore))
        {
            Collided(hit.collider);
        }

        PreviousPosition = transform.position;
    }

    public void StartProjectile(float velocity, float damage)
    {
        Velocity = velocity * transform.forward;
        Damage = damage;
        PreviousPosition = transform.position;
        StartCoroutine(Lifetime(ProjectileLife));
    }

    void Collided(Collider other)
    {
        foreach(CollisionCallback collisionCallback in CollisionCallbacks)
        {
            if(collisionCallback != null)
            {
                collisionCallback(other);
            }
        }

        other.SendMessage("Hit", Damage, SendMessageOptions.DontRequireReceiver);

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        Collided(other.collider);
    }

    IEnumerator Lifetime(float time)
    {
        do
        {
            yield return new WaitForFixedUpdate();
            time -= Time.fixedDeltaTime;
        } while (time > 0);
        Destroy(gameObject);
    }

}

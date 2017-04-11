using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour {

    public float ProjectileLife;
    public bool UseGravity;
    public float Damage;
    public float HeadshotDamage;

    public delegate void CollisionCallback(Collider other);
    public List<CollisionCallback> CollisionCallbacks = new List<CollisionCallback>();

    Vector3 Velocity;
    Vector3 PreviousPosition;
    protected bool useHeadshotDamage;

    void FixedUpdate()
    {
        transform.Translate(Velocity * Time.deltaTime, Space.World);
        if (UseGravity) Velocity += Physics.gravity * Time.deltaTime;

        RaycastHit hit;
        int layerMask = 1 << 13;
        layerMask = ~layerMask;

        if(Physics.Linecast(PreviousPosition, transform.position, out hit, layerMask, QueryTriggerInteraction.Ignore))
        {
            TestForDmgMode(hit.collider, hit.point);
            Collided(hit.collider);
        }

        PreviousPosition = transform.position;
    }

    public void StartProjectile(float velocity, float damage, float headshotDamage)
    {
        Velocity = velocity * transform.forward;
        Damage = damage;
        HeadshotDamage = headshotDamage;
        PreviousPosition = transform.position;
        StartCoroutine(Lifetime(ProjectileLife));
    }

    protected virtual void Collided(Collider other)
    {
        foreach(CollisionCallback collisionCallback in CollisionCallbacks)
        {
            if(collisionCallback != null)
            {
                collisionCallback(other);
            }
        }
        other.SendMessage("Hit", useHeadshotDamage ? HeadshotDamage : Damage, SendMessageOptions.DontRequireReceiver);

        Destroy(gameObject);
    }

    void TestForDmgMode(Collider other, Vector3 hitPoint)
    {
        useHeadshotDamage = false;
        if(other is CapsuleCollider)
        {
            float hitDist = Vector3.Distance(other.transform.position, hitPoint);
            CapsuleCollider myCol = (CapsuleCollider)other;
            useHeadshotDamage = hitDist > myCol.height / 1.18f;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        TestForDmgMode(other.collider, other.contacts[0].point);
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

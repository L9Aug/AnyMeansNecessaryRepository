using UnityEngine;
using System.Collections;

public class TranqProjectile : Projectile
{

    protected override void Collided(Collider other)
    {
        foreach (CollisionCallback collisionCallback in CollisionCallbacks)
        {
            if (collisionCallback != null)
            {
                collisionCallback(other);
            }
        }

        other.SendMessage("Hit", useHeadshotDamage ? HeadshotDamage : Damage, SendMessageOptions.DontRequireReceiver);

        if(other.gameObject.GetComponent<Base_Enemy>() != null)
        {
            if (useHeadshotDamage)
            {
                other.GetComponent<Base_Enemy>().setState(Base_Enemy.State.Stunned);
            }
            else
            {
                other.gameObject.AddComponent<TranqEffect>();
                
            }
        }

        Destroy(gameObject);
    }

}

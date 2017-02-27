using UnityEngine;
using System.Collections;

public class Stalker : MonoBehaviour {

    public float StalkerRange = 10;

    public void TestForStalker(Collider other)
    {
        bool foundEnemy = false;
        Collider[] nearColiders = Physics.OverlapSphere(other.transform.position, StalkerRange);
        foreach (Collider col in nearColiders)
        {
            if(col.GetComponent<Base_Enemy>() != null && col != other)
            {
                foundEnemy = true;
            }
        }

        if (!foundEnemy)
        {
            GetComponent<Projectile>().Damage *= 1.5f;
        }
    }

}

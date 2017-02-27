using UnityEngine;
using System.Collections;

public class BleedDamage : MonoBehaviour
{

    public float Damage = 15;
    public float Lifetime = 5;

    float totalLifetime;
    HealthComp healthComp;

	// Use this for initialization
	void Start ()
    {
        healthComp = GetComponent<HealthComp>();
        if (healthComp == null) Destroy(this);
        totalLifetime = Lifetime;
	}
	
	// Update is called once per frame
	void Update ()
    {
        healthComp.Hit(Damage * (Time.deltaTime / totalLifetime));
        Lifetime -= Time.deltaTime;
        if (Lifetime <= 0) Destroy(this);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthComp : MonoBehaviour {

    public float MaxHealth;
    float health;
    float armour;

    public delegate void HealthChanged(float Health, float ChangeInHealth, float armour);
    public List<HealthChanged> healthChanged = new List<HealthChanged>();

	// Use this for initialization
	void Start ()
    {
        health = MaxHealth;
	}

    public void ChangeHealth(float Amount)
    {
        health += Amount;
        health = Mathf.Clamp(health, 0, MaxHealth);
        CallhealthChanged(Amount);
    }

    public float GetHealth()
    {
        return health;
    }

    public void ChangeMaxHealth(float newMax)
    {
        if (newMax != MaxHealth)
        {
            float deltaHealth = 0;
            if (newMax < MaxHealth)
            {
                MaxHealth = newMax;
                
                if(health > MaxHealth)
                {
                    deltaHealth = MaxHealth - health;
                    health = MaxHealth;
                }
            }
            else
            {
                deltaHealth = newMax - MaxHealth;
                MaxHealth = newMax;
                health += deltaHealth;
            }
            CallhealthChanged(deltaHealth);
        }
    }
    
    public void SetHealth(float Value)
    {
        float DeltaHealth = Value - health;
        health = Value;
        CallhealthChanged(DeltaHealth);
    }

    public void Hit(float Amount)
    {
        if(armour > 0)
        {
            armour -= Amount;
            if(armour < 0)
            {
                health += armour;
                armour = 0;
            }
        }
        else
        {
            health -= Amount;
        }

        CallhealthChanged(Amount);

        if(this.gameObject.tag != "Player" && health > 0 && gameObject.tag != "Civilian" && gameObject.GetComponent<Base_Enemy>()._state != Base_Enemy.State.Dead && gameObject.GetComponent<Base_Enemy>()._state != Base_Enemy.State.Stunned)
        {
            GetComponent<Enemy_Was_Shot>().Shot();
        }
    }

    public void AddArmour(float Amount)
    {
        armour += Amount;
        CallhealthChanged(0);
    }

    void CallhealthChanged(float Amount)
    {
        if (healthChanged.Count > 0)
        {
            foreach (HealthChanged h in healthChanged)
            {
                h(health, Amount, armour);
            }
        }
    }
}

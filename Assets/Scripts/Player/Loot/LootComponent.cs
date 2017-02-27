using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootComponent : MonoBehaviour
{
    Base_Enemy thisAI;
    public bool HasBeenLooted = false;

    // Use this for initialization
    void Start()
    {
        thisAI = GetComponent<Base_Enemy>();
    }

    /// <summary>
    /// add in a way in the inspector for selecting multiple enum conditions
    /// </summary>
    /// <returns></returns>
    public List<LootableItem> GetLoot()
    {
        LootableItem.Providers WhatIAm = LootableItem.Providers.All;

        if(thisAI is Standard_Enemy)
        {
            WhatIAm = LootableItem.Providers.Standard;
        }
        else if (thisAI is Sniper_Enemy)
        {
            WhatIAm = LootableItem.Providers.Sniper;
        }
        else if (thisAI is Armored_Enemy)
        {
            WhatIAm = LootableItem.Providers.Armoured;
        }
        else if(thisAI is Hunter_Enemy)
        {
            WhatIAm = LootableItem.Providers.Lone;
        }

        return LootController.LC.GetLoot(WhatIAm);
    }

    public bool IsLootable()
    {
        return thisAI._state == Base_Enemy.State.Dead;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootController : MonoBehaviour
{ 
    public static LootController LC;

    public List<LootableItem> LootableItems = new List<LootableItem>();

	// Use this for initialization
	void Start ()
    {
        LC = this;
	}
	
    public List<LootableItem> GetLoot(LootableItem.Providers type)
    {
        List<LootableItem> loot = new List<LootableItem>();
        List<LootableItem> AvailableLoot = LootableItems.FindAll(x => x.DroppedBy == LootableItem.Providers.All || x.DroppedBy == type);

        int GuarenteedLoot = Random.Range(0, AvailableLoot.Count);
        loot.Add(AvailableLoot[GuarenteedLoot]);
        AvailableLoot.RemoveAt(GuarenteedLoot);
        loot[0].Quantity = Random.Range(loot[0].MinAmount, loot[0].MaxAmount + 1);

        foreach(LootableItem lootItem in AvailableLoot)
        {
            if(Random.value * 100 < lootItem.Chance)
            {
                lootItem.Quantity = Random.Range(lootItem.MinAmount, lootItem.MaxAmount + 1);
                loot.Add(lootItem);
            }
        }      

        return loot;
    }

}

[System.Serializable]
public class LootableItem
{
    public string name;
    public int MinAmount, MaxAmount;
    public Providers DroppedBy;

    [Tooltip("The chance that this item is added if it is not the guarenteed loot.\nChance is a percentage.")]
    public int Chance;

    [HideInInspector]
    public int Quantity;

    public enum Providers { All, Standard, Armoured, Lone, Sniper };
}

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Items {

   public string itemName;
   public int idValue;
   public Sprite itemSprite;
   public float itemDamage;
   public float itemNoise;
   public int itemValue;
   public int itemWeight;
   public int currentStack;
   public int maxItemStack;
   public string description;
   public GameObject model;
   public TypeofItem itemType;

    public enum TypeofItem
    {
        Equipable,
        Consumable,
        EquipAndConsume,
        Quest,
        misc
    }

    public Items(string name, int id, float damage, float noise, int value, int weight,int stack,int maxStack, string desc,TypeofItem TypeItem )
    {
        itemName = name;
        idValue = id;
        itemDamage = damage;
        itemNoise = noise;
        itemValue = value;
        itemWeight = weight;
        currentStack = stack;
        maxItemStack = maxStack;
        description = desc;
        itemType = TypeItem;
        itemSprite = Resources.Load<Sprite>("" + name); //name passed in must be sprite fileName; 
    }

    public Items() // for 0 items 
    {

    }    
    
}

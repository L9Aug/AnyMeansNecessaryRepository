using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase InventoryDataBase;
    public ItemDetail[] newItem;
    public List<Items> itemList = new List<Items>();    
    
	// Use this for initialization
	void Start ()
    {
        InventoryDataBase = this;
        for (int i = 0; i < newItem.Length; i++)
        {
            InventoryDataBase.itemList.Add(new Items(newItem[i].name,i,newItem[i].damage,newItem[i].noise,
                                                    newItem[i].value,newItem[i].weight,newItem[i].stack,
                                                    newItem[i].maxStack,newItem[i].description,newItem[i].itemType));

            
        }
        //print(itemList.Count);
    }

}

[System.Serializable]
public class ItemDetail
{    
    public string name;
    public float damage;
    public float noise;
    public int weight;
    public int value;
    public int stack;
    public int maxStack;
    public string description;
    public Items.TypeofItem itemType;
}

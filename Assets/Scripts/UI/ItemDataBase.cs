using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;


public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase InventoryDataBase;
    public ItemDetail[] newItem;
    public List<Items> itemList = new List<Items>();

    // Use this for initialization
    void Start()
    {
        InventoryDataBase = this;
        LoadInventory();
    }

    public void SaveInventory()
    {
        List<string> itemData = new List<string>();
        for (int i = 0; i < itemList.Count; i++)
        {
            itemData.Add(string.Format("{0},{1},{2},{3},{4},{5},{6}", itemList[i].itemName, itemList[i].itemValue, itemList[i].itemWeight, itemList[i].currentStack, itemList[i].maxItemStack, (int)itemList[i].itemType, itemList[i].description));
        }
        File.WriteAllLines(Application.persistentDataPath + "/InventoryData.txt", itemData.ToArray());
    }

    public void LoadInventory()
    {
        if (File.Exists(Application.persistentDataPath + "\\InventoryData.txt"))
        {
            // Continue loading from file.
            string[] items = File.ReadAllText(Application.persistentDataPath + "/InventoryData.txt").Split('\n');

            if (itemList.Count > 0) itemList.Clear();

            for (int i = 0; i < 19; ++i)
            {
                itemList.Add(new Items(items[i].Split(',')));
            }
        }
        else
        {
            // load Normally
            for (int i = 0; i < newItem.Length; i++)
            {
                InventoryDataBase.itemList.Add(new Items(newItem[i].name, i, newItem[i].damage, newItem[i].noise,
                                                        newItem[i].value, newItem[i].weight, newItem[i].stack,
                                                        newItem[i].maxStack, newItem[i].description, newItem[i].itemType));
            }
            SaveInventory();
        }
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

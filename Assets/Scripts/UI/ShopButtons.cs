using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopButtons : MonoBehaviour {

    public static int money = 10;
    public static bool selling = false;

    public Button shopItemImage;
    public GameObject SpawnPosition;
    Button[] shopItem = new Button[20];

    int[] id = new int[20];
    int itemCount;
   Vector3 position = new Vector3(0,0,0);

    public void BuyItems()
    {
        if (itemCount < 20)
        {
            instantitiateItem();
        }
        
    }



    void instantitiateItem()
    {
        for(int i=0;i < 20;i++)
        {  
            id[i] = Random.Range(0,ItemDataBase.InventoryDataBase.itemList.Count);
            print(id[i]);
            shopItemImage.GetComponent<Image>().sprite = ItemDataBase.InventoryDataBase.itemList[id[i]].itemSprite;

            shopItem[i] = Instantiate(shopItemImage,SpawnPosition.transform,false) as Button;

            shopItem[i].GetComponent<RectTransform>().localPosition = SpawnPosition.transform.position;
            
            shopItem[i].GetComponent<ItemEnum>().thisItem = (ItemEnum.Item)id[i];

            shopItem[i].onClick.AddListener(delegate { purchaseItem(id[i - 1]); });            

            itemCount++;
        }

        
    }

    void purchaseItem(int id)
    {
        print(id);
        if (money >= ItemDataBase.InventoryDataBase.itemList[id].itemValue)
        {
            Inventory.playerInventory.AddItem(id);
            money = money - ItemDataBase.InventoryDataBase.itemList[id].itemValue;
        }
        else
        {
            print("not enough money");
        }
    }


    public void SellItems()
    {
        selling = true;
        print(selling);
       
        
    }
}

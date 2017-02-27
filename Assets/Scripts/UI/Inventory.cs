using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Inventory : MonoBehaviour {

    static public Inventory playerInventory;
    public Button ItemImage;
    public RawImage Weapon;
    public RawImage Health;
    public RawImage Distraction;
    public RawImage Misc;
    public RawImage Quest;

    static int lastId;

    Vector3 WeaponImagePosition           = Vector3.zero;
    Vector3 HealthImagePosition           = Vector3.zero;
    Vector3 MiscImagePosition             = Vector3.zero;
    Vector3 QuestImagePosition            = Vector3.zero;
    Vector3 DistractionImagePosition      = Vector3.zero;

    int weight;
    int maxWeight;
    Button[] itemButton;

	// Use this for initialization
	void Start() {
       playerInventory = this;
       lastId = -1; 
       maxWeight = 60;
       itemButton = new Button[maxWeight]; 
       WeaponImagePosition = new Vector3(-12, -114, 0);
       HealthImagePosition = new Vector3(-12, -114, 0);
       DistractionImagePosition = new Vector3(-12, -114, 0);
       MiscImagePosition = new Vector3(-12, -114, 0);
       QuestImagePosition = new Vector3(-12, -114, 0);


	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (weight < maxWeight)
            {
                AddItem(9);
            }

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (weight < maxWeight)
            {
                AddItem(6);
            }

        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            if(weight < maxWeight)
            {
                AddItem(0);
            }
        }


    }

 public void AddItem(int id)
    {  
      

        if (ItemDataBase.InventoryDataBase.itemList[id].currentStack < ItemDataBase.InventoryDataBase.itemList[id].maxItemStack) 
        {
            if (ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.Equipable)
            {
                InstantiateItem(Weapon, WeaponImagePosition, id);
                WeaponImagePosition = MoveItemImage(WeaponImagePosition);
            }

            else if (ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.Consumable)
            {
                if (ItemDataBase.InventoryDataBase.itemList[id].currentStack < 1)
                {
                    InstantiateItem(Health, HealthImagePosition, id);
                    HealthImagePosition = MoveItemImage(HealthImagePosition);
                    itemButton[id].GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                }
                else
                {
                    ItemDataBase.InventoryDataBase.itemList[id].currentStack++;
                    weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;
                    itemButton[id].GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                    
                    
                }
            }

            else if(ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.EquipAndConsume)
            {
                if (ItemDataBase.InventoryDataBase.itemList[id].currentStack < 1)
                {
                    InstantiateItem(Distraction, DistractionImagePosition, id);
                    DistractionImagePosition = MoveItemImage(DistractionImagePosition);
                    itemButton[id].GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                }
                else
                {
                    ItemDataBase.InventoryDataBase.itemList[id].currentStack++;
                    weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;
                    itemButton[id].GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                }
            }

            else if(ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.misc)
            {
                if (ItemDataBase.InventoryDataBase.itemList[id].currentStack < 1)
                {
                    InstantiateItem(Misc, MiscImagePosition, id);
                    MiscImagePosition = MoveItemImage(MiscImagePosition);
                }
                else
                {
                    ItemDataBase.InventoryDataBase.itemList[id].currentStack++;
                    weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;
                    itemButton[id].GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                }

            }

            else
            {
                if (ItemDataBase.InventoryDataBase.itemList[id].currentStack < 1)
                {
                    InstantiateItem(Quest, QuestImagePosition, id);
                    QuestImagePosition = MoveItemImage(QuestImagePosition);
                }
                else
                {
                    ItemDataBase.InventoryDataBase.itemList[id].currentStack++;
                    weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;
                    itemButton[id].GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                }

            }

          
            
        }
    }


    Vector3 MoveItemImage(Vector3 ItemImagePos)
    {
           ItemImagePos = ItemImagePos + new Vector3(165, 0, 0);
            
            return ItemImagePos;
        
    }

  

    void InstantiateItem(RawImage ImageType,Vector3 ImagePosition, int id)
    {
        ItemImage.GetComponent<Image>().sprite = ItemDataBase.InventoryDataBase.itemList[id].itemSprite;
        itemButton[id] = Instantiate(ItemImage, ImageType.transform, false) as Button;
        itemButton[id].GetComponent<RectTransform>().localPosition = ImagePosition;
        ItemDataBase.InventoryDataBase.itemList[id].currentStack++;
        weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;

        itemButton[id].GetComponent<ItemEnum>().thisItem = (ItemEnum.Item)id;

        if (ShopButtons.selling == false)
        {
            itemButton[id].onClick.AddListener(delegate { UseItem((int)itemButton[id].GetComponent<ItemEnum>().thisItem); });
        }
        else
        {
            
        }




    }
    
    public void UseItem(int id)
    {
        if (ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.Equipable)
        {
            if (lastId != id)
            {
                ItemEffects.EquipableEffect(itemButton[id]);
                itemButton[id].GetComponentInChildren<Text>().text = "Equipped";
                if (lastId != -1)
                {
                    itemButton[lastId].GetComponentInChildren<Text>().text = "1";
                }
                lastId = id;
            }
        }
          
       else if(ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.Consumable)
       {
            if(ItemDataBase.InventoryDataBase.itemList[id].currentStack > 0 )
            {
                ItemEffects.ConsumableEffect(itemButton[id]);
                ItemDataBase.InventoryDataBase.itemList[id].currentStack--;
                itemButton[id].GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
               
                
            }
            else
            {
              //  itemButton[id].GetComponent<Image>().sprite = null;
                Destroy(itemButton[id].gameObject);
            }
       }
       else if(ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.EquipAndConsume)
       {
            if (ItemDataBase.InventoryDataBase.itemList[id].currentStack > 0)
            {
                if (lastId != id)
                {
                    ItemEffects.EquipAndConsumeEffect(itemButton[id]);
                    itemButton[id].GetComponentInChildren<Text>().text = "Equipped " + ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                    if(lastId != -1)
                    {
                        itemButton[lastId].GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[lastId].currentStack.ToString();
                    }
                    lastId = id;
                }
            }
            else
            {
                Destroy(itemButton[id].gameObject);
            }
       }
       
        else if(ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.misc)
        {
            if(ItemDataBase.InventoryDataBase.itemList[id].currentStack >0)
            {

            }
            else
            {
                Destroy(itemButton[id].gameObject);
            }
        }

        else
        {
            if (ItemDataBase.InventoryDataBase.itemList[id].currentStack > 0)
            {

            }
            else
            {
                Destroy(itemButton[id].gameObject);
            }
        }


        if(ShopButtons.selling == true)
        {
            SellItem((int)itemButton[id].GetComponent<ItemEnum>().thisItem);
            
        }

    }
    
    public void SellItem(int id)
    {
        ShopButtons.money = ShopButtons.money + ItemDataBase.InventoryDataBase.itemList[id].itemValue;
        if(ItemDataBase.InventoryDataBase.itemList[id].currentStack > 1)
        {
            ItemDataBase.InventoryDataBase.itemList[id].currentStack--;
        }
        else
        {
            Destroy(itemButton[id].gameObject);
        }
        print(ShopButtons.money);
        ShopButtons.selling = false;
        
    }
    

}

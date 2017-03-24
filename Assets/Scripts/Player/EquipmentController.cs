using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentController : MonoBehaviour
{
    // the equiptment that is currently spawned in.
    [HideInInspector]
    public List<GameObject> EquipmentOptions = new List<GameObject>();

    // a list of weapons / Distraction objects
    List<Items> Weapons = new List<Items>();

    // A list of items that represent the amount of ammo that the weapons have.
    [HideInInspector]
    public List<Items> Ammo = new List<Items>();

    [HideInInspector]
    public bool InfiniteAmmo = false;

    [HideInInspector]
    public bool SilentSniper = false;

    private int currentEquipment = 0;
    private Transform rightHand;
    private bool isAssigningEquipment = false;
    private UIElements UIE;
    private GameObject gunHolder;

    // Use this for initialization
    void Start ()
    {
        UIE = FindObjectOfType<UIElements>();        
        //ItemDataBase.InventoryDataBase.itemList;
    }

    public void UpdateEquipment()
    {
        if (GetRightHand())
        {
            Weapons = ItemDataBase.InventoryDataBase.itemList.FindAll(x => (x.itemType == Items.TypeofItem.Equipable || x.itemType == Items.TypeofItem.EquipAndConsume) && x.currentStack > 0);

            Ammo = ItemDataBase.InventoryDataBase.itemList.FindAll(x => x.itemType == Items.TypeofItem.misc && x.itemName.Contains("Ammo"));

            GameObject PlayerGun = null;

            if (PlayerController.PC != null)
            {
                if (PlayerController.PC.CurrentWeapon != null)
                {
                    PlayerGun = PlayerController.PC.CurrentWeapon.gameObject;
                }
            }

            //Update Spawned Objects
            //check for objects to be removed.            
            for(int i = 0; i < EquipmentOptions.Count; ++i)
            {
                Items temp = Weapons.Find(x => x.itemName == EquipmentOptions[i].name);
                if (temp == null)
                {
                    Destroy(EquipmentOptions[i]);
                    EquipmentOptions.Remove(EquipmentOptions[i]);
                    --i;
                }
            }

            // add new objects.
            foreach (Items item in Weapons)
            {
                GameObject temp = EquipmentOptions.Find(x => x.name == item.itemName);
                if (temp == null)
                {
                    // spawn and add the object to the list.
                    Object nObject = Resources.Load("Guns/" + item.itemName);
                    if (nObject != null)
                    {
                        temp = (GameObject)Instantiate(nObject);
                        temp.name = item.itemName;
                        AssignEquipment(temp);
                        EquipmentOptions.Add(temp);
                    }
                }

                if (temp != null)
                {
                    BaseGun bg = temp.GetComponent<BaseGun>();
                    // Assign ammo to the equipment.
                    if (item.itemType == Items.TypeofItem.EquipAndConsume)
                    {
                        bg.AmmoReserve = item.currentStack;
                    }
                    else
                    {
                        bg.AmmoReserve = Ammo.Find(x => x.itemName.Contains(item.itemName)).currentStack;
                        if (PlayerController.PC.bleedDamage) bg.OnFireCallbacks.Add(PlayerController.PC.AddBleedDamage);
                        if (PlayerController.PC.Stalker) bg.OnFireCallbacks.Add(PlayerController.PC.AddStalker);
                        if (bg is Sniper) bg.isSilenced = SilentSniper;
                        bg.MagazineSize = (int)(bg.BaseMagazineSize * PlayerController.PC.MagazineSizeMultiplyer);
                        bg.Damage = (int)(bg.BaseDamage * PlayerController.PC.DamageMultiplyer);
                        bg.AudioRange = (int)(bg.BaseAudioRange * PlayerController.PC.NoiseRangeMultiuplyer);
                    }

                    bg.InfiniteAmmo = InfiniteAmmo;
                    bg.reloadUpdate = WeaponStackUpdate;
                    temp.SetActive(false);
                }
                
            }

            if (PlayerGun == null)
            {
                if (EquipmentOptions.Count > 0)
                {
                    currentEquipment = 0;
                    PlayerGun = EquipmentOptions[currentEquipment];
                    PlayerController.PC.CurrentWeapon = PlayerGun.GetComponent<BaseGun>();
                    PlayerController.PC.CurrentWeapon.CallUpdateWeapon();
                    PlayerGun.SetActive(true);
                }
            }
            else
            {
                currentEquipment = EquipmentOptions.IndexOf(PlayerGun);
                PlayerController.PC.CurrentWeapon.CallUpdateWeapon();
                PlayerGun.SetActive(true);
            }
        }
    }

    void WeaponStackUpdate(int ammoReserve, GameObject Weapon)
    {
        Items temp = Weapons.Find(x => x.itemName == Weapon.name);
        if(temp != null)
        {
            if(temp.itemType == Items.TypeofItem.Equipable)
            {
                //find ammo
                Items tempAmmo = Ammo.Find(x => x.itemName.Contains(Weapon.name));
                if(tempAmmo != null)
                {
                    ItemDataBase.InventoryDataBase.itemList.Find(x => x.itemName == tempAmmo.itemName).currentStack = ammoReserve;
                }
            }
            else
            {
                ItemDataBase.InventoryDataBase.itemList.Find(x => x.itemName == temp.itemName).currentStack = ammoReserve;
            }
        }
    }

    public void CycleEquipment()
    {
        UpdateEquipment();
        if (EquipmentOptions.Count > 0)
        {
            if(currentEquipment >= EquipmentOptions.Count)
            {
                currentEquipment = 0;
            }
            else if(currentEquipment < 0)
            {
                currentEquipment = 0;
            }

            EquipmentOptions[currentEquipment].SetActive(false);            

            ++currentEquipment;
            currentEquipment %= EquipmentOptions.Count;

            EquipmentOptions[currentEquipment].SetActive(true);
            PlayerController.PC.CurrentWeapon = EquipmentOptions[currentEquipment].GetComponent<BaseGun>();
            PlayerController.PC.CurrentWeapon.CallUpdateWeapon();
        }
    }

    bool GetGunHolder()
    {
        if(rightHand != null)
        {
            if (gunHolder == null)
            {
                Transform[] handChildren = rightHand.GetComponentsInChildren<Transform>();
                foreach (Transform t in handChildren)
                {
                    if (t.gameObject.name == "GunHolder")
                    {
                        gunHolder = t.gameObject;
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    void AssignEquipment(GameObject equipment)
    {
        if (equipment.GetComponent<BaseGun>() != null)
        {
            if (GetRightHand())
            {
                if (!GetGunHolder())
                {
                    gunHolder = new GameObject();
                    gunHolder.transform.position = Vector3.zero;
                    gunHolder.transform.rotation = Quaternion.identity;
                    gunHolder.transform.SetParent(rightHand, false);
                    gunHolder.name = "GunHolder";
                }

                equipment.transform.position = new Vector3(-0.0901f, -0.0428f, 0.03421f);
                equipment.transform.rotation = Quaternion.Euler(184.443f, 91.212f, -15.281f);
                equipment.transform.SetParent(gunHolder.transform, false);                

                if (UIE != null)
                {
                    equipment.GetComponent<BaseGun>().updateWeapon = UIE.UpdateWeaponStats;
                }
            }
        }
    }

    bool GetRightHand()
    {
        // if the right hand transform is null then look for the right hand.
        if(rightHand == null)
        {
            Transform[] childTransforms = GetComponentsInChildren<Transform>();

            foreach(Transform trans in childTransforms)
            {
                // once we have found the right hand set the right hand variable and return true to say that it exists.
                if(trans.name == "hand_R")
                {
                    rightHand = trans;
                    return true;
                }
            }
            // if we don't find the right hand then return false to show that it couldn't be found.
            return false;
        }
        return true;        
    }

}

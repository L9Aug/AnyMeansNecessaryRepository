using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemEffects : MonoBehaviour {

   public static void ConsumableEffect(Button itemButton)
    {
        if (itemButton.GetComponent<ItemEnum>().thisItem == ItemEnum.Item.BodyArmour)
        {
            //Add armour for player
        }
        else if (itemButton.GetComponent<ItemEnum>().thisItem == ItemEnum.Item.MedKit)
        {
            //Restore some of the players health
            PlayerController.PC.GetComponent<HealthComp>().Hit(-20f);
        }
    }

    public static void EquipableEffect(Button itemButton)
    {
        if (itemButton.GetComponent<ItemEnum>().thisItem == ItemEnum.Item.Pistol)
        {
            //equip pistol model, change ammo
            print(itemButton.GetComponent<ItemEnum>().thisItem);
        }

        else if (itemButton.GetComponent<ItemEnum>().thisItem == ItemEnum.Item.SilencedPistol)
        {
            //equip silenced pistol model etc
        }
        else if (itemButton.GetComponent<ItemEnum>().thisItem == ItemEnum.Item.SniperRifle)
        {
            //Equip sniperrifle
        }
        else if (itemButton.GetComponent<ItemEnum>().thisItem == ItemEnum.Item.TranqPistol)
        {
            //Equip tranq pistol
        }
        else if (itemButton.GetComponent<ItemEnum>().thisItem == ItemEnum.Item.AssualtRifle)
        {
            //Equip assault rifle
        }
        else
        {
            //Equip explosive, stack amount -- on use
        }
    }

    public static void EquipAndConsumeEffect(Button itemButton)
    {
        if(itemButton.GetComponent<ItemEnum>().thisItem == ItemEnum.Item.Bottle)
        {
            //Equip bottle
            print(itemButton.GetComponent<ItemEnum>().thisItem);
        }
        else if(itemButton.GetComponent<ItemEnum>().thisItem == ItemEnum.Item.Rock)
        {
            //Equip Rock
            print(itemButton.GetComponent<ItemEnum>().thisItem);
        }
        else if(itemButton.GetComponent<ItemEnum>().thisItem == ItemEnum.Item.DistractionBox)
        {
            //Equip Distraction box
        }
    }



}

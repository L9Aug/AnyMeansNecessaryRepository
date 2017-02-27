using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/*
Class of functions from console input.
*/

public class ConsoleFunctions
{
    public ConsoleController CC;

    #region Console Functions

    public void Clear()
    {
        CC.LogText.text = "";
    }

    public void GiveAllSkills()
    {
        SkillsController.SC.AddAllSkills();
        CC.PrintToConsole("All skills have been added (May not update on skills screen).");
    }

    public void GiveAllWeapons()
    {
        List<Items> Weapons = ItemDataBase.InventoryDataBase.itemList.FindAll(x => (x.itemType == Items.TypeofItem.Equipable || x.itemType == Items.TypeofItem.EquipAndConsume));
        foreach(Items wep in Weapons)
        {
            wep.currentStack = wep.maxItemStack;
            CC.PrintToConsole("Added " + wep.itemName + " to the player");
        }
        PlayerController.PC.GetComponent<EquipmentController>().UpdateEquipment();
    }

    public void GiveArmour(string ArmourToAdd)
    {
        float ATA;
        if(float.TryParse(ArmourToAdd, out ATA))
        {
            PlayerController.PC.GetComponent<HealthComp>().AddArmour(ATA);
            CC.PrintToConsole("Added " + ATA + " armour to the player.");
        }
        else
        {
            ConversionFailure<float>(ArmourToAdd);
        }
    }

    public void GiveHealth(string HealthToAdd)
    {
        float HTA;
        if (float.TryParse(HealthToAdd, out HTA))
        {
            PlayerController.PC.GetComponent<HealthComp>().ChangeHealth(HTA);
            CC.PrintToConsole("Added " + HTA + " health to the player.");
        }
        else
        {
            ConversionFailure<float>(HealthToAdd);
        }
    }

    public void GiveMoney(string Amount)
    {
        int myMoney;
        if(int.TryParse(Amount, out myMoney))
        {
            ShopButtons.money += myMoney;
            CC.PrintToConsole("Added " + myMoney + " money to the player.");
        }
        else
        {
            ConversionFailure<int>(Amount);
        }
    }

    public void GiveSkill(string SkillName)
    {
        try
        {
            SkillsController.Skills mySkill = (SkillsController.Skills)System.Enum.Parse(typeof(SkillsController.Skills), SkillName, true);
            if (System.Enum.IsDefined(typeof(SkillsController.Skills), mySkill))
            {
                SkillsController.SC.AddNewSkill(mySkill);
                CC.PrintToConsole("Added skill: " + mySkill.ToString() + " to the player.");
            }
            else
            {
                CC.PrintToConsole(mySkill + " is not a valid skill", ConsoleController.LogType.Error);
            }
        }
        catch (System.ArgumentException)
        {
            ConversionFailure<SkillsController.Skills>(SkillName);
        }
    }

    public void GiveSkillpoints(string NumPoints)
    {
        int myPoints;
        if(int.TryParse(NumPoints, out myPoints))
        {
            SkillTree.skillPoints += myPoints;
            SkillTree.updateSkillPoints();
            CC.PrintToConsole("Added " + myPoints + " skill points to the player.");
        }
        else
        {
            ConversionFailure<int>(NumPoints);
        }
    }

    public void GiveWeapon(string WeaponName)
    {
        Items temp = ItemDataBase.InventoryDataBase.itemList.Find(x => (x.itemType == Items.TypeofItem.Equipable || x.itemType == Items.TypeofItem.EquipAndConsume) && x.itemName.Contains(WeaponName));
        if(temp != null)
        {
            ++temp.currentStack;
            temp.currentStack = Mathf.Clamp(temp.currentStack, 0, temp.maxItemStack);
            PlayerController.PC.GetComponent<EquipmentController>().UpdateEquipment();
            CC.PrintToConsole("Added " + temp.itemName + " to the player");
        }
        else
        {
            CC.PrintToConsole("Failed to find Weapon or Distraction " + WeaponName, ConsoleController.LogType.Error);
        }
    }

    public void Help()
    {
        CC.PrintToConsole("<b>Console Help:</b>\nPlease enter commands into the box below.\n\nIf you are unsure what commands are available try:\n\tMethodHelp Methods\n\nIf you would like more information about a method then try:\n\tMethodHelp [MethodName]");
    }

    public void InfiniteAmmo(string Value)
    {
        bool myVal;
        if (ParseBool(Value, out myVal))
        {
            // make infinte ammo happen.
            PlayerController.PC.SetInfiniteAmmo(myVal);
            CC.PrintToConsole("Infinite Ammo " + (myVal ? "ON" : "OFF"));
        }
        else
        {
            ConversionFailure<bool>(Value);
        }
    }

    public void MethodHelp(string MethodName)
    {
        // find the method
        MethodInfo myMethod = typeof(ConsoleFunctions).GetMethod(MethodName, BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.IgnoreCase);
        if (myMethod != null)
        {
            CC.PrintToConsole(myMethod.Name);
            CC.PrintToConsole(GetMethodDesc(myMethod));
        }
        else
        {
            if (MethodName.ToLower() == "methods")
            {
                PrintMethodInfo();
            }
            else
            {
                CC.PrintToConsole("Invalid method : " + MethodName, ConsoleController.LogType.Error);
            }
        }
    }

    public void Undying(string Value)
    {
        bool myVal;
        if(ParseBool(Value, out myVal))
        {
            // make infinite health happen
            PlayerController.PC.Undying = myVal;
            CC.PrintToConsole("Undying " + (myVal ? "ON" : "OFF"));
        }
        else
        {
            ConversionFailure<bool>(Value);
        }
    }

    #endregion

    private void PrintMethodInfo()
    {
        MethodInfo[] myMethods = typeof(ConsoleFunctions).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
        CC.PrintToConsole("Available Methods:\n" + ConvertMethodNamesToString(myMethods) + "In the format:\n[MethodName] [Arg1] [Arg2] ... [ArgN]");
    }

    private string GetMethodDesc(MethodInfo method)
    {
        string retString = "";

        switch (method.Name)
        {
            case "Clear":
                retString += "0 Arguments, Clears the console display.";
                break;

            case "Undying":
                retString += "1 Argument, bool ie. True or False, 1 or 0, Sets if the player dies when they have 0 health.";
                break;

            case "InfiniteAmmo":
                retString += "1 Argument, bool ie. True or False, 1 or 0, Sets if the player has infinite ammo.";
                break;

            case "GiveArmour":
                retString += "1 Argument, float ie. 1 or 23.5, Adds armour to the player. Can be negative.";
                break;

            case "GiveHealth":
                retString += "1 Argument, float ie. 1 or 23.5, Adds health to the player. Can be negative.";
                break;

            default:
            case "Help":
                retString += "0 Arguments, Gives information on how to use the console.";
                break;

            case "MethodHelp":
                retString += "1 Argument, Method Name ie. Help, Provides information about the method.";
                break;

            case "GiveWeapon":
                retString += "1 Argument, Weapon Name ie. SilencedPistol, Gives the player the requested weapon. Doesn't give ammo.";
                break;

            case "GiveAllWeapons":
                retString += "0 arguments, Give all weapons the player. Doesn't give ammo.";
                break;

            case "GiveSkillpoints":
                retString += "1 Argument, int ie. 1 or 20, Gives the player skill points.";
                break;

            case "GiveSkill":
                retString += "1 Argument, Skills Enum ie. Health  or 10, Gives the player the requested skill.";
                break;

            case "GiveAllSkills":
                retString += "0 Arguments, Gives the player all skills.";
                break;

            case "GiveMoney":
                retString += "1 Argument, int ie. 1 or 20, Gives the player money.";
                break;
        }

        return retString;
    }

    private void ConversionFailure<T>(object ConvObj)
    {
        CC.PrintToConsole("Failed to convert '" + ConvObj + "' to " + typeof(T) + ".", ConsoleController.LogType.Error);
    }

    private string ConvertMethodNamesToString(MethodInfo[] methods)
    {
        string retString = "\t<b>Method Name  :  Number of Arguments</b>\n";
        foreach(MethodInfo method in methods)
        {
            retString += "\t" + method.Name + "  :  " + method.GetParameters().Length + "\n";
        }
        return retString;
    }

    private bool ParseBool(string text, out bool result)
    {
        bool myBool;
        if(bool.TryParse(text, out myBool))
        {
            result = myBool;
            return true;
        }
        int myint;
        if(int.TryParse(text, out myint))
        {
            result = (myint != 0 ? true : false);
            return true;
        }
        result = false;
        return false;
    }

}

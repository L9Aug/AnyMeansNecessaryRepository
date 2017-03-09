using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SkillsController : MonoBehaviour
{
    public static SkillsController SC;

    [HideInInspector]
    public int[] SkillValues = new int[30];

    public enum Skills
    {
        // Stealth Skills
        Unseen, SoundRange, CrouchSpeed, EnemyPaths, SnapNeck,
        Stalker, Unseen2, SilencedSniper, SoundRange2, CrouchSpeed2,

        // Run & Gun Skills
        Health, Damage, ExplosiveRange, BleedDamage, ExplosiveTimer,
        Ammo, LargerMag, Health2, Damage2, Armour,

        // Non-Lethal Skills
        KnockedOut, TranqSpeed, GrabSpeed, Conversation, Distraction,
        TranqUses, Conversation2, KnockedOut2, Distraction2, TranqUses2,
    }

    [HideInInspector]
    public List<Skills> CurrentSkills = new List<Skills>();

    public List<Button> SkillButtons = new List<Button>();

    void Start()
    {
        SC = this;
        LoadSkillsFromFile();
    }

    #region File Shenanigans

    public void LoadSkillsFromFile()
    {
        try
        {
            File.Open(Application.persistentDataPath + "\\SkillsData.txt", FileMode.Open).Close();

        }
        catch (FileNotFoundException)
        {
            SaveSkills();
        }

        if (CurrentSkills.Count > 0) CurrentSkills.Clear();

        string[] skills = File.ReadAllText(Application.persistentDataPath + "/SkillsData.txt").Split('\n');

        for (int i = 0; i < 30; ++i)
        {
            SkillHolder tempHolder = new SkillHolder(skills[i].Split(','));
            if (tempHolder.Unlocked)
            {
                // Deactivate corrosponding button
                // SkillButtons[i]
                if (tempHolder.NeedToApply)
                {
                    AddNewSkill((Skills)i, true);
                }
            }
        }

    }

    public void SaveSkills()
    {
        List<string> data = new List<string>();
        for (int i = 0; i < 30; ++i)
        {
            data.Add(TestForSkill((Skills)i).ToString() + "," + TestForApplied(i).ToString());
        }
        File.WriteAllLines(Application.persistentDataPath + "/SkillsData.txt", data.ToArray());
    }

    int TestForSkill(Skills skill)
    {
        int ret = 0;
        for (int i = 0; i < CurrentSkills.Count; ++i)
        {
            if (CurrentSkills[i] == skill)
            {
                ret = 1;
                break;
            }
        }
        return ret;
    }

    int TestForApplied(int ID)
    {
        return 0;
    }

    #endregion

    public void ResetButtons()
    {
        // unlcok all buttons. (mainly for checkpoint stuff).
    }

    /// <summary>
    /// If the player loses all skill benifits then call this to re-add all skills in the current skills list.
    /// </summary>
    void ActivateCurrentSkills()
    {
        foreach (Skills skill in CurrentSkills)
        {
            AddNewSkill(skill, false);
        }
    }

    public void AddNewSkill(Skills newSkill, bool AddToCurrentSkillsList = true)
    {
        switch (newSkill)
        {
            case Skills.Unseen:
                IncreaseTimeToDetect(SkillValues[(int)newSkill]);
                break;

            case Skills.Unseen2:
                IncreaseTimeToDetect(SkillValues[(int)newSkill]);
                break;

            case Skills.SoundRange:
                DecreaseNoiseRange(SkillValues[(int)newSkill]);
                break;

            case Skills.SoundRange2:
                DecreaseNoiseRange(SkillValues[(int)newSkill]);
                break;

            case Skills.CrouchSpeed:
                IncreaseCrouchSpeed(SkillValues[(int)newSkill]);
                break;

            case Skills.CrouchSpeed2:
                IncreaseCrouchSpeed(SkillValues[(int)newSkill]);
                break;

            case Skills.EnemyPaths:
                DisplayEnemyPaths();
                break;

            case Skills.SnapNeck:
                AddSnapNeck();
                break;

            case Skills.Stalker:
                AddStalker(SkillValues[(int)newSkill]);
                break;

            case Skills.SilencedSniper:
                AddSilencedSniper();
                break;

            case Skills.Health:
            case Skills.Health2:
                IncreasePlayerHealth(SkillValues[(int)newSkill]);
                break;

            case Skills.Damage:
                IncreasePlayerDamage(SkillValues[(int)newSkill]);
                break;

            case Skills.Damage2:
                IncreasePlayerDamage(SkillValues[(int)newSkill]);
                break;

            case Skills.ExplosiveRange:
                IncreaseExplosiveRange(SkillValues[(int)newSkill]);
                break;

            case Skills.BleedDamage:
                AddBleedDamage();
                break;

            case Skills.ExplosiveTimer:
                ReduceExplosiveTimers(SkillValues[(int)newSkill]);
                break;

            case Skills.Ammo:
                IncreaseAmmoReserve(SkillValues[(int)newSkill]);
                break;

            case Skills.LargerMag:
                IncreaseMagazineSizes(SkillValues[(int)newSkill]);
                break;

            case Skills.Armour:
                AddArmour(50);
                break;

            case Skills.KnockedOut:
                IncreaseKnockOutTime(SkillValues[(int)newSkill]);
                break;

            case Skills.KnockedOut2:
                IncreaseKnockOutTime(SkillValues[(int)newSkill]);
                break;

            case Skills.TranqSpeed:
                ReduceTranqSpeed(SkillValues[(int)newSkill]);
                break;

            case Skills.GrabSpeed:
                AddGrabSpeed();
                break;

            case Skills.Conversation:
                ImproveConversation(SkillValues[(int)newSkill]);
                break;

            case Skills.Conversation2:
                ImproveConversation(SkillValues[(int)newSkill]);
                break;

            case Skills.Distraction:
                IncreaseDistractionTime(SkillValues[(int)newSkill]);
                break;

            case Skills.Distraction2:
                IncreaseDistractionTime(SkillValues[(int)newSkill]);
                break;

            case Skills.TranqUses:
                AddTranqUses(SkillValues[(int)newSkill]);
                break;

            case Skills.TranqUses2:
                AddTranqUses(SkillValues[(int)newSkill]);
                break;

            default:
                break;
        }

        if (AddToCurrentSkillsList)
        {
            CurrentSkills.Add(newSkill);
        }
    }

    #region Skill Functions

    void IncreaseTimeToDetect(float Amount)
    {
        FieldOfView.detectionTimer += Amount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Amount">Amount as a percentage</param>
    void DecreaseNoiseRange(float Amount)
    {
        // Waiting for the AI to detect through audio to know what to change.
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Amount">Amount as a percentage</param>
    void IncreaseCrouchSpeed(float Amount)
    {
        PlayerController.PC.GetComponent<FirstPersonMovement>().movementSettings.CrouchMultiplier *= (1 + (Amount / 100f));
    }

    void DisplayEnemyPaths()
    {
        DisplayAIPaths.DAP.DisplayPaths();
    }

    void AddSnapNeck()
    {
        PlayerController.PC.SnapNeck = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Amount">Amount as a percentage</param>
    void AddStalker(float Amount)
    {
        PlayerController.PC.Stalker = true;
        EquipmentController EC = PlayerController.PC.GetComponent<EquipmentController>();
        foreach (GameObject go in EC.EquipmentOptions)
        {
            BaseGun bg = go.GetComponent<BaseGun>();
            if (bg != null)
            {
                bg.OnFireCallbacks.Add(PlayerController.PC.AddStalker);
            }
        }
    }

    void AddSilencedSniper()
    {

    }

    void IncreasePlayerHealth(float Amount)
    {
        PlayerController.PC.GetComponent<HealthComp>().ChangeMaxHealth(PlayerController.PC.GetComponent<HealthComp>().MaxHealth + 50);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Amount">Amount as a percentage</param>
    void IncreasePlayerDamage(float Amount)
    {
        PlayerController.PC.DamageMultiplyer += Amount / 100f;
        EquipmentController EC = PlayerController.PC.GetComponent<EquipmentController>();
        foreach (GameObject go in EC.EquipmentOptions)
        {
            BaseGun bg = go.GetComponent<BaseGun>();
            if (bg != null)
            {
                bg.BaseDamage = (int)(bg.BaseDamage * PlayerController.PC.DamageMultiplyer);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Amount">Amount as a percentage</param>
    void IncreaseExplosiveRange(float Amount)
    {

    }

    void AddBleedDamage()
    {
        PlayerController.PC.bleedDamage = true;
        EquipmentController EC = PlayerController.PC.GetComponent<EquipmentController>();
        foreach (GameObject go in EC.EquipmentOptions)
        {
            BaseGun bg = go.GetComponent<BaseGun>();
            if (bg != null)
            {
                bg.OnFireCallbacks.Add(PlayerController.PC.AddBleedDamage);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Amount">Amount as a percentage</param>
    void ReduceExplosiveTimers(float Amount)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Amount">Amount as a percentage</param>
    void IncreaseAmmoReserve(float Amount)
    {
        List<Items> ammoItems = ItemDataBase.InventoryDataBase.itemList.FindAll(x => x.itemType == Items.TypeofItem.misc && x.itemName.Contains("Ammo"));

        foreach (Items item in ammoItems)
        {
            item.maxItemStack += (int)((Amount / 100f) * item.maxItemStack);
        }

        PlayerController.PC.GetComponent<EquipmentController>().Ammo = ammoItems;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Amount">Amount as a percentage</param>
    void IncreaseMagazineSizes(float Amount)
    {
        PlayerController.PC.MagazineSizeMultiplyer += Amount / 100f;
        EquipmentController EC = PlayerController.PC.GetComponent<EquipmentController>();
        foreach (GameObject go in EC.EquipmentOptions)
        {
            BaseGun bg = go.GetComponent<BaseGun>();
            if (bg != null)
            {
                bg.MagazineSize = (int)(bg.BaseMagazineSize * PlayerController.PC.MagazineSizeMultiplyer);
            }
        }
    }

    void AddArmour(int Amount)
    {
        PlayerController.PC.GetComponent<HealthComp>().AddArmour(Amount);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Amount">Amount as a percentage</param>
    void IncreaseKnockOutTime(float Amount)
    {
        Base_Enemy.stunTimer *= (1 + (Amount / 100f));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Amount">Amount as a percentage</param>
    void ReduceTranqSpeed(float Amount)
    {

    }

    void AddGrabSpeed()
    {
        PlayerController.PC.GrabSpeed = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Amount">Amount as a percentage</param>
    void ImproveConversation(float Amount)
    {

    }

    void IncreaseDistractionTime(float Amount)
    {
        Distraction.objectDestroyInSeconds += Amount;
    }

    void AddTranqUses(int Amount)
    {
        Items tranqAmmo = ItemDataBase.InventoryDataBase.itemList.Find(x => x.itemType == Items.TypeofItem.misc && x.itemName.Contains("TaserAmmo"));
        tranqAmmo.currentStack = Mathf.Clamp(tranqAmmo.currentStack + Amount, 0, tranqAmmo.maxItemStack);
        PlayerController.PC.GetComponent<EquipmentController>().UpdateEquipment();
    }

    #endregion

    public void AddAllSkills()
    {
        for (int i = 0; i < 30; ++i)
        {
            if (!CurrentSkills.Contains((Skills)i))
            {
                AddNewSkill((Skills)i);
            }
        }
    }

}

public class SkillHolder
{
    public bool Unlocked;
    public bool NeedToApply;

    public SkillHolder(string[] SkillFromText)
    {
        Unlocked = int.Parse(SkillFromText[0]) == 1 ? true : false;
        NeedToApply = int.Parse(SkillFromText[1]) == 1 ? true : false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SkillsController))]
public class SkillsControllerEditor : Editor
{
    static bool areSkillValuesVisible;
    static bool displayCurrentSkills;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DisplaySkillValues();

        DisplayCurrentSkills();

        SortButtonsList();
    }

    void DisplayCurrentSkills()
    {
        displayCurrentSkills = EditorGUILayout.Foldout(displayCurrentSkills, "Current Skills");

        if (displayCurrentSkills)
        {
            foreach (SkillsController.Skills skill in ((SkillsController)target).CurrentSkills)
            {
                EditorGUILayout.LabelField(skill.ToString());
            }
        }
    }

    void DisplaySkillValues()
    {
        areSkillValuesVisible = EditorGUILayout.Foldout(areSkillValuesVisible, "Skill Values");

        if (areSkillValuesVisible)
        {
            SkillsController mySkillsCont = (SkillsController)target;
            for (int i = 0; i < 30; ++i)
            {
                mySkillsCont.SkillValues[i] = EditorGUILayout.IntField(((SkillsController.Skills)i).ToString(), mySkillsCont.SkillValues[i]);
            }
        }
    }

    void SortButtonsList()
    {
        if (GUILayout.Button("Sort Skill Buttons"))
        {
            SkillsController mySkillsCont = (SkillsController)target;
            List<Button> SortedButtons = new List<Button>();
            for (int i = 0; i < 30; ++i)
            {
                SortedButtons.Add(mySkillsCont.SkillButtons.Find(x => x.GetComponent<SkillTree>().Skill == (SkillsController.Skills)(i)));
            }
            mySkillsCont.SkillButtons = SortedButtons;
        }
    }

}
#endif
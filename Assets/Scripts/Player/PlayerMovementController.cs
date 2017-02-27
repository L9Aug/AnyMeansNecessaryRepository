using UnityEngine;
using System.Collections; 
using System.Collections.Generic;

public class PlayerMovementController : MonoBehaviour {

    /// <summary>
    /// Player Movement State Machine
    /// </summary>
    public SM.StateMachine PMSM;

    public bool BeginTakedown = false;
    public bool BeginLooting = false;
    public FirstPersonMovement m_FPM;
    private Animator anim;

    public static bool PlayerCrouching = false;

    public Transform TakedownTarget;
    public GameObject LootTarget;

    private Vector3 TargetPosition;
    private Quaternion TargetRotation;

    private UIElements uiElements;

    private float LootTimer;
    private float LootTime = 2;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        uiElements = FindObjectOfType<UIElements>();
        SetupStateMachine();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //PMSM.SMUpdate();
	}

    bool AnimTest()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        return (anim != null) ? true : false;
    }

    #region PMSM Functions

    void TakedownStateUpdate()
    {
        if(TakedownTarget != null)
        {
            TargetPosition = TakedownTarget.position + (Quaternion.FromToRotation(Vector3.forward, TakedownTarget.forward) * new Vector3(-0.14f, 0, -1.183f));
            TargetRotation = TakedownTarget.rotation;

            UpdatePlayerTransform();
        }
    }

    void UpdatePlayerTransform()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, Time.deltaTime);
    }

    void EndTakedownState()
    {
        if(TakedownTarget != null)
        {
            TakedownTarget.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
        uiElements.xpGain(25);
    }

    public void BeginFirstPersonState()
    {
        m_FPM.enabled = true;
    }

    public void EndFirstPersonState()
    {
        m_FPM.enabled = false;
    }

    void BeginTakedownTransitionFunc()
    {
        BeginTakedown = false;        
    }

    void EndTakedownTransitionFunc()
    {
        
        BeginTakedown = false;
        TakedownTarget = null;
    }

    void BeginLootingFunc()
    {
        LootTimer = 0;
        BeginLooting = false;
    }

    void LootingUpdateFunc()
    {
        LootTimer += Time.deltaTime;
        if(LootTarget != null)
        {
            TargetPosition = LootTarget.transform.position + (Quaternion.FromToRotation(Vector3.forward, LootTarget.transform.forward) * new Vector3(-0.14f, 0, -1.183f));
            TargetRotation = LootTarget.transform.rotation;

            UpdatePlayerTransform();
        }
    }

    void EndLootingFunc()
    {
        if(LootTarget != null)
        {
            LootComponent tempLootObj = LootTarget.GetComponent<LootComponent>();
            if(tempLootObj != null)
            {
                List<LootableItem> loot = tempLootObj.GetLoot();
                ConsoleController.CC.PrintToConsole("<b>Loot Obtained:</b>");
                foreach (LootableItem item in loot)
                {
                    switch (item.name) 
                    {
                        case "Money":
                            ShopButtons.money += item.Quantity;
                            ConsoleController.CC.PrintToConsole("Added Money: " + item.Quantity);
                            break;

                        case "Distractions":
                            List<Items> tempItems = ItemDataBase.InventoryDataBase.itemList.FindAll(x => x.itemType == Items.TypeofItem.EquipAndConsume);
                            tempItems[Random.Range(0, tempItems.Count)].itemValue += item.Quantity;
                            ConsoleController.CC.PrintToConsole("Added Distractions: " + item.Quantity);
                            break;

                        case "Armour":
                            GetComponent<HealthComp>().AddArmour(item.Quantity);
                            ConsoleController.CC.PrintToConsole("Added Armour: " + item.Quantity);
                            break;

                        case "Pistol Ammunition":
                            //might need fixing
                            Items temp = ItemDataBase.InventoryDataBase.itemList.Find(x => x.itemName.Contains("Pistol") && x.itemType == Items.TypeofItem.misc);
                            temp.currentStack = Mathf.Clamp(temp.currentStack + item.Quantity, 0, temp.maxItemStack);
                            
                            ConsoleController.CC.PrintToConsole("Added Pistol Ammunition: " + item.Quantity);
                            break;

                        case "Assualt Rifle Ammunition":
                            // might need fixing
                            Items temp2 = ItemDataBase.InventoryDataBase.itemList.Find(x => x.itemName.Contains("Assualt") && x.itemType == Items.TypeofItem.misc);
                            temp2.currentStack = Mathf.Clamp(temp2.currentStack + item.Quantity, 0, temp2.maxItemStack);
                            ConsoleController.CC.PrintToConsole("Added Assualt Rifle Ammunition: " + item.Quantity);
                            break;

                        case "Sniper Rifle Ammunition":
                            // might need fixing
                            Items temp3 = ItemDataBase.InventoryDataBase.itemList.Find(x => x.itemName.Contains("Sniper") && x.itemType == Items.TypeofItem.misc);
                            temp3.currentStack = Mathf.Clamp(temp3.currentStack + item.Quantity, 0, temp3.maxItemStack);
                            ConsoleController.CC.PrintToConsole("Added Sniper Rifle Ammunition: " + item.Quantity);
                            break;

                        default:

                            break;
                    }
                }
                PlayerController.PC.GetComponent<EquipmentController>().UpdateEquipment();
                tempLootObj.HasBeenLooted = true;
            }
        }
    }

    #region PMSM Condition Functions

    bool TestAnimTag()
    {
        if (AnimTest())
        {
            return (!anim.GetCurrentAnimatorStateInfo(2).IsTag("InTakedown") && anim.GetCurrentAnimatorStateInfo(2).normalizedTime < 0.5f) || (TakedownTarget == null);
        }
        else
        {
            return false;
        }
    }

    bool TakedownTest()
    {
        return BeginTakedown && (TakedownTarget != null);
    }

    bool LootTest()
    {
        return BeginLooting && (LootTarget != null);
    }

    bool EndLootTest()
    {
        return (LootTarget == null) || (LootTimer >= LootTime);
    }

    #endregion

    void SetupStateMachine()
    {
        // Configure Conditions For Transitions
        Condition.BoolCondition BeginTakedownCond = new Condition.BoolCondition();
        BeginTakedownCond.Condition = TakedownTest;

        Condition.BoolCondition EndTakedownCond = new Condition.BoolCondition();
        EndTakedownCond.Condition = TestAnimTag;

        Condition.BoolCondition BeginLootingCond = new Condition.BoolCondition();
        BeginLootingCond.Condition = LootTest;

        Condition.BoolCondition EndLootingCond = new Condition.BoolCondition();
        EndLootingCond.Condition = EndLootTest;

        // Create Transistions
        SM.Transition BeginTakedown = new SM.Transition("Begin Takedown", BeginTakedownCond, BeginTakedownTransitionFunc);
        SM.Transition EndTakedown = new SM.Transition("End Takedown", EndTakedownCond, EndTakedownTransitionFunc);
        SM.Transition BeginLooting = new SM.Transition("Begin Looting", BeginLootingCond);
        SM.Transition EndLooting = new SM.Transition("End Looting", EndLootingCond);

        // Create States
        SM.State TakedownState = new SM.State("Takedown",
            new List<SM.Transition>() { EndTakedown },
            null,
            new List<SM.Action>() { TakedownStateUpdate },
            new List<SM.Action>() { EndTakedownState });

        SM.State FirstPersonState = new SM.State("Movement",
            new List<SM.Transition>() { BeginLooting, BeginTakedown },
            new List<SM.Action>() { BeginFirstPersonState },
            null,
            new List<SM.Action>() { EndFirstPersonState });

        SM.State LootingState = new SM.State("Looting",
            new List<SM.Transition>() { EndLooting },
            new List<SM.Action>() { BeginLootingFunc },
            new List<SM.Action>() { LootingUpdateFunc },
            new List<SM.Action>() { EndLootingFunc });

        // Assign Target States to Transitions
        BeginTakedown.SetTargetState(TakedownState);
        EndTakedown.SetTargetState(FirstPersonState);
        BeginLooting.SetTargetState(LootingState);
        EndLooting.SetTargetState(FirstPersonState);

        // Create Machine
        PMSM = new SM.StateMachine(null, FirstPersonState, TakedownState, LootingState);

        // Start the State Machine
        PMSM.InitMachine();
    }

    #endregion

}

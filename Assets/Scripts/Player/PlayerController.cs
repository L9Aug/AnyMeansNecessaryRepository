using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Player Controller
    /// </summary>
    public static PlayerController PC;

    public float TakedownFOV;
    public BaseGun CurrentWeapon;
    public bool isADS;

    public bool Undying = false;
    public bool InfiniteAmmo = false;

    public bool bleedDamage;
    public bool Stalker;
    public bool SnapNeck = false;
    public bool GrabSpeed = false;
    public float MagazineSizeMultiplyer = 1;
    public float DamageMultiplyer = 1;

    private List<GameObject> AIInRange = new List<GameObject>(); //list of AI that are in takedown range
    private bool CanTakedown = false;
    private bool CanLoot = false;
    private Animator anim;
    private Camera PlayerCam;
    private PauseMenu pauseMenu;
    private PlayerMovementController PMC;
    private EquipmentController equipmentController;
    private CameraController cameraController;
    private bool CanShoot = false;

    /// <summary>
    /// Player State Machine
    /// </summary>
    public SM.StateMachine PSM ;

    private bool Dying = false;

    public bool Revived = false;

    private float DeathTimer = 0;
    private float DeathLength = 5;

    // Use this for initialization
    void Start()
    {
        PC = this;
        AnimTest();
        PlayerCam = Camera.main;
        pauseMenu = FindObjectOfType<PauseMenu>();
        PMC = GetComponent<PlayerMovementController>();
        equipmentController = GetComponent<EquipmentController>();
        cameraController = GetComponent<CameraController>();
        equipmentController.UpdateEquipment();
        SetupStateMachine();
    }

    public void SetInfiniteAmmo(bool doWe)
    {
        equipmentController.InfiniteAmmo = doWe;
        foreach (GameObject go in equipmentController.EquipmentOptions)
        {
            BaseGun gun = go.GetComponent<BaseGun>();
            if (gun != null)
            {
                gun.InfiniteAmmo = doWe;
            }
        }
    }

    bool AnimTest()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        return (anim != null);
    }

    public void HealthCheck(float Health, float HealthChanged, float armour)
    {
        if (Health <= 0 && !Undying)
        {
            Dying = true;
        }
    }

    void WeaponChecks()
    {
        if (Time.timeScale > 0.01 && PMC.PMSM.GetCurrentState() == "Movement")
        {
            if (CurrentWeapon != null)
            {
                if (CurrentWeapon is Sniper)
                {
                    isADS = Input.GetButton("Aim");
                    ((Sniper)CurrentWeapon).isADS = isADS;
                }

                if (Input.GetButtonDown("Fire"))
                {
                    SetCanShoot(true);
                }

                if (Input.GetButton("Fire") && CanShoot)
                {
                    // 1 << 10 is the AI layer.
                    if (CurrentWeapon.Fire(GunTarget, 1 << 10, 0, false, true))
                    {
                        if (AnimTest())
                        {
                            anim.SetTrigger("Fire");
                        }
                    }
                }

                if (Input.GetButtonUp("Fire"))
                {
                    SetCanShoot(false);
                }

                if (Input.GetButton("Reload"))
                {
                    CurrentWeapon.Reload();
                }
            }

            if (Input.GetButtonDown("CycleEquipment"))
            {
                equipmentController.CycleEquipment();
            }
        }
    }

    public void SetCanShoot(bool canShoot)
    {
        CanShoot = canShoot;
    }

    public void AddBleedDamage(GameObject projectile)
    {
        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.CollisionCallbacks.Add(AddBleedDamage);
        }
    }

    public void AddBleedDamage(Collider other)
    {
        other.gameObject.AddComponent<BleedDamage>();
    }

    public void AddStalker(GameObject projectile)
    {
        projectile.AddComponent<Stalker>();
    }

    Vector3 GunTarget()
    {
        Ray CameraRay = PlayerCam.ScreenPointToRay(new Vector3(0.5f * PlayerCam.pixelWidth, 0.5f * PlayerCam.pixelHeight, 0));
        RaycastHit hit;
        LayerMask mask = 1 << 8; // 1 << 8 is the player layer.
        mask = ~mask; // flip the mask to hit all but the player.
        Debug.DrawRay(CameraRay.origin, CameraRay.direction * 10f, Color.red, 1);
        if (Physics.Raycast(CameraRay, out hit, 1000f, mask))
        {
            return hit.point;
        }
        return (CameraRay.origin + (CameraRay.direction * 1000f));
    }

    void InteractionChecks()
    {
        UIElements.ContextText.GetComponent<UnityEngine.UI.Text>().text = "";
        GameObject interactionTarget = CheckTakedownFOV();
        LootingCheck(interactionTarget);
        TakedownCheck(interactionTarget);
    }

    void LootingCheck(GameObject LootingTarget)
    {
        if (LootingTarget != null)
        {
            if (LootingTarget.GetComponent<LootComponent>() != null && LootingTarget.GetComponent<Base_Enemy>() != null)
            {
                if (LootingTarget.GetComponent<Base_Enemy>()._state == Base_Enemy.State.Dead)
                {
                    // can loot message
                    if (!LootingTarget.GetComponent<LootComponent>().HasBeenLooted) UIElements.ContextText.GetComponent<UnityEngine.UI.Text>().text = "Press 'f' to loot.";
                    if (Input.GetButtonDown("Interact") && PMC.PMSM.CurrentState.Name != "Takedown")
                    {
                        PerformLooting(LootingTarget);
                    }
                }
            }
        }
    }

    void TakedownCheck(GameObject TakedownTarget)
    {
        if (TakedownTarget != null && !Enemy_Patrol.detected)
        {
            if (TakedownTarget.GetComponent<Base_Enemy>() != null)
            {
                if (TakedownTarget.GetComponent<Base_Enemy>()._state != Base_Enemy.State.Dead)
                {
                    // can takedown message.
                    UIElements.ContextText.GetComponent<UnityEngine.UI.Text>().text = "Press 'f' to Takedown.";
                    if (Input.GetButtonDown("Interact") && PMC.PMSM.CurrentState.Name != "Looting")
                    {
                        PerformTakedown(TakedownTarget);
                    }
                }
            }
        }
    }

    void PerformLooting(GameObject Target)
    {
        LootComponent tempComp = Target.GetComponent<LootComponent>();
        if (tempComp != null)
        {
            if (!tempComp.HasBeenLooted)
            {
                // put the player into the looting animation

                // Put the player into the looting state.
                PMC.BeginLooting = true;
                PMC.LootTarget = Target;
            }
        }
    }

    void PerformTakedown(GameObject Target)
    {
        if (Target.GetComponent<Base_Enemy>() != null)
        {
            float Takedown = Random.Range(0, 2); // select at random the takedown to use
            Animator TargetAnim = Target.GetComponent<Animator>(); // get the animator of the AI

            //Target.GetComponent<Base_Enemy>().setState(SnapNeck ? Base_Enemy.State.Dead : Base_Enemy.State.Stunned); // turn off the AI
            Target.GetComponent<Base_Enemy>().setState(SnapNeck ? Base_Enemy.State.Dead : Base_Enemy.State.Dead); // turn off the AI
            if (!GrabSpeed)
            {
                Target.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                Target.GetComponent<NavMeshAgent>().speed = 0;

                TargetAnim.applyRootMotion = false;
                //Anim.applyRootMotion = true;

                TargetAnim.SetTrigger("Takedown"); // trigger the animations for both the AI and the Player.
                if (AnimTest())
                {
                    anim.SetTrigger("Takedown");
                }

                //Put the player into the takedown state
                PMC.TakedownTarget = Target.transform;
                PMC.BeginTakedown = true;
            }
        }
    }

    GameObject CheckTakedownFOV()
    {
        if (AIInRange.Count > 0)
        {
            GameObject returnObject = null;

            List<GameObject> AIInFoV = new List<GameObject>();

            for (int i = 0; i < AIInRange.Count; ++i) //search for the ai that is most in front of the player.
            {
                Vector3 DirToTarget = AIInRange[i].transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, DirToTarget);
                if (angle < (TakedownFOV / 2f))
                {
                    AIInFoV.Add(AIInRange[i]);
                }
            }

            float SmallestDist = 3;

            for (int i = 0; i < AIInFoV.Count; ++i) // of the AI that are in the FoV of the player find the one closest to the player.
            {
                float testDist = Vector3.Distance(transform.position, AIInFoV[i].transform.position);
                if (testDist < SmallestDist)
                {
                    SmallestDist = testDist;
                    returnObject = AIInFoV[i];
                }
            }

            return returnObject;
        }

        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Base_Enemy>() != null)
        {
            if (!AIInRange.Contains(other.gameObject))
            {
                AIInRange.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Base_Enemy>() != null)
        {
            if (AIInRange.Contains(other.gameObject))
            {
                AIInRange.Remove(other.gameObject);
            }
        }
    }

    private void ActiveUpdate()
    {
        WeaponChecks();
        InteractionChecks();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (CurrentWeapon != null)
        {
            if (layerIndex == 1 && AnimTest() && PMC.PMSM.GetCurrentState() == "Movement")
            {
                anim.speed = CurrentWeapon.animSpeed;

                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, CurrentWeapon.RightHandPositionWeight);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, CurrentWeapon.LeftHandPositionWeight);
                anim.SetIKPosition(AvatarIKGoal.RightHand, CurrentWeapon.RightHand.position);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, CurrentWeapon.LeftHand.position);

                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, CurrentWeapon.RightHandRotationWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, CurrentWeapon.LeftHandRotationWeight);
                anim.SetIKRotation(AvatarIKGoal.RightHand, CurrentWeapon.RightHand.rotation);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, CurrentWeapon.LeftHand.rotation);
            }
            else if (PMC.PMSM.GetCurrentState() == "Takedown" && AnimTest())
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }

    private void BeginDead()
    {
        DeathTimer = DeathLength;
    }

    private void DeadUpdate()
    {
        DeathTimer -= Time.deltaTime;
        if (DeathTimer <= 0)
        {
            pauseMenu.reloadCheckpoint();
        }
    }

    private void EndDead()
    {
        Dying = false;
    }

    private void DyingTransFunc()
    {
        if (AnimTest())
        {
            anim.SetBool("Dying", true);
            //Dying = false;
        }
    }

    private void RevivedTransFunc()
    {
        if (AnimTest())
        {
            anim.SetBool("Dying", false);
        }
        Revived = false;
    }

    private bool DyingCheck()
    {
        return Dying;
    }

    private bool RevivedCheck()
    {
        return Revived;
    }

    void SetupStateMachine()
    {
        // Configure conditions for transitions.
        Condition.BoolCondition IsDying = new Condition.BoolCondition();
        IsDying.Condition = DyingCheck;

        Condition.BoolCondition IsRevived = new Condition.BoolCondition();
        IsRevived.Condition = RevivedCheck;

        // Create Transitions.
        SM.Transition dying = new SM.Transition("Dying", IsDying, DyingTransFunc);
        SM.Transition revived = new SM.Transition("Revived", IsRevived, RevivedTransFunc);

        // Create States.
        SM.State Active = new SM.State("Active",
            new List<SM.Transition>() { dying },
            new List<SM.Action>() { PMC.BeginFirstPersonState },
            new List<SM.Action>() { PMC.PMSM.SMUpdate, ActiveUpdate },
            new List<SM.Action>() { PMC.EndFirstPersonState });

        SM.State Dead = new SM.State("Dead",
            new List<SM.Transition>() { revived },
            new List<SM.Action>() { BeginDead },
            new List<SM.Action>() { DeadUpdate },
            new List<SM.Action>() { EndDead });

        // Assign target states to transitions.
        dying.SetTargetState(Dead);
        revived.SetTargetState(Active);

        // Create Machine.
        PSM = new SM.StateMachine(null, Active, Dead);

        // Initialize the machine.
        PSM.InitMachine();
    }

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

    public SM.StateMachine CameraTypeStates;
    public SM.StateMachine CameraModeStates;

    Camera m_Camera;

    public Vector3 FirstPersonPosition;
    public float ThirdPersonCameraDistance;
    public Vector3 ThirdPersonAnchor;
    public float StandardFOV = 60;
    public float ADSFOV = 5;
    public float ADSSensitivity = 0.1f;
    public float DefaultSensitivity = 2;

    public UMARecipeBase FirstPersonRecipe;
    public UMARecipeBase ThirdPersonRecipe;

    public PauseMenu pauseMenu;

    private UMA.UMAData umaData;
    private UMA.SlotData[] slotData;
    private UMA.SlotData[] firstPersonSlots;

    Transform CameraRootObject;

    bool CamModeChange = false;

    float HorizontalAngle = 0;
    float VerticalAngle = -(Mathf.PI / 2);

    bool InvertMouse = false;

    float CamChangeLerpTimer = 0;
    float CamChangeLerpDir = -1;

    Vector3 ThirdPersonTargetPosition;
    Quaternion ThirdPersonTargetRotation;
    Vector3 FirstPersonTargetPosition;
    Quaternion firstPersonTargetRotation;

    bool PunishMisAlignment = true;

    SM.State ThirdPersonState;
    SM.State FirstPersonState;
    SM.State ReturnState;
    SkinnedMeshRenderer[] playerSkinnedmeshRenderers;
    MeshRenderer[] playerMeshRenderers;

    public GameObject PlayerMesh;
    Quaternion ThirdPersonPlayerMeshRot;

	// Use this for initialization
	void Start ()
    {
        m_Camera = Camera.main;
        umaData = GetComponent<UMA.UMAData>();

        GameObject CameraRootObj = new GameObject("Camera Root Obj");
        CameraRootObject = CameraRootObj.transform;
        CameraRootObj.transform.SetParent(transform, false);
        CameraRootObject.localPosition = ThirdPersonAnchor;
        m_Camera.transform.SetParent(transform, false);

        SetupStateMachine();
        //CamModeChange = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // moved to pause menu.
        // CameraModeStates.SMUpdate();
	}

    void TestForCamModeChange()
    {
        if(Input.GetButtonDown("CamSwitch"))
        {
            if (GetComponent<PlayerMovementController>().PMSM.GetCurrentState() == "Movement" && GetComponent<PlayerController>().PSM.GetCurrentState() == "Active")
            {
                CamModeChange = true;
            }
        }
    }

    void SetCameraPosition()
    {
        float CamLerpSpeed = 5;
        CamChangeLerpTimer += Time.deltaTime * CamChangeLerpDir * CamLerpSpeed;

        CamChangeLerpTimer = Mathf.Clamp(CamChangeLerpTimer, 0, 1);

        m_Camera.transform.position = Vector3.Lerp(ThirdPersonTargetPosition, FirstPersonTargetPosition, CamChangeLerpTimer);
        //m_Camera.transform.rotation = Quaternion.Lerp(ThirdPersonTargetRotation, m_Camera.transform.rotation, CamChangeLerpTimer);
    }

    void UpdateThirdPersonCameraPosition()
    {
        ThirdPersonTargetPosition = new Vector3(
            ThirdPersonCameraDistance * Mathf.Sin(VerticalAngle) * Mathf.Sin(HorizontalAngle),
            ThirdPersonCameraDistance * Mathf.Cos(VerticalAngle),
            ThirdPersonCameraDistance * Mathf.Sin(VerticalAngle) * Mathf.Cos(HorizontalAngle)) + CameraRootObject.position;
        ThirdPersonTargetRotation = Quaternion.identity;
    }

    void UpdateFirstPersonCameraPosition()
    {
        Vector3 CrouchedDisplacement = Vector3.zero;
        if (PlayerMovementController.PlayerCrouching)
        {
            CrouchedDisplacement = GetCrouchedDisplacementVector(FirstPersonPosition);
        }

        FirstPersonTargetPosition = transform.position + FirstPersonPosition + CrouchedDisplacement;
        //m_Camera.GetComponent<FirstPersonHeadBob>().m_OriginalCameraPosition = m_Camera.transform.position;
    }

    Vector3 GetCrouchedDisplacementVector(Vector3 AnchorPoint)
    {
        return new Vector3(0, (transform.position.y - (transform.position.y + AnchorPoint.y)) / 2f, 0);
    }

    #region CameraStateFuctions

    void BeginThirdPersonState()
    {

    }

    void ThirdPersonStateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PunishMisAlignment = !PunishMisAlignment;
        }

        float HorizontalDelta = Input.GetAxis("Mouse X") * Time.deltaTime;
        float VerticalDelta = -Input.GetAxis("Mouse Y") * Time.deltaTime;

        if (InvertMouse) VerticalDelta *= -1;

        HorizontalAngle = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

        VerticalAngle = transform.rotation.eulerAngles.x * Mathf.Deg2Rad;

        VerticalAngle = Mathf.Clamp(VerticalAngle, -((Mathf.PI * 3f) / 4f), -(Mathf.PI / 4f));

        UpdateThirdPersonCameraPosition();
    }

    void EndThirdPersonState()
    {
        CamChangeLerpDir = 1;
        LoadRecipe(FirstPersonRecipe);
    }

    void BeginFirstPersonState()
    {
        //PlayerMesh = PlayerController.PC.transform.FindChild("Root").gameObject;
        //ThirdPersonPlayerMeshRot = PlayerMesh.transform.localRotation;
    }

    void FirstPersonStateUpdate()
    {
        UpdateFirstPersonCameraPosition();
        //PlayerMesh.transform.localRotation = m_Camera.transform.rotation;
    }

    void EndFirstPersonState()
    {
        CamChangeLerpDir = -1;
        LoadRecipe(ThirdPersonRecipe);
        //PlayerMesh.transform.rotation = ThirdPersonPlayerMeshRot;
    }

    void LoadRecipe(UMARecipeBase recipe)
    {
        GetComponent<UMADynamicAvatar>().Load(recipe);
    }

    void ResetCamSwitchBool()
    {
        CamModeChange = false;
    }

    void BeginADS()
    {
        // move camera to first person position
        UpdateFirstPersonCameraPosition();

        // turn off skinned mesh renderer
        playerSkinnedmeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer SMR in playerSkinnedmeshRenderers)
        {
            SMR.enabled = false;
        }

        // turn off weapon mesh
        playerMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer MR in playerMeshRenderers)
        {
            MR.enabled = false;
        }

        // Zoom camera in using FOV
        m_Camera.fieldOfView = ADSFOV;

        // adjust mouse sensitivity whilst ADS
        GetComponent<FirstPersonMovement>().mouseLook.XSensitivity = ADSSensitivity;
        GetComponent<FirstPersonMovement>().mouseLook.YSensitivity = ADSSensitivity;

        // change to ADS UI here
        pauseMenu.DisableUI();
        pauseMenu.SniperScopeUI.SetActive(true);
    }

    void EndADS()
    {
        // turn on skinned mesh renderer
        playerSkinnedmeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (SkinnedMeshRenderer SMR in playerSkinnedmeshRenderers)
        {
            SMR.enabled = true;
        }
        
        // turn on weapon mesh
        playerMeshRenderers = GetComponentsInChildren<MeshRenderer>(true);
        foreach (MeshRenderer MR in playerMeshRenderers)
        {
            MR.enabled = true;
        }

        //reset the cameras FOV
        m_Camera.fieldOfView = StandardFOV;

        // reset mouse sensitivity after ADS
        GetComponent<FirstPersonMovement>().mouseLook.XSensitivity = DefaultSensitivity;
        GetComponent<FirstPersonMovement>().mouseLook.YSensitivity = DefaultSensitivity;

        // Go back to normal UI here
        pauseMenu.DisableUI();
        pauseMenu.GamePlayHUD.SetActive(true);
    }

    void UpdateCamTypeStateMachine()
    {
        CameraTypeStates.SMUpdate();
    }

    #region CameraStateConditions

    bool ChangeCamMode()
    {
        return CamModeChange;
    }

    bool IsADS()
    {
        return (PlayerController.PC != null) ? PlayerController.PC.isADS : false;
    }

    #endregion

    void SetupStateMachine()
    {
        // Configure Conditions For Transitions
        Condition.BoolCondition CamSwitchCond = new Condition.BoolCondition();
        CamSwitchCond.Condition = ChangeCamMode;

        Condition.BoolCondition ADSCond = new Condition.BoolCondition();
        ADSCond.Condition = IsADS;

        Condition.NotCondition NotADSCond = new Condition.NotCondition();
        NotADSCond.Condition = ADSCond;

        // Create Transistions
        SM.Transition GoToFirst = new SM.Transition("Go to first person", CamSwitchCond, ResetCamSwitchBool);
        SM.Transition GoToThird = new SM.Transition("Go to third person", CamSwitchCond, ResetCamSwitchBool);
        SM.Transition GoToADS = new SM.Transition("Go To Aiming Down Sight", ADSCond);
        SM.Transition LeaveADS = new SM.Transition("Leaving ADS", NotADSCond);

        // Create States
        ThirdPersonState = new SM.State("Third person state",
            //transitions
            new List<SM.Transition>() { GoToFirst },
            //entry actions
            null,
            //update actions
            new List<SM.Action>() { TestForCamModeChange, ThirdPersonStateUpdate, SetCameraPosition },
            //exit actions
            new List<SM.Action>() { EndThirdPersonState });

        FirstPersonState = new SM.State("First person state",
            new List<SM.Transition>() { GoToThird },
            new List<SM.Action>() { BeginFirstPersonState },
            new List<SM.Action>() { TestForCamModeChange, FirstPersonStateUpdate, SetCameraPosition },
            new List<SM.Action>() { EndFirstPersonState });

        SM.State ADSState = new SM.State("Aiming Down Sight",
            new List<SM.Transition>() { LeaveADS },
            new List<SM.Action>() { BeginADS },
            new List<SM.Action>() { UpdateFirstPersonCameraPosition },
            new List<SM.Action>() { EndADS });

        SM.State CamTypeSMState = new SM.State("Camera Type State Machine",
            new List<SM.Transition>() { GoToADS },
            new List<SM.Action>() { },
            new List<SM.Action>() { UpdateCamTypeStateMachine },
            new List<SM.Action>() { });

        // Assign Target States to Transitions
        GoToFirst.SetTargetState(FirstPersonState);
        GoToThird.SetTargetState(ThirdPersonState);
        GoToADS.SetTargetState(ADSState);
        LeaveADS.SetTargetState(CamTypeSMState);

        // Create Machine
        CameraTypeStates = new SM.StateMachine(null, ThirdPersonState, FirstPersonState);
        CameraModeStates = new SM.StateMachine(null, CamTypeSMState, ADSState);

        // Start the State Machine
        CameraTypeStates.InitMachine();
        CameraModeStates.InitMachine();
    }

#endregion

}

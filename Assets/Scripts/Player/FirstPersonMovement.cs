using UnityEngine;
using System.Collections;

public class FirstPersonMovement : MonoBehaviour {

    [System.Serializable]
    public class MovementSettings
    {
        public float ForwardSpeed = 2.0f;   // Speed when walking forward
        public float BackwardSpeed = 1f;  // Speed when walking backwards
        public float StrafeSpeed = 1f;    // Speed when walking sideways
        public float RunMultiplier = 2.0f;   // Speed when sprinting
        public float CrouchMultiplier = 0.5f; // Speed when crouching
        public KeyCode RunKey = KeyCode.LeftShift;
        public float JumpForce = 0f;
        public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
        [HideInInspector]
        public float CurrentTargetSpeed = 8f;

        private bool m_Running;

        public void UpdateDesiredTargetSpeed(Vector2 input)
        {
            if (input == Vector2.zero) return;

            if (input.x > 0 || input.x < 0)
            {
                //strafe
                CurrentTargetSpeed = StrafeSpeed;
            }

            if (input.y < 0)
            {
                //backwards
                CurrentTargetSpeed = BackwardSpeed;
            }

            if (input.y > 0)
            {
                //forwards
                //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                CurrentTargetSpeed = ForwardSpeed;
            }

            if (Input.GetAxis("Sprint") > 0)
            {
                CurrentTargetSpeed *= RunMultiplier;
                m_Running = true;
            }
            else
            {
                m_Running = false;
            }            

            if (PlayerMovementController.PlayerCrouching)
            {
                CurrentTargetSpeed *= CrouchMultiplier;
            }
        }

        public bool Running
        {
            get { return m_Running; }
        }
    }

    [System.Serializable]
    public class AdvancedSettings
    {
        public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
        public float stickToGroundHelperDistance = 0.5f; // stops the character
        public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
        public bool airControl; // can the user control the direction that is being moved in the air
        [Tooltip("set it to 0.1 or more if you get stuck in wall")]
        public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
    }

    public Camera cam;
    public MovementSettings movementSettings = new MovementSettings();
    public UnityStandardAssets.Characters.FirstPerson.MouseLook mouseLook = new UnityStandardAssets.Characters.FirstPerson.MouseLook();
    public AdvancedSettings advancedSettings = new AdvancedSettings();

    private Rigidbody m_RigidBody;
    private CapsuleCollider m_Capsule;
    private float m_YRotation;
    private Vector3 m_GroundContactNormal;
    private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;
    private Animator anim;
    private bool m_Grounded;

    public Vector3 Velocity
    {
        get { return m_RigidBody.velocity; }
    }

    public bool Grounded
    {
        get { return m_IsGrounded; }
    }

    public bool Jumping
    {
        get { return m_Jumping; }
    }

    public bool Running
    {
        get
        {
            return movementSettings.Running;
        }
    }

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_RigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_Capsule = GetCapsule();
        cam = Camera.main;
        mouseLook.Init(transform, cam.transform);
        mouseLook.SetCursorLock(false);
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
    }

    CapsuleCollider GetCapsule()
    {
        CapsuleCollider[] caps = GetComponents<CapsuleCollider>();
        foreach(CapsuleCollider col in caps)
        {
            if (!col.isTrigger)
            {
                return col;
            }
        }
        return null;
    }

    private void OnEnable()
    {
        if(anim != null)
            anim.applyRootMotion = false;
    }

    private void OnDisable()
    {
        if (anim != null)
            anim.applyRootMotion = true;
    }

    public void ResetRootMotion()
    {
        anim.applyRootMotion = false;
    }

    private void Update()
    {
        RotateView();

        if (Input.GetButtonDown("CrouchToggle"))
        {
            PlayerMovementController.PlayerCrouching = !PlayerMovementController.PlayerCrouching;
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Vector2 input = GetInput();

        //print(m_IsGrounded);

        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
        {
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
            desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

            desiredMove.x = desiredMove.x * movementSettings.CurrentTargetSpeed;
            desiredMove.z = desiredMove.z * movementSettings.CurrentTargetSpeed;
            desiredMove.y = desiredMove.y * movementSettings.CurrentTargetSpeed;
            if (m_RigidBody.velocity.sqrMagnitude <
                (movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed))
            {
                m_RigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.VelocityChange);
            }
        }

        if (m_IsGrounded)
        {
            m_RigidBody.drag = 5f;

            if (m_Jump)
            {
                m_RigidBody.drag = 0f;
                m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
                m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                m_Jumping = true;
            }

            if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
            {
                m_RigidBody.Sleep();
            }
        }
        else
        {
            m_RigidBody.drag = 0f;
            if (m_PreviouslyGrounded && !m_Jumping)
            {
                StickToGroundHelper();
            }
        }

        m_Jump = false;

        UpdateAnimator();

    }

    public void UpdateAnimator()
    {
        if (anim != null)
        {
            float ForwardMotion = Vector3.Scale(m_RigidBody.velocity, transform.forward).magnitude;
            anim.SetFloat("Forward", ForwardMotion, 0.1f, Time.deltaTime);
            anim.SetBool("Crouch", PlayerMovementController.PlayerCrouching);
            anim.SetBool("OnGround", m_IsGrounded);
            if (!m_IsGrounded)
            {
                anim.SetFloat("Jump", m_RigidBody.velocity.y);
            }
        }
        else
        {
            anim = GetComponent<Animator>();
        }
    }

    private float SlopeMultiplier()
    {
        float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
        return movementSettings.SlopeCurveModifier.Evaluate(angle);
    }

    private void StickToGroundHelper()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) +
                               advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
            {
                m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
            }
        }
    }

    private Vector2 GetInput()
    {

        Vector2 input = new Vector2
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical")
        };
        movementSettings.UpdateDesiredTargetSpeed(input);
        return input;
    }


    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        mouseLook.LookRotation(transform, cam.transform);

        if (m_IsGrounded || advancedSettings.airControl)
        {
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
        }
    }

    /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
    private void GroundCheck()
    {
        m_PreviouslyGrounded = m_IsGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position + (Vector3.up * 0.3f), m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
            ((m_Capsule.height / 2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            m_IsGrounded = true;
            m_GroundContactNormal = hitInfo.normal;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundContactNormal = Vector3.up;
        }
        if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
        {
            m_Jumping = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    PlayerManager playerManager;

    public PlayerControls controls;

    [Header("Movement Characteristics")]
    [Tooltip("The maximum amount of forward speed the player can achieve while walking")] public float maxMoveSpeed;
    [Tooltip("How quickly the player accelerates forward")] public float accSpeed;
    [Tooltip("Currently unused in the code.")] public float maxStrafeSpeed;
    [Tooltip("How quickly the player accelerates sideways")] public float accStrafe;
    [Tooltip("The multiplier to the player's acceleration while sprinting")] public float accSprint;
    [Tooltip("The maximum possible speed achievable while sprinting")] public float maxSprintSpeed;
    [Tooltip("The rate at which the player sheds speed when above maximum")] public float slowDownSpeed;
    [Tooltip("The speed the player will slow down to when movement keys are released (not factoring friction)")] public float minimumThresholdSpeed;

    [Space]
    [Tooltip("The force applied to the player when jumping")] public float jumpForce;

    [Header("Parkour Characteristics")]
    [Tooltip("The forward impulse applied when vaulting")] public float vaultBoost;
    [Tooltip("The upwards impulse applied when vaulting")] public float vaultHeight;

    [Space]
    public Transform cameraHolder;

    [Header("Debug")]
    [SerializeField] Vector3 HorizontalVelocity;
    [SerializeField] Vector3 MovementVector;
    public float HorizontalVelocityf;

    Vector3 lastPos;
    //[SerializeField] Vector3 HorizontalVelocity;

    float xMove;
    float zMove;

    bool isAtMaxSpeed;
    bool isAtMaxSprintSpeed;
    bool canJump = true;
    bool isSprinting;
    bool isInVaultTrigger;

    void Awake()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();

        //enable controls
        controls = new PlayerControls();
        controls.PlayerMovement.Enable();
        Debug.Log(controls);

        canJump = true;

        //
        InvokeRepeating("calculateMovementVector", 0, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {

        movement();

        //get input in
        // zMove = Input.GetAxis("Horizontal");
        // xMove = Input.GetAxis("Vertical");



        //locking the mouse
        if (Input.GetKeyDown(KeyCode.P)) playerManager.lockMouse();
    }

    void movement()
    {
        //jumping and vaulting
        JumpAndVault();

        //calculate how fast we're moving along the ground
        HorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        HorizontalVelocityf = HorizontalVelocity.magnitude;

        //just check if we're moving at maximum speed
        if (Mathf.Abs(HorizontalVelocityf) >= maxMoveSpeed) isAtMaxSpeed = true;
        else isAtMaxSpeed = false;

        //check if moving at max Sprint speed
        if (Mathf.Abs(HorizontalVelocityf) >= maxSprintSpeed) isAtMaxSprintSpeed = true;
        else isAtMaxSprintSpeed = false;

        //input
        Vector2 movInput = controls.PlayerMovement.Movement.ReadValue<Vector2>();
        xMove = movInput.y;
        zMove = movInput.x;

        //sprinting
        float isSprinting_ = controls.PlayerMovement.Sprint.ReadValue<float>();
        if (isSprinting_ == 0) isSprinting = false;
        else isSprinting = true;

        //controls.PlayerMovement.Sprint.started += ctx => isSprinting = ctx.ReadValue<bool>();

        //ismoving
        if (movInput != Vector2.zero)
        {
            if (!isSprinting)
            {
                if (!isAtMaxSpeed)//is below max speed and isnt sprinting (walking)
                {
                    rb.AddForce(transform.forward * xMove * accSpeed * Time.deltaTime);
                    rb.AddForce(transform.right * zMove * accStrafe * Time.deltaTime);
                }
            }
            else
            {
                if (!isAtMaxSprintSpeed)//is sprinting
                {
                    rb.AddForce(transform.forward * xMove * accSprint * accSpeed * Time.deltaTime);
                    rb.AddForce(transform.right * zMove * accStrafe * Time.deltaTime);
                }
            }

        }
        else
        {
            if (Mathf.Abs(HorizontalVelocity.magnitude) > minimumThresholdSpeed)
                rb.AddForce(-MovementVector * slowDownSpeed * Time.deltaTime);
        }
    }

    void JumpAndVault()
    {
        //if player is trying to vault
        if (controls.PlayerMovement.Vault.triggered)
        {
            if (isInVaultTrigger && /*isSprinting &&*/ !playerManager.isOnGround())//and theyre in the vault zone, sprinting, and in the air,
            {
                //vault
                rb.AddForce(transform.forward * vaultBoost, ForceMode.Impulse);
                rb.AddForce(transform.up * vaultHeight, ForceMode.Impulse);
            }
        }

        if (controls.PlayerMovement.Jump.triggered)
        {
            if (playerManager.isOnGround())
            {
                rb.AddForce(Vector3.up * jumpForce);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("VaultTrigger"))
        {
            isInVaultTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("VaultTrigger"))
        {
            isInVaultTrigger = false;
        }
    }

    //generates a vector that faces the direction the player is moving
    //is reversed to slow them down
    void calculateMovementVector()
    {
        MovementVector = transform.position - lastPos;
        lastPos = transform.position;
    }
}

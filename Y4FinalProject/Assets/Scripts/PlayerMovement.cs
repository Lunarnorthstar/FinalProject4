using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
//using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public SettingsHolder SH;
    public CameraMove cam;

    [Tooltip("a list of animation names that when playing, the camera should stop moving")]
    public string[] camPauseAniStates;

    public PlayerControls controls;

    Animator ani;
    Rigidbody rb;
    PlayerManager playerManager;
    IKControl playerIK;
    CameraMove playerCamera;
    Powerups powerUps;

    [Header("Movement Characteristics")]
    [Tooltip("The base maximum movement speed the player starts at")] public float startMaxSpeed = 10;
    [Tooltip("The rate at which the player's max speed increases")] public float maxSpeedAccel = 1;
    [Tooltip("The highest maximum speed the player can achieve")] public float totalMaxSpeed = 15;
    [Tooltip("The maximum amount of forward speed the player can achieve while walking")] private float maxMoveSpeed;
    [Tooltip("How quickly the player accelerates forward")] public float accSpeed;
    [Tooltip("Currently unused in the code.")] public float maxStrafeSpeed;
    [Tooltip("How quickly the player accelerates sideways")] public float accStrafe;
    //[Tooltip("The multiplier to the player's acceleration while sprinting")] public float accSprint;
    //[Tooltip("The maximum possible speed achievable while sprinting")] public float maxSprintSpeed;
    [Tooltip("The rate at which the player sheds speed when above maximum")] public float slowDownSpeed;
    [Tooltip("How fast you're going in the air")] public float airSpeedMultiplier;
    [Tooltip("The speed the player will slow down to when movement keys are released (not factoring friction)")] public float minimumThresholdSpeed;
    [Tooltip("The speed you must achieve to initiate a slide")] public float minSlideSpeed;
    [Tooltip("The speed at which the player automatically stops sliding")] public float slideStopSpeed;
    public float slideDeadzone;
    public float JoyCamSensitivity = 1.2f;

    [Tooltip("The amount of time the player model 'grabs' something")]
    public float IKGrabTime = 0.2f;

    [Space]
    [Tooltip("The force applied to the player when jumping")] public float jumpForce;
    [Tooltip("the force applied when wallRunning")] public float wallJumpForce;

    [Header("Parkour Characteristics")]
    [Tooltip("The forward impulse applied when vaulting")] public float vaultBoost;
    [Tooltip("The upwards impulse applied when vaulting")] public float vaultHeight;
    public float ClimbUpForce;
    public float ClimbForwardsForce;
    public float WallRunSideBoost;
    public float WallRunUpBoost;

    [Space]
    public float standardDrag;
    public float inAirDrag;
    public float slidingDrag;
    public float crouchingDrag;

    [Space]
    [HideInInspector] public Transform cameraHolder;
    [HideInInspector] public Transform ClimbLookTarget;
    [HideInInspector] public Transform hangPos;

    [Space]
    [Header("Terrain")]
    [HideInInspector] public float moveSpeedMult = 1;
    [HideInInspector] public float jumpHeightMult = 1;
    [HideInInspector] public float fricitonMult = 1;

    [Header("Debug")]
    public Slider speedSlider;
    public TextMeshProUGUI speedText;
    public GameObject pauseMenu;

    public Vector3 HorizontalVelocity;
    public float HorizontalVelocityf;
    public float verticalVelocity;
    public float horizontalVelocityLerp;
    public float velocityLerpSpeed;

    Vector3 lastPos;
    //[SerializeField] Vector3 HorizontalVelocity;

    float xMove;
    float zMove;

    bool isAtMaxSpeed;
    bool isAtMaxSprintSpeed;
    bool isClimbing;
    [SerializeField] bool canJump = true;
    bool jumpCheck;
    bool wallRunCheck;
    bool canSideJump = true;
    bool isInVaultTrigger;
    bool hasJustBeenAgainstWall;
    bool hasJustVaulted;
    bool hasResetWallRun;

    public bool isAgainstLedge;
    public bool isHangingOnWall;
    public bool isTryingToSlide;
    public bool isTryingToWallRun;
    public bool isTryingToCrouch;
    public bool isSliding;
    public bool isWallRunning;
    public bool hasJustWallRun;
    public bool hasSlid;
    public bool isOnGround;
    public bool isFacingWall;
    public int wallRunDir;
    public int lastRunDir;
    public bool isGliding;
    public bool isAffectedByTerrain = true;

    public string wallFacingTag;

    private bool climbCool = false;

    void Awake()
    {
        Time.timeScale = 1;
        maxMoveSpeed = startMaxSpeed;
        //Application.targetFrameRate = 60;

        //assign variables
        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        ani = GetComponent<Animator>();
        playerIK = GetComponentInChildren<IKControl>();
        playerCamera = GetComponentInChildren<CameraMove>();
        powerUps = GetComponent<Powerups>();

        //enable controls
        controls = new PlayerControls();
        controls.PlayerMovement.Enable();

        canSideJump = true;
        canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        JoyCamSensitivity = SH.JoySensitivity;
        //if (Input.GetKeyDown(KeyCode.M)) resetInput(); I've disabled this because we don't need it anymore and it's causing lots of problems with movement reactivating when it shouldn't.

        //manage input and movement
        input();
        movement();

        //wall detection
        if (!isHangingOnWall)
            isOnGround = playerManager.isOnGround();
        else isOnGround = false;

        //this method returns both a bool for if it is and a string for the tag.
        var wallInfo = playerManager.isTouchingWall(transform.forward);
        isFacingWall = wallInfo.Item1;
        wallFacingTag = wallInfo.Item2;

        //if youre touching a wall to the left then its 1, right = 2 and if not then 0
        if (playerManager.isTouchingWall(-transform.right).Item1) wallRunDir = 1;
        else if (playerManager.isTouchingWall(transform.right).Item1) wallRunDir = 2;
        else wallRunDir = 0;
    }

    void resetInput()
    {
        controls = new PlayerControls();
        controls.PlayerMovement.Enable();
    }

    
    void input()
    {
        //reset scene if R is pressed
        if (controls.PlayerMovement.Reset.triggered)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //send input to camera
        playerCamera.joyCamera = controls.PlayerMovement.Look.ReadValue<Vector2>() * JoyCamSensitivity;

        //locking the mouse
        //if (Input.GetKeyDown(KeyCode.L)) playerManager.lockMouse(); I've disabled this because it's a debug key anyway, it's replaceable by pressing esc, and it's causing problems in the level end screen.

        if (controls.PlayerMovement.Pause.triggered)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            playerManager.lockMouse();

            if (controls.PlayerMovement.Ability.enabled) //We have to do it like this because disabling the entire playercontrols makes it impossible to unpause.
            {
                controls.PlayerMovement.Ability.Disable();
                controls.PlayerMovement.ChangeAbility.Disable();
            }
            else
            {
                controls.PlayerMovement.Ability.Enable();
                controls.PlayerMovement.ChangeAbility.Enable();
            }
            
            

            Time.timeScale = 1 - Time.timeScale;
        }
    }

    void movement()
    {
        //jumping and vaulting
        dragControl();
        JumpAndVault();
        sliding();

        //calculate how fast we're moving along the ground
        HorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        HorizontalVelocityf = HorizontalVelocity.magnitude;

        horizontalVelocityLerp = Mathf.Lerp(horizontalVelocityLerp, HorizontalVelocityf, velocityLerpSpeed);

        verticalVelocity = rb.velocity.y;

        speedSlider.value = HorizontalVelocityf;
        speedText.text = HorizontalVelocityf.ToString("0");

        
        //just check if we're moving at maximum speed
        if (Mathf.Abs(HorizontalVelocityf) >= maxMoveSpeed * moveSpeedMult) isAtMaxSpeed = true;
        else isAtMaxSpeed = false;

        //sliding
        float isSlide = controls.PlayerMovement.ControlKey.ReadValue<float>();
        if (isSlide != 0) isTryingToSlide = true;
        else { isTryingToSlide = false; hasSlid = false; }

        //wallrunning
        float isWallRun = controls.PlayerMovement.SpeedKey.ReadValue<float>();
        if (isWallRun != 0) isTryingToWallRun = true;
        else { isTryingToWallRun = false; }

        //crouching
        if (controls.PlayerMovement.ControlKey.ReadValue<float>() != 0) isTryingToCrouch = true;
        else isTryingToCrouch = false;

        //input
        Vector2 movInput = controls.PlayerMovement.Movement.ReadValue<Vector2>();
        if (!isSliding)
        {//if you're sliding, then you're sliding and not walking - thus nullify any input.
            xMove = movInput.y;
            zMove = movInput.x;
        }
        else
        {
            xMove = 0;
            zMove = 0;
        }

        //teleport player to hang pos and go no further in script (so player can't move)
        if (isHangingOnWall && hangPos != null)
        {
            transform.position = Vector3.Lerp(transform.position, hangPos.position, 0.2f);
            return;
        }

        //ismoving
        if (movInput != Vector2.zero /*&& !isAtMaxSpeed*/)
        {
            if (isOnGround)
            {
                rb.AddForce(transform.forward * xMove * accSpeed * moveSpeedMult * Time.deltaTime);
                rb.AddForce(transform.right * zMove * accStrafe * moveSpeedMult * Time.deltaTime);
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxMoveSpeed);
            }
            else if(!isAtMaxSpeed)
            {
                rb.AddForce(transform.forward * xMove * accSpeed * airSpeedMultiplier * moveSpeedMult * Time.deltaTime);
                rb.AddForce(transform.right * zMove * accStrafe * airSpeedMultiplier * moveSpeedMult * Time.deltaTime);
            }

            
        }

        //abilities
        if (controls.PlayerMovement.Ability.triggered)
        {
            powerUps.ActivatePowerup(1);
        }

        if (controls.PlayerMovement.ChangeAbility.triggered) //Probably could change the name of this or make it some vector2d whatever whatever but this works fine as is.
        {
            powerUps.ActivatePowerup(2);
        }

        if (controls.PlayerMovement.Ability.WasReleasedThisFrame())
        {
            powerUps.ReleasePowerup(1);
        }

        if (controls.PlayerMovement.ChangeAbility.WasReleasedThisFrame())
        {
            powerUps.ReleasePowerup(2);
        }



        if (powerUps.grappleHook.Active && controls.PlayerMovement.Jump.triggered)
        {
            //powerUps.OffBreakGrapple();
        }

        //Top Speed Accel
        if (movInput != Vector2.zero && maxMoveSpeed < totalMaxSpeed)
        {
            maxMoveSpeed += maxSpeedAccel * Time.deltaTime;
        }
        else if (movInput == Vector2.zero)
        {
            maxMoveSpeed = startMaxSpeed;
        }
    }

    void smallVault()
    {
        rb.AddForce(transform.forward * vaultBoost, ForceMode.Impulse);
        rb.AddForce(transform.up * vaultHeight, ForceMode.Impulse);

        ani.Play("Vault");

        playerIK.ikActive = true; //Touch stuff

        hasJustVaulted = true;
        InvokeRepeating("resetVaultBoost", 0, 0.05f);
        // StartCoroutine(resetVault());
        //  col.enabled = false;
    }

    void JumpAndVault()
    {
        //if player is trying to vault
        if (controls.PlayerMovement.Vault.triggered)
        {
            if (isInVaultTrigger && /*isSprinting &&*/ !isOnGround)//and theyre in the vault zone, sprinting, and in the air,
            {
                //vault
                // rb.AddForce(transform.forward * vaultBoost, ForceMode.Impulse);
                // rb.AddForce(transform.up * vaultHeight, ForceMode.Impulse);
                smallVault();
            }
        }

        //flatground jumping
        if (controls.PlayerMovement.Jump.ReadValue<float>() != 0)
        {
            if (isOnGround && canJump)
            {
                canJump = false;
                jumpCheck = false;
                rb.drag = inAirDrag; //Make sure the drag is set consistently - if you're jumping you are by definition going to be in the air.
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);//

                Debug.Log(rb.velocity);
                rb.AddForce(Vector3.up * jumpForce * jumpHeightMult, ForceMode.Impulse);
                InvokeRepeating("resetJump", 0, 0.02f);
            }
        }

        /*
        //wallRunning (vertical, up a wall)
        if (isFacingWall && !isOnGround && !hasJustBeenAgainstWall)
        {
            if (controls.PlayerMovement.Jump.triggered)
            {
                rb.velocity = new Vector3(0, 4, 0);
                rb.AddForce(Vector3.up * wallJumpForce * jumpHeightMult, ForceMode.Impulse);

                hasJustBeenAgainstWall = true;
            }
        }
        if (hasJustBeenAgainstWall && isOnGround) hasJustBeenAgainstWall = false;
        */

        //side wall jump (along the side of a wall)
        if (!isOnGround && wallRunDir != 0)//if in air and touching a wall
        {
            //if u jump, haven't jumped in the last 0.2 secs and arent trying to jump off a wall you just jumped off
            if (controls.PlayerMovement.Jump.triggered && canSideJump && lastRunDir != wallRunDir)
            {
                canSideJump = false;
                switch (wallRunDir)
                {
                    case 1://the wall is on the left, boost right
                        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);//zero vertical velocity for consitency
                        rb.AddForce(transform.right * WallRunSideBoost, ForceMode.Impulse);
                        rb.AddForce(transform.up * WallRunUpBoost, ForceMode.Impulse);

                        lastRunDir = wallRunDir;
                        break;
                    case 2://the wall is on the right, boost left
                        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);//zero vertical velocity for consitency
                        rb.AddForce(-transform.right * WallRunSideBoost, ForceMode.Impulse);
                        rb.AddForce(transform.up * WallRunUpBoost, ForceMode.Impulse);

                        lastRunDir = wallRunDir;
                        break;
                }
                GetIKTarget("release");

                Invoke("resetSideJump", 0.2f);
            }
        }
        if (isOnGround && lastRunDir != 0) lastRunDir = 0;//when player touches ground, they can now wall run off the same side wall

        //wall sliding
        // if player is beside wall, in the air and holding slide, then attach em to that wall and slow them down like it's a slide.
        wallSliding();

        //ledge grabbing
        
        if (isClimbing)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            moveSpeedMult = 0;

            if (controls.PlayerMovement.Jump.triggered)
            {
                GetIKTarget("release");
                isClimbing = false;
                climbCool = true;
                rb.useGravity = true;
                moveSpeedMult = 1;
                rb.AddForce(transform.up * ClimbUpForce, ForceMode.Impulse);
                rb.AddForce(transform.forward * ClimbForwardsForce, ForceMode.Impulse);
                Invoke("resetClimb", 0.5f);
            }
        }
        
        //is pressing grab button and not grounded and in a valid trigger and not already climbing
        if (controls.PlayerMovement.Grab.triggered && !isOnGround && isAgainstLedge && !isClimbing && !climbCool)
        {
            isClimbing = true;
            transform.Translate(-transform.forward * 0.5f);
            
            GetIKTarget("climb");
        }
    }

    public void wallSliding()
    {

        if (isTryingToWallRun && controls.PlayerMovement.Jump.ReadValue<float>() == 0)//is the button being pressed and not jumping?
        {
            //are u moving fast enough, touching a wall with your side and in the air and not trying to jump off said wall, and have you not just jumped off a wall?
            if (HorizontalVelocityf >= minSlideSpeed && wallRunDir != 0 && !isOnGround && canSideJump)
            {
                isWallRunning = true;
            }
            else if (!isWallRunning || wallRunDir == 0)
            {
                isWallRunning = false;
            }
        }
        else
        {
            isWallRunning = false;
        }


        if (isWallRunning && HorizontalVelocityf < slideStopSpeed)
        {
            isWallRunning = false;
        }

        // if (!isWallRunning && !hasJustWallRun && !hasResetWallRun)
        // {
        //     hasResetWallRun = false;
        //     InvokeRepeating("resetWallRun", 0, 0.02f);

        // }

        if (isWallRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (wallRunDir == 1)
            {
                ani.SetInteger("WallSlide", 1);
                GetIKTarget("wallRunL"); //Find the point you grab and grab it
            }
            if (wallRunDir == 2)
            {
                ani.SetInteger("WallSlide", 2);
                GetIKTarget("wallRunR"); //Find the point you grab and grab it
            }
        }
        else
        {
            ani.SetInteger("WallSlide", 0);
        }
    }

    public void sliding()
    {
        if (isTryingToSlide)//is button being pressed?
        {
            bool isStraight;

            if (zMove > -slideDeadzone && zMove < slideDeadzone) isStraight = true;
            else isStraight = false;
            if (isOnGround && /*isSprinting &&*/ HorizontalVelocityf >= minSlideSpeed && !hasSlid && isStraight)//are you sprinting, moving faster than minimum and havent just exited a slide?
            {
                isSliding = true;
            }
            else if (!isSliding)//if all this is not the case and is not currently in a slide, no slide for u (you can slide when below the start speed, just not start a slide)
            {
                isSliding = false;
            }
        }
        else// if button isnt even being pressed, then no slide
        {
            isSliding = false;
            // hasSlid = true; //you were just in a slide - only way for variable to change is with a release of the slide button
        }

        if (isSliding && HorizontalVelocityf <= slideStopSpeed)//if currently sliding but then go too slow, no slide for u  
        {
            isSliding = false;
            hasSlid = true; //you were just in a slide - only way for variable to change is with a release of the slide button
        }


        if (isSliding)
            ani.SetBool("slide", true);
        else
            ani.SetBool("slide", false);

        if (hasSlid)
            ani.SetBool("hasSlid", true);
        else
            ani.SetBool("hasSlid", false);

    }

    private void GetIKTarget(String type)
    {
        switch (type)
        {
            case "vault":
                RaycastHit hit;
                Physics.Raycast(transform.position, transform.forward - transform.up, out hit, 10.0f);
                playerIK.vaultPoint = hit.point;
                playerIK.IKTime = IKGrabTime;
                playerIK.ikActive = true;
                break;
            case "climb":
                RaycastHit climbHit;
                Physics.Raycast(transform.position, transform.forward, out climbHit, 10.0f);
                playerIK.vaultPoint = climbHit.point;
                playerIK.IKTime = 10000;
                playerIK.ikActive = true;
                break;
            case "wallRunR":
                RaycastHit WallRHit;
                Physics.Raycast(transform.position + new Vector3(0, 0, 1), transform.forward + transform.right, out WallRHit, 10.0f);
                playerIK.vaultPoint = WallRHit.point;
                playerIK.IKTime = IKGrabTime;
                playerIK.ikActive = true;
                break;
            case "wallRunL":
                RaycastHit WallLHit;
                Physics.Raycast(transform.position + new Vector3(0, 0, 1), transform.forward - transform.right, out WallLHit, 10.0f);
                playerIK.vaultPoint = WallLHit.point;
                playerIK.IKTime = IKGrabTime;
                playerIK.ikActive = true;
                break;
            case "release":
                playerIK.ikActive = false;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("VaultTrigger"))
        {
            isInVaultTrigger = true;

            GetIKTarget("vault"); //Find the point you grab and grab it
        }

        if (other.gameObject.CompareTag("ClimbTrigger"))
        {
            /*Transform[] childrenTransforms = other.transform.GetComponentsInChildren<Transform>();
            for (int i = 0; i < childrenTransforms.Length; i++)
            {
                switch (childrenTransforms[i].name)
                {
                    case "ClimbLookTarget":
                        ClimbLookTarget = childrenTransforms[i];
                        break;
                    case "HangPos":
                        hangPos = childrenTransforms[i];
                        break;
                }
            }*/
            isAgainstLedge = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("VaultTrigger"))
        {
            isInVaultTrigger = false;
            //playerIK.ikActive = false; //Stop touching stuff.
        }

        if (other.gameObject.CompareTag("ClimbTrigger"))
        {
            ClimbLookTarget = null;
            hangPos = null;
            isAgainstLedge = false;
        }
    }

    void dragControl()
    {
        rb.useGravity = !isHangingOnWall;

        if (!isOnGround || !canJump)//is in air
        {
            rb.drag = inAirDrag;
        }
        else if (!isTryingToSlide && !isTryingToCrouch)//is simply on ground
        {
            rb.drag = standardDrag * fricitonMult;
        }
        else if (isSliding || isWallRunning)//is sliding or running horizontally across a wall
        {
            rb.drag = slidingDrag * fricitonMult;
        }
        else if (isTryingToCrouch)//is crouching
        {
            rb.drag = crouchingDrag * fricitonMult;
        }

        //if is in air and not vaulting and not dashing then use this as speed limit
        /*if (!hasJustVaulted || !isGliding)
        {

            if (isGliding) return;
            if (HorizontalVelocityf > maxMoveSpeed)
            {
                Vector3 limitedVel = HorizontalVelocity.normalized * (maxMoveSpeed - 0.1f);
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }*/

    }

    void resetWallRun()
    {
        if (wallRunDir != 0 && !wallRunCheck) hasJustWallRun = true;//currently wallrunnin
        if (wallRunDir == 0 && !wallRunCheck) wallRunCheck = true;//was but now isnt
        if (wallRunDir != 0 && wallRunCheck) hasJustWallRun = false;

        if (hasJustWallRun)
        {
            CancelInvoke("resetWallRun");

            hasResetWallRun = true;
        }

    }

    void resetJump()
    {
        //if (isOnGround && !jumpCheck) canJump = false;//if Youre on the ground but havent left the ground yet, you can't jump again.
        //if (!jumpCheck && !isOnGround) jumpCheck = true;//if you arent on ground but the bool hasnt updated, then update the bool
        //if (isOnGround && jumpCheck) canJump = true;// if youre on the ground and youve been checked to be in air, then you can jump again

        canJump = isOnGround; //Not sure why we needed something so complicated, but it was causing problems so this will do.

        if (canJump) CancelInvoke("resetJump");//if you can jump, stop looping this function
    }

    void resetSideJump()
    {
        canSideJump = true;
    }

    void resetVaultBoost()
    {
        if (isOnGround)
        {
            hasJustVaulted = false;
            CancelInvoke("resetVaultBoost");
        }
    }

    void resetClimb()
    {
        climbCool = false;
        ani.ResetTrigger("Pull Up");
    }
}
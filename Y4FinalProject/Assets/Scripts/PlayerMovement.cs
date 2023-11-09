using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CameraMove cam;

    [Tooltip("a list of animation names that when playing, the camera should stop moving")]
    public string[] camPauseAniStates;

    public PlayerControls controls;

    Animator ani;
    Rigidbody rb;
    PlayerManager playerManager;
    IKControl playerIK;

    [Header("Movement Characteristics")]
    [Tooltip("The maximum amount of forward speed the player can achieve while walking")] public float maxMoveSpeed;
    [Tooltip("How quickly the player accelerates forward")] public float accSpeed;
    [Tooltip("Currently unused in the code.")] public float maxStrafeSpeed;
    [Tooltip("How quickly the player accelerates sideways")] public float accStrafe;
    [Tooltip("The multiplier to the player's acceleration while sprinting")] public float accSprint;
    [Tooltip("The maximum possible speed achievable while sprinting")] public float maxSprintSpeed;
    [Tooltip("The rate at which the player sheds speed when above maximum")] public float slowDownSpeed;
    [Tooltip("How fast you're going in the air")] public float airSpeedMultiplier;
    [Tooltip("The speed the player will slow down to when movement keys are released (not factoring friction)")] public float minimumThresholdSpeed;
    [Tooltip("The speed you must acheive to initiate a slide")] public float minSlideSpeed;
    [Tooltip("The speed at which the player automatically stops sliding")] public float slideStopSpeed;

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
    public float GrapplingDrag;

    [Space]
    public Transform cameraHolder;
    public Transform ClimbLookTarget;
    public Transform hangPos;

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
    bool isClimbing;
    [SerializeField] bool canJump = true;
    [SerializeField] bool jumpCheck;
    bool wallRunCheck;
    bool canSideJump = true;
    bool isInVaultTrigger;
    bool hasJustBeenAgainstWall;
    bool hasJustVaulted;

    public bool isSprinting;
    public bool isAgainstLedge;
    public bool isHangingOnWall;
    public bool isTryingToSlide;
    public bool isSliding;
    public bool isWallRunning;
    public bool hasJustWallRun;
    public bool hasSlid;
    public bool isOnGround;
    public bool isFacingWall;
    public int wallRunDir;
    public int lastRunDir;

    void Awake()
    {
        Application.targetFrameRate = 60;

        //assign variables
        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        ani = GetComponent<Animator>();
        playerIK = GetComponentInChildren<IKControl>();

        //enable controls
        controls = new PlayerControls();
        controls.PlayerMovement.Enable();

        canSideJump = true;
        canJump = true;

        InvokeRepeating("calculateMovementVector", 0, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {

        if (rb.velocity.y > 0)
        {
            Debug.Log(rb.velocity.y);
            Debug.Log(rb.drag);
        }
        
        
        
        //if you reload the script, pressing M will get rid of the error with input
        if (Input.GetKeyDown(KeyCode.M)) resetInput();

        if (controls.PlayerMovement.Reset.triggered)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        movement();

        if (!isHangingOnWall)
            isOnGround = playerManager.isOnGround();
        else isOnGround = false;

        isFacingWall = playerManager.isTouchingWall(transform.forward);

        //if youre touching a wall to the left then its 1, right = 2 and if not then 0
        if (playerManager.isTouchingWall(-transform.right)) wallRunDir = 1;
        else if (playerManager.isTouchingWall(transform.right)) wallRunDir = 2;
        else wallRunDir = 0;

        //locking the mouse
        if (Input.GetKeyDown(KeyCode.P)) playerManager.lockMouse();
    }

    void resetInput()
    {
        controls = new PlayerControls();
        controls.PlayerMovement.Enable();
    }
    void movement()
    {
        //jumping and vaulting
        JumpAndVault();
        sliding();
        dragControl();

        //calculate how fast we're moving along the ground
        HorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        HorizontalVelocityf = HorizontalVelocity.magnitude;

        //just check if we're moving at maximum speed
        if (Mathf.Abs(HorizontalVelocityf) >= maxMoveSpeed) isAtMaxSpeed = true;
        else isAtMaxSpeed = false;

        //check if moving at max Sprint speed
        if (Mathf.Abs(HorizontalVelocityf) >= maxSprintSpeed) isAtMaxSprintSpeed = true;
        else isAtMaxSprintSpeed = false;

        //sliding
        float isSlide = controls.PlayerMovement.Slide.ReadValue<float>();
        if (isSlide != 0) isTryingToSlide = true;
        else { isTryingToSlide = false; hasSlid = false; }

        //input
        Vector2 movInput = controls.PlayerMovement.Movement.ReadValue<Vector2>();
        if (!isSliding)//if youre sliding, then you're sliding and not walking - thus nullify any input.
            xMove = movInput.y;
        else
            xMove = 0;
        zMove = movInput.x;

        //sprinting

        float isSprinting_ = controls.PlayerMovement.Sprint.ReadValue<float>();
        if (isSprinting_ != 0)//if u are holding sprint rn
        {
            isSprinting = true;
        }
        else//this will remain true until you slow down too much
        {
            if (HorizontalVelocityf <= maxMoveSpeed || !isOnGround)
            {
                isSprinting = false;
            }
        }

        if (movInput == Vector2.zero) isSprinting = false;
        //controls.PlayerMovement.Sprint.started += ctx => isSprinting = ctx.ReadValue<bool>();

        //teleport player to hang pos and go no further in script (so player can't move)
        if (isHangingOnWall && hangPos != null)
        {
            transform.position = Vector3.Lerp(transform.position, hangPos.position, 0.2f);
            return;
        }

        //ismoving
        if (movInput != Vector2.zero && !hasJustVaulted)
        {
            if (!isSprinting)
            {
                if (isOnGround)
                {
                    rb.AddForce(transform.forward * xMove * accSpeed * Time.deltaTime);
                    rb.AddForce(transform.right * zMove * accStrafe * Time.deltaTime);
                }
                else
                {
                    rb.AddForce(transform.forward * xMove * accSpeed * airSpeedMultiplier * Time.deltaTime);
                    rb.AddForce(transform.right * zMove * accStrafe * airSpeedMultiplier * Time.deltaTime);
                }
            }
            else
            {
                if (isOnGround)
                {
                    rb.AddForce(transform.forward * xMove * accSprint * accSpeed * Time.deltaTime);
                    rb.AddForce(transform.right * zMove * accStrafe * Time.deltaTime);
                }
                else
                {
                    rb.AddForce(transform.forward * xMove * accSprint * airSpeedMultiplier * accSpeed * Time.deltaTime);
                    rb.AddForce(transform.right * zMove * accStrafe * airSpeedMultiplier * Time.deltaTime);
                }
            }
        }
        else
        {
            // if (Mathf.Abs(HorizontalVelocity.magnitude) > minimumThresholdSpeed)
            //     rb.AddForce(-MovementVector * slowDownSpeed * Time.deltaTime);
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
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                InvokeRepeating("resetJump", 0, 0.02f);
            }
        }

        //wallRunning (vertical, up a wall)
        if (isFacingWall && !isOnGround && !hasJustBeenAgainstWall)
        {
            if (controls.PlayerMovement.Jump.triggered)
            {
                rb.velocity = new Vector3(0, 0, 0);
                rb.AddForce(Vector3.up * wallJumpForce, ForceMode.Impulse);

                hasJustBeenAgainstWall = true;
            }
        }
        if (hasJustBeenAgainstWall && isOnGround) hasJustBeenAgainstWall = false;

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

                Invoke("resetSideJump", 0.2f);
            }
        }
        if (isOnGround && lastRunDir != 0) lastRunDir = 0;//when player touches ground, they can now wall run off the same side wall

        //wall sliding
        // if player is beside wall, in the air and holding slide, then attach em to that wall and slow them down like it's a slide.
        wallSliding();

        //ledge grabbing
        //is pressing crouch button, is next to a hangable ledge, is facing the wall and isnt actually climbing it,
        if (isTryingToSlide && isAgainstLedge && isFacingWall && !isClimbing)
        {
            // ani.SetBool("Climb", true);
            isHangingOnWall = true;//hold player against wall

            if (controls.PlayerMovement.Jump.triggered)//if they jump, then
            {
                //  ani.SetTrigger("Pull Up");
                isHangingOnWall = false;//release them from wall

                rb.velocity = new Vector3(0, 0, 0);//zero current velocity
                rb.AddForce(transform.forward * ClimbForwardsForce, ForceMode.Impulse);//boost them above wall

                isClimbing = true;//ensure they wont immediatley stick back to wall
                Invoke("resetClimb", 1f);//reset ^that bool in 1 second (when theyve cleared it)
            }
        }
        else
        {
            isHangingOnWall = false;
            ani.SetBool("Climb", false);
        }
    }

    public void wallSliding()
    {

        if (isTryingToSlide && controls.PlayerMovement.Jump.ReadValue<float>() == 0)//is the buttom being pressed and not jumping?
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

        if (!isWallRunning && !hasJustWallRun) InvokeRepeating("resetWallRun", 0, 0.02f);

        if (isWallRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (wallRunDir == 1) ani.SetInteger("WallSlide", 1);
            if (wallRunDir == 2) ani.SetInteger("WallSlide", 2);
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
            if (isOnGround && isSprinting && HorizontalVelocityf >= minSlideSpeed && !hasSlid)//are you sprinting, moving faster than minimum and havent just exited a slide?
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
        {
            ani.SetBool("slide", true);
        }
        else
        {
            ani.SetBool("slide", false);
        }
    }

    private void GetIKTarget(GameObject vault)
    {
        GameObject grabPoint = null;
        float grabDist = 100;
        foreach (GameObject point in vault.GetComponentInParent<VaultTargetHandler>().Targets)
        {
            if (math.distance(gameObject.transform.position, point.transform.position) <= grabDist)
            {
                grabPoint = point;
                grabDist = math.distance(gameObject.transform.position, point.transform.position);
            }
        }

        if (grabPoint == null)
        {
            playerIK.ikActive = false;
        }
        else
        {
            playerIK.vaultObject = grabPoint.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("VaultTrigger"))
        {
            isInVaultTrigger = true;

            GetIKTarget(other.gameObject); //Find the point you grab
            playerIK.IKTime = IKGrabTime; //Reset the player's grab time
        }

        if (other.gameObject.CompareTag("ClimbTrigger"))
        {
            Transform[] childrenTransforms = other.transform.GetComponentsInChildren<Transform>();
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
            }
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
        if (isHangingOnWall)
        {
            rb.drag = GrapplingDrag;
        }
        else if (!isOnGround)//is in air
        {
            rb.drag = inAirDrag;
            
        }
        else if (!isTryingToSlide)//is simply on ground
        {
            rb.drag = standardDrag;
        }
        else if (isSliding || isWallRunning)//is sliding or running horizontally across a wall
        {
            rb.drag = slidingDrag;
        }
        else if (isTryingToSlide && !isSliding && !isSprinting)//is crouching (pressing button but not sliding and sprinting)
        {
            rb.drag = crouchingDrag;
        }

        if (!hasJustVaulted)
        {
            if (!isSprinting)
            {
                if (HorizontalVelocityf > maxMoveSpeed)
                {
                    Vector3 limitedVel = HorizontalVelocity.normalized * (maxMoveSpeed - 0.1f);
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }
            else
            {
                if (HorizontalVelocityf > maxSprintSpeed)
                {
                    Vector3 limitedVel = HorizontalVelocity.normalized * (maxSprintSpeed - 0.1f);
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }
        }
    }

    void resetWallRun()
    {
        if (wallRunDir != 0 && !wallRunCheck) hasJustWallRun = true;
        if (wallRunDir == 0 && !wallRunCheck) wallRunCheck = true;
        if (wallRunDir != 0 && wallRunCheck) hasJustWallRun = false;

        if (hasJustWallRun) CancelInvoke("resetWallRun");
    }

    void resetJump()
    {
        if (isOnGround && !jumpCheck) canJump = false;//if Youre on the ground but havent left the ground yet, you can't jump again.
        if (!jumpCheck && !isOnGround) jumpCheck = true;//if you arent on ground but the bool hasnt updated, then update the bool
        if (isOnGround && jumpCheck) canJump = true;// if youre on the ground and youve been checked to be in air, then you can jump again

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
        isClimbing = false;
        ani.ResetTrigger("Pull Up");
    }

    //generates a vector that faces the direction the player is moving
    //is reversed to slow them down
    void calculateMovementVector()
    {
        MovementVector = transform.position - lastPos;
        lastPos = transform.position;
    }
}
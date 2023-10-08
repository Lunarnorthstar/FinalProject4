using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    PlayerManager playerManager;

    [Header("Movement Characteristics")]
    public float maxMoveSpeed;
    public float accSpeed;
    public float maxStrafeSpeed;
    public float accStrafe;
    public float slowDownSpeed;
    public float minimumThresholdSpeed;

    [Space]
    public float jumpForce;

    [Space]
    public float yRotSpeed;
    public float zRotSpeed;

    public Transform cameraHolder;

    [SerializeField] Vector3 HorizontalVelocity;
    [SerializeField] Vector3 MovementVector;

    Vector3 lastPos;
    //[SerializeField] Vector3 HorizontalVelocity;

    float xMove;
    float zMove;

    float xRot;
    float yRot;

    bool isAtMaxSpeed;
    bool canJump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();

        canJump = true;

        InvokeRepeating("calculateMovementVector", 0, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        zMove = Input.GetAxis("Horizontal");
        xMove = Input.GetAxis("Vertical");

        xRot = Input.GetAxis("Mouse Y");
        yRot = Input.GetAxis("Mouse X");

        HorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (Mathf.Abs(HorizontalVelocity.magnitude) >= maxMoveSpeed) isAtMaxSpeed = true;
        else isAtMaxSpeed = false;

        if (Input.GetKeyDown(KeyCode.P)) playerManager.lockMouse();//locking the mouse
        if (Input.GetKeyDown(KeyCode.Space)) jump();

    }

    void FixedUpdate()
    {
        if (!isAtMaxSpeed)
        {
            rb.AddForce(transform.forward * (xMove * accSpeed));
            rb.AddForce(transform.right * (zMove * accSpeed));
        }

        //if player is moving faster than a rlly small amount, slow them down
        if (Mathf.Abs(HorizontalVelocity.magnitude) > minimumThresholdSpeed)
            rb.AddForce(-MovementVector * slowDownSpeed);

        //transform.Rotate(Vector3.up * yRot * yRotSpeed);
        //cameraHolder.Rotate(cameraHolder.right * -xRot * zRotSpeed);
        // cameraHolder.localRotation = quaternion.Euler(cameraHolder.localRotation.x, 0, 0);
    }

    void calculateMovementVector()
    {
        MovementVector = transform.position - lastPos;
        lastPos = transform.position;
    }

    void jump()
    {
        if (canJump)
        {
            if (playerManager.isOnGround())
            {
                rb.AddForce(Vector3.up * jumpForce);
                StartCoroutine(jumpWait());
            }
        }
    }

    IEnumerator jumpWait()
    {
        canJump = false;
        yield return new WaitForSeconds(0.5f);
        canJump = true;
    }

}

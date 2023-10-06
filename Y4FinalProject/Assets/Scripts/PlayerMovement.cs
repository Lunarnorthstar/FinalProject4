using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    [Header("Movement Characteristics")]
    public float maxMoveSpeed;
    public float accSpeed;
    public float maxStrafeSpeed;
    public float accStrafe;

    [Space]
    public float yRotSpeed;
    public float zRotSpeed;

    public Transform cameraHolder;

    [SerializeField] Vector3 HorizontalVelocity;
    //[SerializeField] Vector3 HorizontalVelocity;

    float xMove;
    float zMove;

    float xRot;
    float yRot;

    bool isAtMaxSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

    }

    void FixedUpdate()
    {
        if (!isAtMaxSpeed)
        {
            rb.AddForce(transform.forward * (xMove * accSpeed));
            rb.AddForce(transform.right * (zMove * accSpeed));
        }

        transform.Rotate(Vector3.up * yRot * yRotSpeed);
        //cameraHolder.Rotate(cameraHolder.right * -xRot * zRotSpeed);
        // cameraHolder.localRotation = quaternion.Euler(cameraHolder.localRotation.x, 0, 0);


    }

}

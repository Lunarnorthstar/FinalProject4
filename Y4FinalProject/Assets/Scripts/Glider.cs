using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Glider : MonoBehaviour
{
    public bool isEnabled;

    [Space]
    public Transform cam;
    PlayerMovement playerMovement;
    Rigidbody rb;

    [Space]


    [Header("Glider CHaracteristics")]
    public float liftCoE;
    public float forwardForce;

    [Space]
    public AnimationCurve liftCurve;
    public Vector3 lookVector;
    public Vector3 travelVector;

    [Header("Debug")]
    [SerializeField] float horVel;
    [SerializeField] float totalLift;
    [SerializeField] float angleOfAttack;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            lookVector = new Vector3(
                cam.transform.localRotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            0);

            travelVector = playerMovement.HorizontalVelocity;

            horVel = playerMovement.HorizontalVelocityf;
            totalLift = liftCurve.Evaluate(horVel) * liftCoE;
        }
    }

    void FixedUpdate()
    {
        if (isEnabled)
        {
            angleOfAttack = Vector3.Angle(travelVector, lookVector);

            rb.AddForce(transform.forward * forwardForce);
            rb.AddForce(Vector3.up * totalLift);
        }
    }
}

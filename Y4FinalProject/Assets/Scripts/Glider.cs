using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Glider : MonoBehaviour
{
    public bool isEnabled;

    [Space]
    public Transform cam;
    PlayerMovement playerMovement;
    Powerups powerups;
    Rigidbody rb;

    [Space]
    [Header("Glider CHaracteristics")]
    public float liftCoE;
    public float forwardForce;
    public float time;

    [Space]
    public AnimationCurve liftCurve;
    public AnimationCurve AoTCurve;
    public AnimationCurve SpeedCurve;
    public AnimationCurve dragOverTime;
    public Vector3 lookVector;
    public Vector3 travelVector;

    [Header("Debug")]
    [SerializeField] float horVel;
    [SerializeField] float totalVel;
    [SerializeField] float totalLift;
    [SerializeField] float angleOfAttack;
    [SerializeField] float totalForwardsForce;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        powerups = GetComponent<Powerups>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            playerMovement.isGliding = true;

            horVel = playerMovement.HorizontalVelocityf;
            totalVel = rb.velocity.magnitude;

            totalLift = liftCurve.Evaluate(totalVel) * SpeedCurve.Evaluate(cam.transform.localRotation.eulerAngles.x) *
             liftCoE * AoTCurve.Evaluate(angleOfAttack);

            //the total lift is affected by
            //1 speed, 2, angle of attack, and 3, lift multiplier (lift CoE)
            //the AOT curve will muliply lift depnding on how far up or down the camera is pointed, pushing u up or down
            //the speed curve adds lift the faster u go allowing you to dive to gain speed and then shoot up in the air
            //if the anglw of attack goes too high, then you start to fall out of the sky again.

            lookVector = cam.transform.localRotation.eulerAngles;

            totalForwardsForce = SpeedCurve.Evaluate(cam.transform.localRotation.eulerAngles.x) * forwardForce * dragOverTime.Evaluate(time);
            //the forwards force will reduce when you climb (high AOT) and increase when you dive (low AOT)

            if (!powerups.isCountingDown)
            {
                isEnabled = false;
            }

            time++;
        }
        else
        {
            playerMovement.isGliding = false;
            time = 0;
        }
    }

    void FixedUpdate()
    {
        if (isEnabled)
        {
            var localVelocity = cam.transform.InverseTransformDirection(rb.velocity);
            angleOfAttack = Mathf.Atan2(-localVelocity.y, localVelocity.z);
            angleOfAttack *= Mathf.Rad2Deg;

            rb.AddForce(transform.forward * totalForwardsForce);
            rb.AddForce(Vector3.up * totalLift);
        }
    }
}

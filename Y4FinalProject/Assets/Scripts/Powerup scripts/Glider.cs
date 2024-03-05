using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Glider : MonoBehaviour
{
    public bool isEnabled;

    [Space]
    public Transform cam;
    PlayerMovement playerMovement;
    Powerups powerups;
    Rigidbody rb;

    [Space]
    [Header("Glider Characteristics")]
    public float liftCoE;
    public float forwardForce;
    private float time;
    public float glideDuration = 8;
    public float glideCooldown = 4;
    private bool isCoolingDown = false;
    private float cooldownTimer = 0;
    [HideInInspector] public bool ready = false;


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
        if (!isCoolingDown && !isEnabled)
        {
            ready = true;
        }
        else
        {
            ready = false;
        }



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

            time += Time.deltaTime;

            if (time >= glideDuration)
            {
                isEnabled = false;
                isCoolingDown = true;
            }
        }
        else
        {
            playerMovement.isGliding = false;
            time = 0;
        }

        if (isCoolingDown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= glideCooldown)
            {
                cooldownTimer = 0;
                isCoolingDown = false;
            }
        }

        if (equipped)
        {
            UpdateUI();
        }
    }

    [HideInInspector] public TextMeshProUGUI countdown;
    [HideInInspector] public Slider slider;
    [HideInInspector] public TextMeshProUGUI name;
    [HideInInspector] public bool equipped = false;
    public void UpdateUI()
    {
        name.text = "Glider";

        if (ready)
        {
            countdown.text = " ";
        }


        if (isCoolingDown && !isEnabled)
        {
            countdown.text = CleanTimeConversion(glideCooldown - cooldownTimer, 2);
            slider.value = cooldownTimer / glideCooldown;
        }

        if (isEnabled)
        {
            countdown.text = CleanTimeConversion(glideDuration - time, 2);
            slider.value = 1 - time / glideDuration;
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

    public void Activate()
    {
        if (!isCoolingDown)
        {
            isEnabled = true;

            AudioManager.instance.GenerateSound(AudioReference.instance.glideDeploy, Vector3.zero);
        }
    }

    public string CleanTimeConversion(float rawTime, int Dplaces)
    {
        int minutes = Mathf.FloorToInt(rawTime / 60);
        int seconds = Mathf.FloorToInt(rawTime - minutes * 60);
        int milliseconds = Mathf.FloorToInt((rawTime - (minutes * 60) - seconds) * (math.pow(10, Dplaces)));



        string timeReadable = string.Format("{0:00}.{1:0}", seconds, milliseconds);
        return timeReadable;
    }
}

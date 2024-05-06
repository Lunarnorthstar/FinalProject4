using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    public Camera playerCam;
    [Tooltip("The distance in units the player moves when blinking")] public float blinkDistance = 3;
    [Tooltip("The time in seconds between blinks")] public float cooldown = 3;
    public ParticleSystem particles;

    private float cooldownTimer = 0;
    private bool coolingDown = false;
    public GameObject camera;
    public GameObject player;
    [HideInInspector] public bool ready = false;
    public Valid checker;
    
    private Animator walkAni;

    private void Start()
    {
        walkAni = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerCam.transform.forward, out hit, blinkDistance))
        {
            if (hit.collider.CompareTag("Blinkblock"))
            {
                checker.unblocked = false;
            }
            else
            {
                checker.unblocked = true;
            }
        }
        else
        {
            checker.unblocked = true;
        }
        
        
        ready = !coolingDown;


        if (coolingDown)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= cooldown)
            {
                cooldownTimer = 0;
                coolingDown = false;
            }
        }
        if (equipped)
        {
            UpdateUI();
        }
    }

    [HideInInspector] public TextMeshProUGUI countdown;
    [HideInInspector] public Slider slider;
    [HideInInspector] public bool equipped = false;
    public void UpdateUI()
    {
        countdown.text = " ";

        if (coolingDown)
        {
            countdown.text = CleanTimeConversion(cooldown - cooldownTimer, 2);
            slider.value = cooldownTimer / cooldown;
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
        }
        else
        {
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.green;
        }
    }

    public void Activate()
    {
        RaycastHit hit; //Get ready to store the target of a raycast
        if (Physics.Raycast(transform.position, playerCam.transform.forward, out hit, blinkDistance)) //Raycast. If it hits something...
        {
            if (hit.collider.CompareTag("Blinkblock")) //If that something has the Blinkblock tag...
            {
                GetComponent<JuiceBehaviours>().playPowerupAni(false); //Don't work.
                return; //Stop trying to blink.
            }
            else
            {
                RaycastHit secondHit; //Get ready to store another target of a raycast...
                if (Physics.Raycast(transform.position + Vector3.down, playerCam.transform.forward, out secondHit, blinkDistance)) //Raycast again. If it hits something...
                {
                    if (secondHit.collider.CompareTag("Blinkblock")) //If that something has the Blinkblock tag...
                    {
                        GetComponent<JuiceBehaviours>().playPowerupAni(false); //Don't work.
                        return; //Stop trying to blink.
                    }
                }
            }
        }

        //If you didn't hit anything invalid...

        if (!coolingDown) //If you're not on cooldown...
        {
            GetComponent<PlayerMovement>().StopClimb();
            walkAni.SetBool("isMidJump", false);
            GetComponent<JuiceBehaviours>().playPowerupAni(true);

            Vector3 blinkDirection = Vector3.Normalize(camera.transform.forward);

            player.transform.position += (blinkDirection * blinkDistance);

            particles.Play();

            coolingDown = true;

            AudioManager.instance.GenerateSound(AudioReference.instance.blinkDeploy, Vector3.zero);
        }
        else
        {
            GetComponent<JuiceBehaviours>().playPowerupAni(false);
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

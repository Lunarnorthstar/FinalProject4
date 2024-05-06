using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public float minMoveSpeedMult = 1.2f;
    public float shieldDuration = 4;
    public float shieldCooldown = 4;
    private float shieldTimer = 0;
    private float cooldownTimer = 0;
    private bool coolingDown = false;
    private bool active;
    public PlayerMovement player;
    public GameObject shieldObject;
    [HideInInspector] public bool ready = false;
    
    private Animator walkAni;

    private void Start()
    {
        walkAni = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!coolingDown && !active)
        {
            ready = true;
        }
        else
        {
            ready = false;
        }



        if (active)
        {
            shieldObject.SetActive(true);
            shieldTimer += Time.deltaTime;
            player.moveSpeedMult = minMoveSpeedMult;

            if (shieldTimer >= shieldDuration)
            {
                player.moveSpeedMult = 1;
                shieldObject.SetActive(false);
                shieldTimer = 0;
                active = false;
                coolingDown = true;
            }
        }

        player.isAffectedByTerrain = !active;

        if (coolingDown && !active)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= shieldCooldown)
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
        if (ready)
        {
            countdown.text = " ";
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.green;
        }

        if (coolingDown && !active)
        {
            countdown.text = CleanTimeConversion(shieldCooldown - cooldownTimer, 2);
            slider.value = cooldownTimer / shieldCooldown;
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;

        }

        if (active)
        {
            countdown.text = CleanTimeConversion(shieldDuration - shieldTimer, 2);
            slider.value = 1 - shieldTimer / shieldDuration;

        }
    }

    public void Activate()
    {
        if (!coolingDown)
        {
            if (!active)
            {
                walkAni.SetBool("isMidJump", false);
                AudioManager.instance.GenerateSound(AudioReference.instance.shieldDeploy, Vector3.zero);
                active = true;
                GetComponent<JuiceBehaviours>().playPowerupAni(true);
            }
            else
            {
                GetComponent<JuiceBehaviours>().playPowerupAni(false);
            }
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

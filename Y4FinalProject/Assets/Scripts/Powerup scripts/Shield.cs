using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public float shieldDuration = 4;
    public float shieldCooldown = 4;
    private float shieldTimer = 0;
    private float cooldownTimer = 0;
    private bool coolingDown = false;
    private bool active;
    public PlayerMovement player;
    public GameObject shieldObject;


    private void Start()
    {
        countdown = player.GetComponent<Powerups>().countdownText;
        slider = player.GetComponent<Powerups>().powerupSlider;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            shieldObject.SetActive(true);
            shieldTimer += Time.deltaTime;

            if (player.moveSpeedMult < 1) player.moveSpeedMult = 1;
                
            if (player.jumpHeightMult < 1) player.jumpHeightMult = 1;
            
            player.fricitonMult = 1;

            if (shieldTimer >= shieldDuration)
            {
                shieldObject.SetActive(false);
                shieldTimer = 0;
                active = false;
                coolingDown = true;
            }
        }

        if (coolingDown && !active)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= shieldCooldown)
            {
                cooldownTimer = 0;
                coolingDown = false;
            }
        }
        UpdateUI();
    }
    
    private TextMeshProUGUI countdown;
    private Slider slider;
    public void UpdateUI()
    {
        if (!coolingDown)
        {
            countdown.text = " ";
        }
        
        if (coolingDown && !active)
        {
            countdown.text = (shieldCooldown - cooldownTimer).ToString();
            slider.value = cooldownTimer / shieldCooldown;
        }

        if (active)
        {
            countdown.text = (shieldDuration - shieldTimer).ToString();
            slider.value = 1 - shieldTimer / shieldDuration;
        }
    }

    public void Activate()
    {
        if (!coolingDown)
        {
            active = true;
        }
    }
}

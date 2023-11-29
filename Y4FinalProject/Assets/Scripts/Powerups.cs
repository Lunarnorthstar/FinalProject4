using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Timers;
using Polybrush;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Powerups : MonoBehaviour
{
    public CameraMove cam;
    public PlayerMovement playerMovement;
    Rigidbody rb;

    [Space]
    public int currentPowerUp;
    public int maxPowerUps;
    public float powerUpReloadTime;

    [Space]
    public TextMeshProUGUI currentPowerUpText;
    public Slider timerSlider;

    public Image sliderImage;
    public Image indicatorImage;

    public Color notDoneColour;
    public Color DoneColour;

    bool canPowerUp = true;

    [Header("Blip Settings")]
    [Tooltip("how long the blip will be")] public float maxBlipTime;
    [Tooltip("how fast you will blip")] public float blipForce;
    [Tooltip("the fov thats added when blipping")] public float blipFovAdd;
    [Tooltip("the tag the object must have to blip through it")] public string blipTag;
    bool canBlip;
    int currentBlipIndex;

    [Header("Dash Settings")]
    public bool isDashing;
    public float maxDashTime;
    public float dashForce;
    public float dashFovChange;
    int currentDashIndex;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        changePowerUp(true);
        changePowerUp(true);
    }

    void FixedUpdate()
    {
        if (timerSlider.value < timerSlider.maxValue)
        {
            timerSlider.value++;
            canPowerUp = false;
            sliderImage.color = notDoneColour;
        }
        else
        {
            canPowerUp = true;
            sliderImage.color = DoneColour;
        }
        timerSlider.maxValue = powerUpReloadTime;

        //change indicator colour
        if (currentPowerUp == 1)
        {
            if (playerMovement.wallFacingTag == blipTag)
            {
                canBlip = true;
                indicatorImage.color = DoneColour;
            }
            else
            {
                canBlip = false;
                indicatorImage.color = notDoneColour;
            }
        }
        else//blip is the only one that has conditions
        {
            indicatorImage.color = DoneColour;
        }
    }

    public void ActivatePowerup()
    {
        if (!canPowerUp) return;

        switch (currentPowerUp)
        {
            case 1://blip
                if (canBlip)
                {
                    currentBlipIndex = 0;
                    cam.changeFov(blipFovAdd);

                    timerSlider.value = 0;

                    InvokeRepeating("blip", 0, 0.01f);
                }
                break;

            case 2://dash
                timerSlider.value = 0;

                currentDashIndex = 0;
                cam.changeFov(dashFovChange);

                isDashing = true;

                InvokeRepeating("dash", 0, 0.01f);
                break;
        }
    }

    public void changePowerUp(bool up)
    {
        if (currentPowerUp == maxPowerUps) currentPowerUp = 1;
        else
        {
            if (up) currentPowerUp++;
            else currentPowerUp--;
        }

        switch (currentPowerUp)
        {
            case 1://blip
                currentPowerUpText.text = "Blip";
                break;
            case 2://dash
                currentPowerUpText.text = "Dash";
                break;
        }

        currentPowerUpText.GetComponent<Animator>().Play("Change State");
    }

    //this function is called 100times a second (every 0.01sec)
    //every time its called the "current blip index" goes up. once reaching the maxBliptime x 100 it will stop.
    //while its called it will translate you forward based on the blip force
    public void blip()
    {
        if (currentBlipIndex > (maxBlipTime * 100))//100 because this function is called 100 times a sec
        {
            CancelInvoke("blip");

            cam.changeFov(0);

            return;
        }

        transform.Translate(0, 0, blipForce);
        currentBlipIndex++;
    }

    public void dash()
    {
        if (currentDashIndex > (maxDashTime * 100))//100 because this function is called 100 times a sec
        {
            CancelInvoke("dash");

            cam.changeFov(0);

            isDashing = false;

            return;
        }

        rb.AddForce(transform.forward * dashForce, ForceMode.Force);
        currentDashIndex++;
    }
}

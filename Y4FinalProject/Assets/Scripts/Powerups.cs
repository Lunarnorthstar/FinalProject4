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
    public string[] powerUpList;

    [Space]
    public TextMeshProUGUI currentPowerUpText;
    public Slider timerSlider;
    public Slider countDownSlider;

    public Image sliderImage;
    public Image indicatorImage;
    public Image countDownImage;

    public Color notDoneColour;
    public Color DoneColour;

    bool canPowerUp = true;
    bool isInAbility;
    public bool isCountingDown;

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

    [Header("Glider")]
    Glider glider;
    public float gliderDuration;

    [Header("Shield")]
    public bool isShielded;
    public GameObject shield;
    public float shieldDuration;

    //to enable a power up, it must be added to the "activatePowerup" method.
    //then it must evanually be canceled somehow, if by timer then add the "toggleCountdown" method
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        glider = GetComponent<Glider>();

        changePowerUp(true);
        changePowerUp(true);
    }

    void FixedUpdate()
    {
        //if the powerup you have is below 10% duration, it will play animation and go red
        if (countDownSlider.normalizedValue < 0.3f)
        {
            if (isCountingDown)
            {
                countDownSlider.GetComponent<Animator>().SetBool("low", true);
                countDownImage.color = notDoneColour;
            }
        }
        else
        {
            countDownImage.color = DoneColour;
        }

        //if not in an ability and the timer isnt already at max, add to it. otherwise go green.
        if (timerSlider.value < timerSlider.maxValue && !isInAbility)
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

    void toggleCountdown(float countDownDuration)
    {
        if (!isCountingDown)
        {
            countDownSlider.GetComponent<Animator>().Play("Powerup Count Down");
            isCountingDown = true;

            countDownSlider.maxValue = countDownDuration;
            countDownSlider.value = countDownDuration;

            InvokeRepeating("advanceCountdown", 0, 0.1f);

            countDownSlider.GetComponent<Animator>().SetBool("low", false);
        }
        else
        {
            countDownSlider.value = 0;
            countDownSlider.GetComponent<Animator>().Play("Powerup Count Down 0");
            isCountingDown = false;
        }
    }

    void advanceCountdown()
    {
        if (countDownSlider.value > 0)
            countDownSlider.value--;
        else
        {
            CancelInvoke("advanceCountdown");
            toggleCountdown(0);
        }
    }

    void Update()
    {
        if (glider.isEnabled)
        {
            if (playerMovement.isOnGround)
            {
                glider.isEnabled = false;
                isInAbility = false;
            }
        }
        if (isShielded && !isCountingDown)//timer has run out
        {
            isShielded = false;
            shield.SetActive(false);
        }
    }

    public void ActivatePowerup()
    {
        if (!canPowerUp) return;

        isInAbility = true;

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

            case 3://glider
                if (!playerMovement.isOnGround)
                {
                    toggleCountdown(gliderDuration);
                    glider.isEnabled = true;
                    timerSlider.value = 0;
                }
                break;
            case 4: //shield
                toggleCountdown(shieldDuration);
                shield.SetActive(true);
                isShielded = true;

                break;
        }
    }

    public void changePowerUp(bool up)
    {
        if (!isInAbility)
        {
            if (currentPowerUp == maxPowerUps) currentPowerUp = 1;
            else
            {
                if (up) currentPowerUp++;
                else currentPowerUp--;
            }
            currentPowerUpText.text = powerUpList[currentPowerUp - 1];

            currentPowerUpText.GetComponent<Animator>().Play("Change State");
        }
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

            isInAbility = false;

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

            isInAbility = false;

            return;
        }

        rb.AddForce(transform.forward * dashForce, ForceMode.Force);
        currentDashIndex++;
    }
}

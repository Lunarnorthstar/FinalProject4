using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Timers;
using Polybrush;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Powerups : MonoBehaviour
{
    //Following the changes, this script now only handles whether a powerup is equipped or not, and handles the UI. Individual powerups have been moved to their own scripts for readability's sake.

    public Dash dash;
    public Glider glider;
    public Blink blink;
    public GameObject blinkShadow;
    public GameObject camera;
    public Shield shield;
    public GrappleHook grappleHook;

    [Header("UI")]
    public GameObject[] powerupUI; //The overarching object
    private TextMeshProUGUI[] countdownText; //The text displaying the cooldown or duration countdown
    private Slider[] powerupSlider; //The slider displaying the percent of cooldown/countdown
    private TextMeshProUGUI[] powerupText; //The name of the powerup
    private Image[] powerupImage; //The white box behind the name
    private Image[] indicatorImage; //The small green box
    private String[] letter = new[] { "A", "B" }; //For iterative purposes


    [Space]
    public PowerupData PD;
    public List<string> equippedPowerups;

    private int slotSelected = 0;

    private void Start()
    {
        powerupSlider = new Slider[powerupUI.Length];
        powerupText = new TextMeshProUGUI[powerupUI.Length];
        countdownText = new TextMeshProUGUI[powerupUI.Length];
        powerupImage = new Image[powerupUI.Length];
        indicatorImage = new Image[powerupUI.Length]; //Initialize all the arrays


        for (int i = 0; i < powerupUI.Length; i++) //Fill all the arrays.
        {
            powerupSlider[i] = powerupUI[i].GetComponentInChildren<Slider>();
            powerupImage[i] = powerupUI[i].GetComponentInChildren<Image>();

            powerupText[i] = GameObject.FindWithTag("PowerText" + letter[i]).GetComponent<TextMeshProUGUI>();
            countdownText[i] = GameObject.FindWithTag("CountText" + letter[i]).GetComponent<TextMeshProUGUI>();
            indicatorImage[i] = GameObject.FindWithTag("Indicator" + letter[i]).GetComponent<Image>();
            //We need tags here because both text objects are in the "child". I wanted to minimize setup which ended up maximizing code.
        }
        equippedPowerups = PD.equippedPowerups;

        updateUI();
    }


    private void Update()
    {
        if (equippedPowerups[slotSelected] == "blink") //If you've got Blink selected...
        {
            blinkShadow.SetActive(true); //Set the shadow object to be enabled so you can see it.
            blinkShadow.transform.position =
                gameObject.transform.position + (blink.blinkDistance * camera.transform.forward) + new Vector3(0, 0.3f, 0); //Set its position to where you will blink to
        }
        else
        {
            blinkShadow.SetActive(false); //if you don't have blink selected set the shadow object to be inactive so you don't see it anymore.
        }

        if (equippedPowerups[slotSelected] == "grapple")
        {
            grappleHook.grappleShadow.SetActive(true);
        }
        else
        {
            grappleHook.grappleShadow.SetActive(false);
        }



        //Cooldown color changing
        for (int i = 0; i < powerupUI.Length; i++)
        {
            if (equippedPowerups.Count <= i)
            {
                indicatorImage[i].color = Color.red;
                break;
            }
            
            switch (equippedPowerups[i])
            {
                case "dash":
                    if (!dash.ready) indicatorImage[i].color = Color.red;
                    else indicatorImage[i].color = Color.green;
                    break;
                case "glider":
                    if (!glider.ready) indicatorImage[i].color = Color.red;
                    else indicatorImage[i].color = Color.green;
                    break;
                case "blink":
                    if (!blink.ready) indicatorImage[i].color = Color.red;
                    else indicatorImage[i].color = Color.green;
                    break;
                case "shield":
                    if (!shield.ready) indicatorImage[i].color = Color.red;
                    else indicatorImage[i].color = Color.green;
                    break;
                case "grapple":
                    if (!grappleHook.ready) indicatorImage[i].color = Color.red;
                    else indicatorImage[i].color = Color.green;
                    break;
                default: break;
            }
        }

        //Selecting color changing
        for (int i = 0; i < powerupImage.Length; i++)
        {
            powerupImage[i].color = Color.white;
        }
        powerupImage[slotSelected].color = Color.green;

    }


    public void updateUI()
    {
        for (int i = 0; i < powerupUI.Length; i++) //For each powerup you have...
        {
            if (equippedPowerups.Count <= i)
            {
                powerupText[i].text = "None";
                break;
            }
            
            //This is all so ugly but I have too much tech debt to do it any other way :(
            switch (equippedPowerups[i]) //Hook up all the powerup script's UI elements.
            {
                case "dash":
                    dash.equipped = true;
                    dash.name = powerupText[i];
                    dash.slider = powerupSlider[i];
                    dash.countdown = countdownText[i];
                    break;
                case "glider":
                    glider.equipped = true;
                    glider.name = powerupText[i];
                    glider.slider = powerupSlider[i];
                    glider.countdown = countdownText[i];
                    break;
                case "blink":
                    blink.equipped = true;
                    blink.name = powerupText[i];
                    blink.slider = powerupSlider[i];
                    blink.countdown = countdownText[i];
                    break;
                case "shield":
                    shield.equipped = true;
                    shield.name = powerupText[i];
                    shield.slider = powerupSlider[i];
                    shield.countdown = countdownText[i];
                    break;
                case "grapple":
                    grappleHook.equipped = true;
                    grappleHook.name = powerupText[i];
                    grappleHook.slider = powerupSlider[i];
                    grappleHook.countdown = countdownText[i];
                    break;
                default:
                    Debug.Log("Oops! " + equippedPowerups[slotSelected] + " is not a valid powerup name. The valid names are; dash, glider, blink, shield, and grapple (case sensitive)");
                    powerupText[i].text = "None";
                    break;
            }
        }
    }

    public void SwitchPowerup()
    {
        slotSelected++;
        if (slotSelected > equippedPowerups.Count - 1)
        {
            slotSelected = 0;
        }
    }

    public void ActivatePowerup()
    {
        switch (equippedPowerups[slotSelected])
        {
            case "dash": dash.Activate(); break;
            case "glider": glider.Activate(); break;
            case "blink": if (blinkShadow.GetComponent<Valid>().validPosition) blink.Activate(); break;
            case "shield": shield.Activate(); break;
            case "grapple": grappleHook.Activate(); break;
            default: break;
        }
    }

    public void OffBreakGrapple()
    {
        grappleHook.DeactivateGrapple();
    }
}

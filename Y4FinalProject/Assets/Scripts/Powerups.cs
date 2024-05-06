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

    private PlayerMovement PM;
    
    public Dash dash;
    public Glider glider;
    public Blink blink;
    public GameObject blinkShadow;
    public GameObject camera;
    public Shield shield;
    public GrappleHook grappleHook;

    public Texture dashImage;
    public Texture gliderImage;
    public Texture blinkImage;
    public Texture bootsImage;
    public Texture grappleImage;

    [Header("UI")]
    public GameObject[] powerupUI; //The overarching object
    private TextMeshProUGUI[] countdownText; //The text displaying the cooldown or duration countdown
    private Slider[] powerupSlider; //The slider displaying the percent of cooldown/countdown
    private RawImage[] powerupImage; //The white box behind the name
    private Image[] indicatorImage; //The small green box
    private String[] letter = new[] { "A", "B" }; //For iterative purposes


    [Space]
    public PowerupData PD;
    public List<string> equippedPowerups;

    private int slotSelected = 0;

    private void Start()
    {
        PM = GetComponent<PlayerMovement>();
        powerupSlider = new Slider[powerupUI.Length];
        countdownText = new TextMeshProUGUI[powerupUI.Length];
        powerupImage = new RawImage[powerupUI.Length];
        indicatorImage = new Image[powerupUI.Length]; //Initialize all the arrays


        for (int i = 0; i < powerupUI.Length; i++) //Fill all the arrays.
        {
            powerupSlider[i] = powerupUI[i].GetComponentInChildren<Slider>();
            powerupImage[i] = powerupUI[i].GetComponentInChildren<RawImage>();

            countdownText[i] = powerupUI[i].GetComponentInChildren<TextMeshProUGUI>();
            indicatorImage[i] = GameObject.FindWithTag("Indicator" + letter[i]).GetComponent<Image>();
            //We need tags here because both text objects are in the "child". I wanted to minimize setup which ended up maximizing code.
        }
        equippedPowerups = PD.equippedPowerups;

        for (int i = 0; i < powerupUI.Length; i++)
        {
            switch (equippedPowerups[i])
            {
                case "dash":
                    powerupImage[i].texture = dashImage;
                    break;
                case "glider":
                    powerupImage[i].texture = gliderImage;
                    break;
                case "blink":
                    powerupImage[i].texture = blinkImage;
                    break;
                case "boots":
                    powerupImage[i].texture = bootsImage;
                    break;
                case "grapple":
                    powerupImage[i].texture = grappleImage;
                    break;
                default:
                    powerupUI[i].SetActive(false);
                    break;
            }
        }





        updateUI();
    }


    private void Update()
    {
        if (equippedPowerups.Contains("blink")) //If you've got Blink selected...
        {
            blinkShadow.transform.position =
                gameObject.transform.position + (blink.blinkDistance * camera.transform.forward) + new Vector3(0, 0.3f, 0); //Set its position to where you will blink to
        }
        else
        {
            blinkShadow.SetActive(false); //if you don't have blink selected set the shadow object to be inactive so you don't see it anymore.
        }

        if (equippedPowerups.Contains("grapple"))
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
                case "boots":
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

        if (!blink.ready)
        {
            blinkShadow.GetComponent<Valid>().ready = false;
        }
        else
        {
            blinkShadow.GetComponent<Valid>().ready = true;
        }


        /* Obsolete now as we don't switch selected powerups
        //Selecting color changing
        for (int i = 0; i < powerupImage.Length; i++)
        {
            powerupImage[i].color = Color.white;
        }
        powerupImage[slotSelected].color = Color.green;*/

    }


    public void updateUI()
    {
        for (int i = 0; i < powerupUI.Length; i++) //For each powerup you have...
        {
            if (equippedPowerups.Count <= i)
            {
                break;
            }

            //This is all so ugly but I have too much tech debt to do it any other way :(
            switch (equippedPowerups[i]) //Hook up all the powerup script's UI elements.
            {
                case "dash":
                    dash.equipped = true;
                    dash.slider = powerupSlider[i];
                    dash.countdown = countdownText[i];
                    powerupImage[i].color = PD.dashColor;
                    break;
                case "glider":
                    glider.equipped = true;
                    glider.slider = powerupSlider[i];
                    glider.countdown = countdownText[i];
                    powerupImage[i].color = PD.glideColor;
                    break;
                case "blink":
                    blink.equipped = true;
                    blink.slider = powerupSlider[i];
                    blink.countdown = countdownText[i];
                    powerupImage[i].color = PD.blinkColor;
                    break;
                case "boots":
                    shield.equipped = true;
                    shield.slider = powerupSlider[i];
                    shield.countdown = countdownText[i];
                    powerupImage[i].color = PD.bootsColor;
                    break;
                case "grapple":
                    grappleHook.equipped = true;
                    grappleHook.slider = powerupSlider[i];
                    grappleHook.countdown = countdownText[i];
                    powerupImage[i].color = PD.grappleColor;
                    break;
                default:
                    Debug.Log("Oops! " + equippedPowerups[i] + " is not a valid powerup name. The valid names are; dash, glider, blink, boots, and grapple (case sensitive)");
                    break;
            }
        }
    }

    public void ActivatePowerup(int selection)
    {
        switch (equippedPowerups[selection - 1])
        {
            case "dash":
                dash.Activate(); break;
            case "glider":
                glider.Activate(); break;
            case "blink": blinkShadow.GetComponent<MeshRenderer>().enabled = true; break;
            case "boots":
                shield.Activate(); break;
            case "grapple":
                grappleHook.Activate(); break;
            default: break;
        }
    }

    public void ReleasePowerup(int selection)
    {
        switch (equippedPowerups[selection - 1])
        {
            case "dash": break;
            case "glider": break;
            case "blink":
                if (blinkShadow.GetComponent<Valid>().validPosition) blink.Activate();
                blinkShadow.GetComponent<MeshRenderer>().enabled = false;
                break;
            case "boots": break;
            case "grapple": break;
            default: break;
        }
    }

    public void OffBreakGrapple()
    {
        //grappleHook.DeactivateGrapple();
    }
}

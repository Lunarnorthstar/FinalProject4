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
    //Following the changes, this script now only handles whether a powerup is equipped or not, and handles the UI. Individual powerups have been moved to their own scripts for readability's sake.
    
    public Dash dash;
    public Glider glider;
    public Blink blink;
    public GameObject blinkShadow;
    public GameObject camera;
    public Shield shield;
    [Space] 
    public List<string> equippedPowerups;

    private int slotSelected = 0;

    private void Update()
    {
        if (equippedPowerups[slotSelected] == "blink")
        {
            blinkShadow.SetActive(true);
            blinkShadow.transform.position =
                gameObject.transform.position + (blink.blinkDistance * camera.transform.forward);
        }
        else
        {
            blinkShadow.SetActive(false);
        }
        
        updateUI();
    }


    public TextMeshProUGUI powerupText;
    public void updateUI()
    {
        powerupText.text = equippedPowerups[slotSelected];
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
            case "blink": if(blinkShadow.GetComponent<Valid>().validPosition)  blink.Activate(); break;
            case "shield": shield.Activate(); break;
            default: break;
        }
    }
}

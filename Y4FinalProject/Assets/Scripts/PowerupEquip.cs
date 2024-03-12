using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupEquip : MonoBehaviour
{
    public PowerupData PD;
    public int maxPowerups = 2;
    public TextMeshProUGUI powerupEquipReadout;
    
    [Space]

    public Image[] powerupColorImage = new Image[2];
    public Color dashColor = Color.blue;
    public Color glideColor = Color.yellow;
    public Color shieldColor = Color.green;
    public Color blinkColor = Color.magenta;
    public Color grappleColor = Color.gray;
    public Color unequippedColor = Color.white;

    public Button dashButton;
    public Button glideButton;
    public Button shieldButton;
    public Button blinkButton;
    public Button grappleButton;

    private Button[] allButtons;

    private void Start()
    {
        allButtons = new[] {dashButton, glideButton, shieldButton, blinkButton, grappleButton}; //Put all the buttons in an array for easier access
        
        foreach (var equip in PD.equippedPowerups)
        {
            switch (equip)
            {
                case "None":
                    break;
                case "dash":
                    dashButton.GetComponentInParent<Animator>().Play("DashSelect");
                    break;
                case "glider":
                    glideButton.GetComponentInParent<Animator>().Play("GlideSelect");
                    break;
                case "shield":
                    shieldButton.GetComponentInParent<Animator>().Play("ShieldSelect");
                    break;
                case "blink":
                    blinkButton.GetComponentInParent<Animator>().Play("BlinkSelect");
                    break;
                case "grapple":
                    grappleButton.GetComponentInParent<Animator>().Play("GrappleSelect");
                    break;
                default:
                    break;
            }
        }

        powerupEquipReadout.text = "";
        
        foreach (var VARIABLE in PD.equippedPowerups)
        {
            powerupEquipReadout.text += VARIABLE + " ";
        }
    }

    public void Update()
    {
        if (PD.equippedPowerups.Contains("None"))
        {
            foreach (var VARIABLE in allButtons)
            {
                VARIABLE.interactable = true;
            }
        }
        else
        {
            foreach (var VARIABLE in allButtons)
            {
                VARIABLE.interactable = false;
            }
        }
        
        
        
        if (powerupColorImage[0] == null || powerupColorImage[1] == null)
        {
            return;
        }

        for (int i = 0; i < 2; i++)
        {
            switch (PD.equippedPowerups[i])
            {
                case null:
                    powerupColorImage[i].color = unequippedColor;
                    break;
                case "dash":
                    powerupColorImage[i].color = dashColor;
                    break;
                case "glider":
                    powerupColorImage[i].color = glideColor;
                    break;
                case "shield":
                    powerupColorImage[i].color = shieldColor;
                    break;
                case "blink":
                    powerupColorImage[i].color = blinkColor;
                    break;
                case "grapple":
                    powerupColorImage[i].color = grappleColor;
                    break;
                default:
                    powerupColorImage[i].color = unequippedColor;
                    break;
            }
        }
        
        
        powerupEquipReadout.text = "";

        foreach (var VARIABLE in PD.equippedPowerups)
        {
            powerupEquipReadout.text += VARIABLE + " ";
        }
        
    }

    public void EquipPowerup(string powerup)
    {
        if (PD.equippedPowerups.Contains(powerup)) //If it's already equipped
        {
            PD.equippedPowerups.Remove(powerup);
            PD.equippedPowerups.Add("None");
        }
        else
        {
            if (PD.equippedPowerups.Contains("None"))
            {
                PD.equippedPowerups.Remove("None");
                if (PD.equippedPowerups.Contains("None")) //If it still contains None (I.E. both slots are empty)
                {
                    PD.equippedPowerups.Insert(0, powerup); //Put it in slot 0 (the left)
                }
                else
                {
                    PD.equippedPowerups.Add(powerup); //Put it at the end (the right)
                }

            }
        }
    }

    public void SwapPowerups()
    {
        PD.equippedPowerups.Add(PD.equippedPowerups[0]);
        PD.equippedPowerups.RemoveAt(0);
    }

    public void EjectPowerups()
    {
        foreach (var equip in PD.equippedPowerups)
        {
            switch (equip)
            {
                case "None":
                    break;
                case "dash":
                    dashButton.GetComponentInParent<Animator>().Play("Idle");
                    break;
                case "glider":
                    glideButton.GetComponentInParent<Animator>().Play("Idle");
                    break;
                case "shield":
                    shieldButton.GetComponentInParent<Animator>().Play("Idle");
                    break;
                case "blink":
                    blinkButton.GetComponentInParent<Animator>().Play("Idle");
                    break;
                case "grapple":
                    grappleButton.GetComponentInParent<Animator>().Play("Idle");
                    break;
                default:
                    break;
            }
        }

        PD.equippedPowerups = new List<string>() {"None", "None"};
    }
}

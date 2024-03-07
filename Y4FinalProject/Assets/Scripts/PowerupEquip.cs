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

    public Image[] powerupColorImage = new Image[2];
    public Color dashColor = Color.blue;
    public Color glideColor = Color.yellow;
    public Color shieldColor = Color.green;
    public Color blinkColor = Color.magenta;
    public Color grappleColor = Color.gray;
    public Color unequippedColor = Color.white;

    private void Start()
    {
        powerupEquipReadout.text = "";
        
        foreach (var VARIABLE in PD.equippedPowerups)
        {
            powerupEquipReadout.text += VARIABLE + " ";
        }
    }

    public void Update()
    {
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
                case "glide":
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

        powerupEquipReadout.text = "";

        foreach (var VARIABLE in PD.equippedPowerups)
        {
            powerupEquipReadout.text += VARIABLE + " ";
        }
       
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerupEquip : MonoBehaviour
{
    public PowerupData PD;
    public int maxPowerups = 2;
    public TextMeshProUGUI powerupEquipReadout;

    private void Start()
    {
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
        }
        else
        {
            if (PD.equippedPowerups.Count + 1 <= maxPowerups)
            {
                PD.equippedPowerups.Add(powerup);
            }
        }

        powerupEquipReadout.text = "";

        foreach (var VARIABLE in PD.equippedPowerups)
        {
            powerupEquipReadout.text += VARIABLE + " ";
        }
       
    }
}

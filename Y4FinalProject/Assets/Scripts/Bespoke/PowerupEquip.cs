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
    private int floop = 0;
    
    [Space]

    public Image[] powerupColorImage = new Image[2];
    public Color unequippedColor = Color.white;
    
    public Button dashButton;
    public Button glideButton;
    public Button bootsButton;
    public Button blinkButton;
    public Button grappleButton;
    
    private Button[] allButtons;
    
    public Animator dashAnimator;
    public Animator glideAnimator;
    public Animator bootsAnimator;
    public Animator blinkAnimator;
    public Animator grappleAnimator;
    

    private void Start()
    {
        allButtons = new[] {dashButton, glideButton, bootsButton, blinkButton, grappleButton}; //Put all the buttons in an array for easier access
        
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
                case "boots":
                    bootsButton.GetComponentInParent<Animator>().Play("ShieldSelect");
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
                    powerupColorImage[i].color = PD.dashColor;
                    break;
                case "glider":
                    powerupColorImage[i].color = PD.glideColor;
                    break;
                case "boots":
                    powerupColorImage[i].color = PD.bootsColor;
                    break;
                case "blink":
                    powerupColorImage[i].color = PD.blinkColor;
                    break;
                case "grapple":
                    powerupColorImage[i].color = PD.grappleColor;
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
            floop = 0;
            PD.equippedPowerups.Remove(powerup);
            PD.equippedPowerups.Add("None");
        }
        else
        {
            if (PD.equippedPowerups.Contains("None"))
            {
                floop = 0;
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
            else
            {
                switch (PD.equippedPowerups[floop])
                {
                    case "None":
                        break;
                    case "dash":
                        dashAnimator.Play("DashIdle");
                        dashButton.interactable = true;
                        break;
                    case "glider":
                        glideAnimator.Play("GlideIdle");
                        glideButton.interactable = true;
                        break;
                    case "boots":
                        bootsAnimator.Play("BootsIdle");
                        bootsButton.interactable = true;
                        break;
                    case "blink":
                        blinkAnimator.Play("BlinkIdle");
                        blinkButton.interactable = true;
                        break;
                    case "grapple":
                        grappleAnimator.Play("GrappleIdle");
                        grappleButton.interactable = true;
                        break;
                    default:
                        break;
                }
                PD.equippedPowerups.RemoveAt(floop);
                PD.equippedPowerups.Insert(floop, powerup);
                if (floop == 0)
                {
                    floop++;
                }
                else
                {
                    floop--;
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
                        dashAnimator.Play("DashIdle");
                        dashButton.interactable = true;
                        break;
                    case "glider":
                        glideAnimator.Play("GlideIdle");
                        glideButton.interactable = true;
                        break;
                    case "boots":
                        bootsAnimator.Play("BootsIdle");
                        bootsButton.interactable = true;
                        break;
                    case "blink":
                        blinkAnimator.Play("BlinkIdle");
                        blinkButton.interactable = true;
                        break;
                    case "grapple":
                        grappleAnimator.Play("GrappleIdle");
                        grappleButton.interactable = true;
                        break;
                    default:
                        break;
            }
        }

        PD.equippedPowerups = new List<string>() {"None", "None"};
    }
}

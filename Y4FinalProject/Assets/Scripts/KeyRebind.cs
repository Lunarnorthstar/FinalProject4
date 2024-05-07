using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyRebind : MonoBehaviour
{
    [SerializeField] private InputActionReference slideKey;
    [SerializeField] private InputActionReference powerupAKey;
    [SerializeField] private InputActionReference powerupBKey;
    [SerializeField] private InputActionReference wallrunKey;
    [SerializeField] private InputActionReference grabKey;
    
    [SerializeField] private PlayerMovement playermovement;
    [SerializeField] private TextMeshProUGUI[] bindingDisplay;
    [SerializeField] private GameObject inputWaitUI;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Start()
    {
        string rebindsS = PlayerPrefs.GetString("RebindsS", string.Empty);
        if (!string.IsNullOrEmpty(rebindsS))
        {
            slideKey.action.LoadBindingOverridesFromJson(rebindsS);
        }

        string rebindsA = PlayerPrefs.GetString("RebindsA", string.Empty);
        if (!string.IsNullOrEmpty(rebindsA))
        {
            powerupAKey.action.LoadBindingOverridesFromJson(rebindsA);
        }

        string rebindsB = PlayerPrefs.GetString("RebindsB", string.Empty);
        if (!string.IsNullOrEmpty(rebindsB))
        {
            powerupBKey.action.LoadBindingOverridesFromJson(rebindsB);
        }

        string rebindsW = PlayerPrefs.GetString("RebindsW", string.Empty);
        if (!string.IsNullOrEmpty(rebindsW))
        {
            wallrunKey.action.LoadBindingOverridesFromJson(rebindsW);
        }

        string rebindsG = PlayerPrefs.GetString("RebindsG", string.Empty);
        if (!string.IsNullOrEmpty(rebindsG))
        {
            grabKey.action.LoadBindingOverridesFromJson(rebindsG);
        }
        
        
        bindingDisplay[0].text = InputControlPath.ToHumanReadableString(slideKey.action.bindings[0].effectivePath);
        bindingDisplay[1].text = InputControlPath.ToHumanReadableString(powerupAKey.action.bindings[0].effectivePath);
        bindingDisplay[2].text = InputControlPath.ToHumanReadableString(powerupBKey.action.bindings[0].effectivePath);
        bindingDisplay[3].text = InputControlPath.ToHumanReadableString(wallrunKey.action.bindings[0].effectivePath);
        bindingDisplay[4].text = InputControlPath.ToHumanReadableString(grabKey.action.bindings[0].effectivePath);
    }


    public void StartRebinding(int controlType)
    {
        inputWaitUI.SetActive(true);

        switch (controlType)
        {
            case 0:slideKey.action.Disable();
                rebindingOperation = slideKey.action.PerformInteractiveRebinding()
                    //.WithControlsExcluding("Mouse")
                    .OnMatchWaitForAnother(0.1f)
                    .OnComplete(operation => RebindComplete(controlType))
                    .Start();
                break;
            case 1: powerupAKey.action.Disable();
                rebindingOperation = powerupAKey.action.PerformInteractiveRebinding()
                    //.WithControlsExcluding("Mouse")
                    .OnMatchWaitForAnother(0.1f)
                    .OnComplete(operation => RebindComplete(controlType))
                    .Start();
                break;
            case 2: powerupBKey.action.Disable();
                rebindingOperation = powerupBKey.action.PerformInteractiveRebinding()
                    //.WithControlsExcluding("Mouse")
                    .OnMatchWaitForAnother(0.1f)
                    .OnComplete(operation => RebindComplete(controlType))
                    .Start();
                break;
            case 3: wallrunKey.action.Disable();
                rebindingOperation = wallrunKey.action.PerformInteractiveRebinding()
                    //.WithControlsExcluding("Mouse")
                    .OnMatchWaitForAnother(0.1f)
                    .OnComplete(operation => RebindComplete(controlType))
                    .Start();
                break;
            case 4: grabKey.action.Disable();
                rebindingOperation = grabKey.action.PerformInteractiveRebinding()
                    //.WithControlsExcluding("Mouse")
                    .OnMatchWaitForAnother(0.1f)
                    .OnComplete(operation => RebindComplete(controlType))
                    .Start();
                break;
            default: break;
        }
        
        
        
        
        
        
        
    }

    private void RebindComplete(int controltype)
    {
        switch (controltype)
        {
            case 0: bindingDisplay[controltype].text = InputControlPath.ToHumanReadableString(slideKey.action.bindings[0].effectivePath);
                rebindingOperation.Dispose();
                inputWaitUI.SetActive(false);
                slideKey.action.Enable();
                break;
            case 1: bindingDisplay[controltype].text = InputControlPath.ToHumanReadableString(powerupAKey.action.bindings[0].effectivePath);
                rebindingOperation.Dispose();
                inputWaitUI.SetActive(false);
                powerupAKey.action.Enable();
                break;
            case 2: bindingDisplay[controltype].text = InputControlPath.ToHumanReadableString(powerupBKey.action.bindings[0].effectivePath);
                rebindingOperation.Dispose();
                inputWaitUI.SetActive(false);
                powerupBKey.action.Enable();
                break;
            case 3: bindingDisplay[controltype].text = InputControlPath.ToHumanReadableString(wallrunKey.action.bindings[0].effectivePath);
                rebindingOperation.Dispose();
                inputWaitUI.SetActive(false);
                wallrunKey.action.Enable();
                break;
            case 4: bindingDisplay[controltype].text = InputControlPath.ToHumanReadableString(grabKey.action.bindings[0].effectivePath);
                rebindingOperation.Dispose();
                inputWaitUI.SetActive(false);
                grabKey.action.Enable();
                break;
            default: break;
        }
        Save();
    }

    private string finalRebinds;

    public void Save()
    {
        string rebindsS = slideKey.action.SaveBindingOverridesAsJson();
        string rebindsA = powerupAKey.action.SaveBindingOverridesAsJson();
        string rebindsB = powerupBKey.action.SaveBindingOverridesAsJson();
        string rebindsW = wallrunKey.action.SaveBindingOverridesAsJson();
        string rebindsG = grabKey.action.SaveBindingOverridesAsJson();
        
        PlayerPrefs.SetString("RebindsS", rebindsS);
        PlayerPrefs.SetString("RebindsA", rebindsA);
        PlayerPrefs.SetString("RebindsB", rebindsB);
        PlayerPrefs.SetString("RebindsW", rebindsW);
        PlayerPrefs.SetString("RebindsG", rebindsG);
    
        
    }

    public void CancelRebind()
    {
        if (rebindingOperation != null)
        {
            rebindingOperation.Dispose();
            inputWaitUI.SetActive(false);
        }
        
        
    }
}

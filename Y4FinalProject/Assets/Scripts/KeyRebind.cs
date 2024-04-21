using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyRebind : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpKey;
    [SerializeField] private PlayerMovement playermovement;
    [SerializeField] private TextMeshProUGUI bindingDisplay;
    [SerializeField] private GameObject inputWaitUI;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Start()
    {
        //bindingDisplay.text = InputControlPath.ToHumanReadableString(jumpKey.action.bindings[0].effectivePath);
    }


    public void StartRebinding()
    {
        inputWaitUI.SetActive(true);
        //playermovement.controls.PlayerMovement.Disable();
        rebindingOperation = jumpKey.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete()
    {
        bindingDisplay.text = InputControlPath.ToHumanReadableString(jumpKey.action.bindings[0].effectivePath);
        rebindingOperation.Dispose();
        inputWaitUI.SetActive(false);
        //playermovement.controls.PlayerMovement.Enable();
        //Save();
    }

    public void Save()
    {
        string rebinds = playermovement.controls.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("Rebinds", rebinds);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDataManager : MonoBehaviour
{
    public SettingsHolder SH;

    public Slider mouseSensitivitySlider;
    [Space]
    public Slider joySensitivitySlider;

    private void Start()
    {
        mouseSensitivitySlider.value = SH.mouseSensitivity;
        joySensitivitySlider.value = SH.JoySensitivity;
    }


    public void updateMouseSensitivity()
    {
        SH.mouseSensitivity = mouseSensitivitySlider.value;
    }
    
    public void updateJoySensitivity()
    {
        SH.JoySensitivity = joySensitivitySlider.value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings Data", menuName = "ScriptableObjects/SettingsDataScriptableObject", order = 2)]
public class SettingsHolder : ScriptableObject
{
    public float mouseSensitivity = 1.2f;
    public float JoySensitivity = 1.2f;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Powerup Data", menuName = "ScriptableObjects/PowerupDataScriptableObject", order = 1)]
public class PowerupData : ScriptableObject
{
    public List<String> equippedPowerups;
    
    public Color dashColor = Color.blue;
    public Color glideColor = Color.yellow;
    public Color bootsColor = Color.green;
    public Color blinkColor = Color.magenta;
    public Color grappleColor = Color.gray;
}

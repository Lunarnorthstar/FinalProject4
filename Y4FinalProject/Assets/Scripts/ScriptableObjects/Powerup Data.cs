using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Powerup Data", menuName = "ScriptableObjects/PowerupDataScriptableObject", order = 1)]
public class PowerupData : ScriptableObject
{
    public List<String> equippedPowerups;
}

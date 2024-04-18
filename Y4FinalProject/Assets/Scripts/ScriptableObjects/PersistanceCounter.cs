using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Cutscene Tracker", menuName = "ScriptableObjects/PersistanceCounterScriptableObject", order = 3)]
public class PersistanceCounter : ScriptableObject
{
    // Start is called before the first frame update
    public bool repeat = false;
}

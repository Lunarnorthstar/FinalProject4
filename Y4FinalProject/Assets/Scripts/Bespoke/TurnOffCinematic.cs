using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffCinematic : MonoBehaviour
{
    public PersistanceCounter PC;

    public GameObject cinematic;
    // Start is called before the first frame update
    void Start()
    {
        if (PC.repeat)
        {
            cinematic.SetActive(false);
        }
    }

    public void resetRepeat()
    {
        PC.repeat = false;
    }
}

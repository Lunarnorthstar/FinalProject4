using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valid : MonoBehaviour
{
    public bool validPosition = true;
    public bool ready = true;
    public Material validMaterial;
    public Material invalidMaterial;

    private void Update()
    {
        if (validPosition && ready)
        {
            GetComponent<MeshRenderer>().material = validMaterial;
        }
        else
        {
            GetComponent<MeshRenderer>().material = invalidMaterial;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        validPosition = false;
    }

    private void OnTriggerExit(Collider other)
    {
        validPosition = true;
    }
}

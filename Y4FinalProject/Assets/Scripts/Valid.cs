using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valid : MonoBehaviour
{
    public bool validPosition = true;
    public Material validMaterial;
    public Material invalidMaterial;


    private void OnTriggerEnter(Collider other)
    {
        validPosition = false;
        GetComponent<MeshRenderer>().material = invalidMaterial;
    }

    private void OnTriggerExit(Collider other)
    {
        validPosition = true;
        GetComponent<MeshRenderer>().material = validMaterial;
    }
}

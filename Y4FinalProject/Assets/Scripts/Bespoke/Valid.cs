using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valid : MonoBehaviour
{
    public bool validPosition = true;
    public bool ready = true;
    public bool unblocked = true;
    public Material validMaterial;
    public Material invalidMaterial;

    private void Update()
    {
        Debug.Log("VP: " + validPosition + " R: " + ready + " UB: " + unblocked);
        
        
        
        if (validPosition && ready && unblocked)
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
        if (!other.CompareTag("StartTrigger"))
        {
            validPosition = false;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("StartTrigger"))
        {
            validPosition = true;
        }
    }
}

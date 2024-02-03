using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valid : MonoBehaviour
{
    public bool validPosition = true;


    private void OnTriggerEnter(Collider other)
    {
        validPosition = false;
    }

    private void OnTriggerExit(Collider other)
    {
        validPosition = true;
    }
}

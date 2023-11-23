using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script goes on the terrain object. It needs a trigger collider that stretches slightly above as well.

public class TerrainHandler : MonoBehaviour
{

    [Header("Effects")] 
    public bool modifiesMoveSpeed = false;
    public bool modifiesJumpHeight = false;
    public bool modifiesFriction = false;
    
    [Header("Modifiers")]
    public float moveSpeedMult = 1;
    public float jumpHeightMult = 1;
    public float frictionMult = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            
            PlayerMovement target = other.gameObject.GetComponentInParent<PlayerMovement>(); //Get the playermovement script for easy access

            if (modifiesMoveSpeed)
            {
                target.moveSpeedMult = moveSpeedMult;
            }

            if (modifiesJumpHeight)
            {
                target.jumpHeightMult = jumpHeightMult;
            }

            if (modifiesFriction)
            {
                target.fricitonMult = frictionMult;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement target = other.gameObject.GetComponentInParent<PlayerMovement>(); //Get the playermovement script for easy access

            target.moveSpeedMult = 1;
            target.jumpHeightMult = 1;
            target.fricitonMult = 1;
        }
    }
}

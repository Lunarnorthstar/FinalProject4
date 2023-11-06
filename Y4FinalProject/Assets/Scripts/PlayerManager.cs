using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool isMouseLocked;
    public float isonGroundClearance;
    public float wallRunClearance;

    void Start()
    {
        lockMouse();
    }


    public void lockMouse()
    {
        if (isMouseLocked)
        {
            isMouseLocked = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            isMouseLocked = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public bool isOnGround()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, Vector3.down, Color.red, isonGroundClearance);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, isonGroundClearance))
        {
            if (!hit.transform.gameObject.CompareTag("VaultTrigger"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool isTouchingWall(Vector3 dir)
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, dir, Color.red, wallRunClearance);

        if (Physics.Raycast(transform.position - new Vector3(0, 0.8f, 0), dir, out hit, wallRunClearance))
        {
            if (!hit.transform.gameObject.CompareTag("VaultTrigger"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0.8f, 0), dir, out hit, wallRunClearance))
        {
            if (!hit.transform.gameObject.CompareTag("VaultTrigger"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishTrigger")
        {
            gameObject.SendMessage("StopTimer");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool isMouseLocked;
    public float isonGroundClearance;

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
}

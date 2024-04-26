using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public bool isMouseLocked = true;
    public float isonGroundClearance;
    public float wallRunClearance;

    public string CurrentSurface;
    public bool isLockedAudio;

    void Start()
    {
        //lockMouse(true);
    }


    public void lockMouse(bool _lock)
    {
        if (!_lock)
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
        
        Debug.Log(isMouseLocked + ", the mouse is (hmmm?)");
    }

    public bool isOnGround()
    {
        RaycastHit hit;

        // Debug.DrawRay(transform.position, Vector3.down, Color.red, isonGroundClearance);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, isonGroundClearance))
        {
            TerrainAudioData terrainAudioData_ = hit.transform.GetComponent<TerrainAudioData>();


            if (terrainAudioData_ != null && !isLockedAudio)
            {
                CurrentSurface = terrainAudioData_.TerrainType;
            }
            else if (!isLockedAudio)
            {
                CurrentSurface = "unknown";
            }

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

    public (bool, string) isTouchingWall(Vector3 dir)
    {
        RaycastHit hit;

        //Debug.DrawRay(transform.position, dir, Color.red, wallRunClearance);

        if (Physics.Raycast(transform.position - new Vector3(0, 0.8f, 0), dir, out hit, wallRunClearance))
        {
            if (!hit.transform.gameObject.CompareTag("VaultTrigger"))
            {
                return (true, hit.transform.gameObject.tag);
            }
            else
            {
                return (false, string.Empty);
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0.8f, 0), dir, out hit, wallRunClearance))
        {
            if (!hit.transform.gameObject.CompareTag("VaultTrigger"))
            {
                return (true, hit.transform.gameObject.tag);
            }
            else
            {
                return (false, string.Empty);
            }
        }

        //These two are to keep you on the wall when you're looking towards it
        if (Physics.Raycast(transform.position + new Vector3(0.2f, 0.8f, 0), dir, out hit, wallRunClearance))
        {
            if (!hit.transform.gameObject.CompareTag("VaultTrigger"))
            {
                return (true, hit.transform.gameObject.tag);
            }
            else
            {
                return (false, string.Empty);
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(-0.2f, 0.8f, 0), dir, out hit, wallRunClearance))
        {
            if (!hit.transform.gameObject.CompareTag("VaultTrigger"))
            {
                return (true, hit.transform.gameObject.tag);
            }
            else
            {
                return (false, string.Empty);
            }
        }


        return (false, string.Empty);

    }
}

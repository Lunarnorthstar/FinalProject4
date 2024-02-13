using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GrappleHook : MonoBehaviour
{
    public Camera playerCam;

    public float grappleCooldown;
    private float cooldownTimer;
    private bool ready = true;

    public float grappleRange = 20;
    public float grappleElasticity = 4.5f;
    public float grappleDamper = 7;
    public float grappleMassScale = 4.5f;
    private Vector3 grapplePoint;
    private SpringJoint joint;
    private LineRenderer lr;

    public bool Active;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        countdown = gameObject.GetComponent<Powerups>().countdownText;
        slider = gameObject.GetComponent<Powerups>().powerupSlider;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active && !ready)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= grappleCooldown)
            {
                cooldownTimer = 0;
                ready = true;
            }
        }
        UpdateUI();
        DrawRope();
    }

    private TextMeshProUGUI countdown;
    private Slider slider;
    public void UpdateUI()
    {
        if (!ready && !Active)
        {
            countdown.text = (grappleCooldown - cooldownTimer).ToString();
            slider.value = cooldownTimer / grappleCooldown;
        }
        else
        {
            countdown.text = " ";
        }
    }

    public void Activate()
    {
        if (!Active && ready)
        {
            ActivateGrapple();
        }
        else if (Active)
        {
            DeactivateGrapple();
        }
    }

    private void ActivateGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerCam.transform.forward, out hit, grappleRange))
        {
            Debug.Log("Hit something");
            grapplePoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = math.distance(gameObject.transform.position, grapplePoint);

            
            //The range of distance the grapple will try to keep from the contact point
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = grappleElasticity;
            joint.damper = grappleDamper;
            joint.massScale = grappleMassScale;

            lr.positionCount = 2;

            Active = true;
            ready = false;
        }
        else
        {
            Debug.Log("Didn't hit anything");
        }
    }

    public void DeactivateGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
        Active = false;
    }

    private void DrawRope()
    {
        if (!joint) return;
        
        lr.SetPosition(0, gameObject.transform.position);
        lr.SetPosition(1, grapplePoint);
    }
}

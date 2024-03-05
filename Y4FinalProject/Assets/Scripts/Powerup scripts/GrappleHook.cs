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
    [HideInInspector] public bool ready = true;

    public float grappleRange = 20;
    public float reelSpeed = 1;
    public float grappleElasticity = 4.5f;
    public float grappleDamper = 7;
    public float grappleMassScale = 4.5f;
    public GameObject grappleHead;
    private Vector3 grapplePoint;
    private SpringJoint joint;
    private LineRenderer lr;


    public bool Active;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        grappleHead.transform.position = grapplePoint;
        grappleHead.transform.LookAt(gameObject.transform);


        if (!Active && !ready)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= grappleCooldown)
            {
                cooldownTimer = 0;
                ready = true;
            }
        }
        if (equipped)
        {
            UpdateUI();
            DrawRope();


            if (Active)
            {
                joint.maxDistance += gameObject.GetComponent<PlayerMovement>().controls.PlayerMovement.Reel.ReadValue<float>() * reelSpeed * 0.01f;
            }
        }

    }

    [HideInInspector] public TextMeshProUGUI countdown;
    [HideInInspector] public Slider slider;
    [HideInInspector] public TextMeshProUGUI name;
    [HideInInspector] public bool equipped = false;
    public GameObject grappleShadow;
    //public Material shadowValid;
    //public Material shadowInvalid;

    public Color shadowValid = Color.green;
    public Color shadowInvalid = Color.red;
    public void UpdateUI()
    {
        RaycastHit project;
        if (Physics.Raycast(transform.position, playerCam.transform.forward, out project, grappleRange))
        {
            grappleShadow.GetComponent<RawImage>().color = shadowValid;
        }
        else if (Physics.Raycast(transform.position, playerCam.transform.forward, out project))
        {
            grappleShadow.GetComponent<RawImage>().color = shadowInvalid;
        }

        name.text = "Grapple";



        if (!ready)
        {
            grappleShadow.GetComponent<RawImage>().color = shadowInvalid;
        }



        if (!ready && !Active)
        {
            countdown.text = CleanTimeConversion(grappleCooldown - cooldownTimer, 2);
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
            AudioManager.instance.GenerateSound(AudioReference.instance.grappleDeploy, Vector3.zero);

            Debug.Log("Hit something");
            grapplePoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;


            grappleHead.SetActive(true);

            float distanceFromPoint = math.distance(gameObject.transform.position, grapplePoint);


            //The range of distance the grapple will try to keep from the contact point
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = joint.maxDistance * 0.25f;

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
        grappleHead.SetActive(false);
        Destroy(joint);
        Active = false;
    }

    private void DrawRope()
    {
        if (!joint) return;

        lr.SetPosition(0, gameObject.transform.position);
        lr.SetPosition(1, grapplePoint);
    }

    public string CleanTimeConversion(float rawTime, int Dplaces)
    {
        int minutes = Mathf.FloorToInt(rawTime / 60);
        int seconds = Mathf.FloorToInt(rawTime - minutes * 60);
        int milliseconds = Mathf.FloorToInt((rawTime - (minutes * 60) - seconds) * (math.pow(10, Dplaces)));



        string timeReadable = string.Format("{0:00}.{1:0}", seconds, milliseconds);
        return timeReadable;
    }
}

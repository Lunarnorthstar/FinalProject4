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
    public Transform grappleOrigin;

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

    private LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("MovingObject");
        mask += LayerMask.GetMask("Ignore Raycast");
        mask += LayerMask.GetMask("Train");
        mask = ~mask;
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
    [HideInInspector] public bool equipped = false;
    public GameObject grappleShadow;
    //public Material shadowValid;
    //public Material shadowInvalid;

    public Color shadowValid = Color.green;
    public Color shadowInvalid = Color.red;
    public void UpdateUI()
    {
        RaycastHit project;
        if (Physics.Raycast(transform.position, playerCam.transform.forward, out project, grappleRange, mask)) //If it's hitting something you can grapple to...
        {
            grappleShadow.GetComponent<Image>().color = shadowValid;
        }
        else //Otherwise...
        {
            grappleShadow.GetComponent<Image>().color = shadowInvalid;
        }



        if (!ready)
        {
            grappleShadow.GetComponent<Image>().color = shadowInvalid;
        }



        if (!ready && !Active)
        {
            countdown.text = CleanTimeConversion(grappleCooldown - cooldownTimer, 2);
            slider.value = cooldownTimer / grappleCooldown;
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
        }
        else
        {
            countdown.text = " ";
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.green;
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
        if (Physics.Raycast(transform.position, playerCam.transform.forward, out hit, grappleRange, mask))
        {

            Debug.Log(hit.collider.gameObject.name);
            GetComponent<JuiceBehaviours>().playPowerupAni(true);

            AudioManager.instance.GenerateSound(AudioReference.instance.grappleDeploy, Vector3.zero);
            
            grapplePoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;
            
            grappleHead.SetActive(true);

            float distanceFromPoint = math.distance(gameObject.transform.position, grapplePoint);


            //The range of distance the grapple will try to keep from the contact point
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = 0.01f;

            joint.spring = grappleElasticity;
            joint.damper = grappleDamper;
            joint.massScale = grappleMassScale;

            lr.positionCount = 2;

            Active = true;
            ready = false;
        }
        else
        {
            GetComponent<JuiceBehaviours>().playPowerupAni(false);
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

        lr.SetPosition(0, grappleOrigin.position);
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

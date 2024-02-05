using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    [Tooltip("The impulse applied to the player while dashing")] public float dashForce = 100;
    [Tooltip("The maximum speed the player attains while dashing")] public float maxDashSpeed = 150;
    [Tooltip("The time in seconds the player dashes for")] public float dashDuration = 1;

    [Tooltip("The amount the vertical velocity is divided by to prevent rocketing off into space")] public float verticalLaunchScalar = 3;
    private bool dashing = false;
    private float durationTimer = 0;
    [Tooltip("The time in seconds the powerup cools down for")] public float dashCooldown = 4;
    private float cooldownTimer = 0;
    private bool ready = true;
    [Space] 
    public GameObject player;
    public GameObject camera;

    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready && !dashing)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= dashCooldown)
            {
                ready = true;
                cooldownTimer = 0;
            }
        }

        if (dashing)
        {
            ready = false;
            durationTimer += Time.deltaTime;
            Vector3 dashVector = camera.transform.forward;
            dashVector.Normalize();

            if (dashVector.y != 0)
            {
                dashVector.y = dashVector.y / verticalLaunchScalar;
            }
            
            
            
            rb.AddForce(dashVector * dashForce);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxDashSpeed);


            if (durationTimer >= dashDuration)
            {
                dashing = false;
                durationTimer = 0;
            }
        }
        
        UpdateUI();
    }

    public TextMeshProUGUI countdown;
    public Slider slider;
    public void UpdateUI()
    {
        if (!ready && !dashing)
        {
            countdown.text = cooldownTimer.ToString();
            slider.value = cooldownTimer / dashCooldown;
        }

        if (dashing)
        {
            countdown.text = durationTimer.ToString();
            slider.value =  1 - durationTimer / dashDuration;
        }
    }

    public void Activate()
    {
        if (ready)
        {
            dashing = true;
            
        }
    }
}

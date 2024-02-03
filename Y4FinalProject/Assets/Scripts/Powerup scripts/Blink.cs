using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    [Tooltip("The distance in units the player moves when blinking")] public float blinkDistance = 3;
    [Tooltip("The time in seconds between blinks")] public float cooldown = 3;
    private float cooldownTimer = 0;
    private bool coolingDown = false;
    public GameObject camera;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (coolingDown)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= cooldown)
            {
                cooldownTimer = 0;
                coolingDown = false;
            }
        }
        UpdateUI();
    }
    
    public TextMeshProUGUI countdown;
    public Slider slider;
    public void UpdateUI()
    {
        if (coolingDown)
        {
            countdown.text = cooldownTimer.ToString();
            slider.value = cooldownTimer / cooldown;
        }
    }

    public void Activate()
    {
        if (!coolingDown)
        {
            Vector3 blinkDirection = Vector3.Normalize(camera.transform.forward);

            player.transform.position += (blinkDirection * blinkDistance);

            coolingDown = true;
        }
    }
}

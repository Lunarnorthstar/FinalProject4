using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    public Camera playerCam;
    [Tooltip("The distance in units the player moves when blinking")] public float blinkDistance = 3;
    [Tooltip("The time in seconds between blinks")] public float cooldown = 3;
    public ParticleSystem particles;

    private float cooldownTimer = 0;
    private bool coolingDown = false;
    public GameObject camera;
    public GameObject player;
    [HideInInspector] public bool ready = false;

    // Update is called once per frame
    void Update()
    {
        ready = !coolingDown;


        if (coolingDown)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= cooldown)
            {
                cooldownTimer = 0;
                coolingDown = false;
            }
        }
        if (equipped)
        {
            UpdateUI();
        }
    }

    [HideInInspector] public TextMeshProUGUI countdown;
    [HideInInspector] public Slider slider;
    [HideInInspector] public bool equipped = false;
    public void UpdateUI()
    {
        countdown.text = " ";

        if (coolingDown)
        {
            countdown.text = CleanTimeConversion(cooldown - cooldownTimer, 2);
            slider.value = cooldownTimer / cooldown;
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
        }
        else
        {
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.green;
        }
    }

    public void Activate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerCam.transform.forward, out hit, blinkDistance))
        {
            if (hit.collider.CompareTag("Blinkblock"))
            {
                return;
            }
        }
        
        
        if (!coolingDown)
        {
            Vector3 blinkDirection = Vector3.Normalize(camera.transform.forward);

            player.transform.position += (blinkDirection * blinkDistance);
            
            particles.Play();

            coolingDown = true;

            AudioManager.instance.GenerateSound(AudioReference.instance.blinkDeploy, Vector3.zero);
        }
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

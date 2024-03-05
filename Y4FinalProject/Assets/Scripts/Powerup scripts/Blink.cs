using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
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
    [HideInInspector] public TextMeshProUGUI name;
    [HideInInspector] public bool equipped = false;
    public void UpdateUI()
    {
        name.text = "Blink";


        countdown.text = " ";

        if (coolingDown)
        {
            countdown.text = CleanTimeConversion(cooldown - cooldownTimer, 2);
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

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    private float levelTime = 0;
    public GameObject timerDisplay;
    public bool timerActive = true;
    public int significantDecimals = 2;

    [Header("Debug")]
    [SerializeField] private bool timerSpeedMultx60 = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        timerActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            if (timerSpeedMultx60)
            {
                levelTime += Time.deltaTime * 60;
            }
            else
            {
                levelTime += Time.deltaTime;
            }
        }
        
        /*string timeReadable = Mathf.Floor(levelTime / 60) + ":"; //Minutes
        if (Mathf.Floor(levelTime % 60) < 10) //If it's less than 10 seconds after a minute...
        {
            timeReadable = timeReadable + "0"; //Add a 0 to keep the length consistent
        }
        timeReadable += Mathf.Floor(levelTime % 60); //Seconds

        int timeRounding = Mathf.RoundToInt(levelTime);

        timeReadable += "." + (levelTime - timeRounding);

        timerDisplay.GetComponent<TextMeshProUGUI>().text = timeReadable;*/

        int minutes = Mathf.FloorToInt(levelTime / 60);
        int seconds = Mathf.FloorToInt(levelTime - minutes * 60);
        int milliseconds = Mathf.FloorToInt((levelTime - (minutes * 60) - seconds) * (math.pow(10, significantDecimals)));
        
        
        
        string timeReadable = string.Format("{0:0}:{1:00}.{2:0}", minutes, seconds, milliseconds);
        
        timerDisplay.GetComponent<TextMeshProUGUI>().text = timeReadable;

    }

    public void ResetTimer()
    {
        levelTime = 0;
    }

    public void StopTimer()
    {
        timerActive = false;
        timerDisplay.GetComponent<TextMeshProUGUI>().color = Color.green;
    }
}

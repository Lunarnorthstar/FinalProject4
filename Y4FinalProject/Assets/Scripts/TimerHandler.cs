using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    private float levelTime = 0;
    public GameObject timerDisplay;

    [Header("Debug")]
    [SerializeField] private bool timerSpeedMultx60 = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerSpeedMultx60)
        {
            levelTime += Time.deltaTime * 60;
        }
        else
        {
            levelTime += Time.deltaTime;
        }
        

        string timeReadable = Mathf.Floor(levelTime / 60) + ":";
        if (Mathf.Floor(levelTime % 60) < 10)
        {
            timeReadable = timeReadable + "0";
        }
        
        timeReadable += Mathf.Floor(levelTime % 60);

        timerDisplay.GetComponent<TextMeshProUGUI>().text = timeReadable;
    }

    public void ResetTimer()
    {
        levelTime = 0;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using System.IO;

public class TimerHandler : MonoBehaviour
{
    private float levelTime = 0;
    public GameObject timerDisplay;
    public bool timerActive = true;
    public int significantDecimals = 2;
    
    private string filePath;
    private const string FILE_NAME = "PersonalScores.Json"; 
    private float bestTime = 10000; 
    private float lastTime = 0;

    public GameObject leaderboard;

    [Header("Debug")]
    [SerializeField] private bool timerSpeedMultx60 = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        timerActive = true;
        
        filePath = Application.persistentDataPath;
        if (File.Exists(filePath + "/" + FILE_NAME)) //If the data file exists...
        {
            string loadedJson = File.ReadAllText(filePath + "/" + FILE_NAME); //Read all the text into a string

            bestTime = JsonUtility.FromJson<float>(loadedJson); //De-JSON it and pass it into the float
            if (bestTime < 4)
            {
                bestTime = 10000;
            }

        }
        else //Otherwise (if it doesn't exist)...
        {
            Debug.Log("File not found");
        }


        leaderboard.GetComponent<TextMeshProUGUI>().text = "TheRunningMan: " + CleanTimeConversion(bestTime - 1) + 
                                                            "\n You: " + CleanTimeConversion(bestTime) +
                                                            "\n TheWalkingMan: " + CleanTimeConversion(100000);
    }

    private void OnApplicationQuit()
    {
        Debug.Log(bestTime);
        string gameStatusJSON = JsonUtility.ToJson(bestTime); //Convert your time to JSON
        Debug.Log(gameStatusJSON);
        File.WriteAllText(filePath + "/" + FILE_NAME, gameStatusJSON);
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

        timerDisplay.GetComponent<TextMeshProUGUI>().text = CleanTimeConversion(levelTime);
    }

    public string CleanTimeConversion(float rawTime)
    {
        int minutes = Mathf.FloorToInt(rawTime / 60);
        int seconds = Mathf.FloorToInt(rawTime - minutes * 60);
        int milliseconds = Mathf.FloorToInt((rawTime - (minutes * 60) - seconds) * (math.pow(10, significantDecimals)));
        
        
        
        string timeReadable = string.Format("{0:0}:{1:00}.{2:0}", minutes, seconds, milliseconds);
        return timeReadable;
    }
    

    public void ResetTimer()
    {
        levelTime = 0;
    }

    public void StopTimer()
    {
        timerActive = false;
        timerDisplay.GetComponent<TextMeshProUGUI>().color = Color.green;
        lastTime = levelTime;
        if (lastTime < bestTime)
        {
            bestTime = lastTime;
        }
    }
}

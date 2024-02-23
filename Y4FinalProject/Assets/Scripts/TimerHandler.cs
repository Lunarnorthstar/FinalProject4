using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using System.IO;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct LeaderboardStats
{
    public float previousSave;
    public float highSave;
    public float highHundredpercentSave;
    public List<float> lastTimes;
    public List<float> lastTimesHundred;
}

public class TimerHandler : MonoBehaviour
{
    public float levelTime = 0;
    public GameObject timerDisplay;
    public bool timerActive = false;
    public int significantDecimals = 2;
    public GameObject finishPanel;
    
    string filePath;
    const string FILE_NAME = "PersonalScores.Json";
    private LeaderboardStats[] dataScore;
    
    private float bestTime = 10000;
    private float bestHundredpercentTime = 10000;
    private float lastTime = 0;
    private int levelIndex;
    public CollectibleHandler handler;



    [Header("Debug")]
    [SerializeField] private bool timerSpeedMultx60 = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.dataPath;
        dataScore = new LeaderboardStats[6];

        for (int i = 0; i < dataScore.Length; i++)
        {
            dataScore[i] = new LeaderboardStats();
            dataScore[i].highSave = 0;
            dataScore[i].previousSave = 0;
            dataScore[i].highHundredpercentSave = 0;
            dataScore[i].lastTimes = new List<float>(3);
            dataScore[i].lastTimesHundred = new List<float>(3);
        }




        levelIndex = SceneManager.GetActiveScene().buildIndex - 1;
        
        Debug.Log(filePath);

        LoadGameStatus();
        UpdateSceneFromManager();
        
        
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

        timerDisplay.GetComponent<TextMeshProUGUI>().text = CleanTimeConversion(levelTime, significantDecimals);
    }

    public string CleanTimeConversion(float rawTime, int Dplaces)
    {
        int minutes = Mathf.FloorToInt(rawTime / 60);
        int seconds = Mathf.FloorToInt(rawTime - minutes * 60);
        int milliseconds = Mathf.FloorToInt((rawTime - (minutes * 60) - seconds) * (math.pow(10, Dplaces)));
        
        
        
        string timeReadable = string.Format("{0:00}.{1:0}", seconds, milliseconds);
        return timeReadable;
    }
    

    public void ResetTimer()
    {
        levelTime = 0;
    }

    public void StopTimer()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GetComponent<PlayerMovement>().controls.PlayerMovement.Disable();
        
        
        
        timerActive = false;
        timerDisplay.GetComponent<TextMeshProUGUI>().color = Color.green;
        lastTime = levelTime;
        dataScore[levelIndex].previousSave = lastTime;
        SaveGameStatus();
        if (lastTime < bestTime || bestTime <= 0)
        {
            bestTime = lastTime;
            
            dataScore[levelIndex].highSave = bestTime;
        }
        else
        {
            if (dataScore[levelIndex].lastTimes.Count >= 3)
            {
                dataScore[levelIndex].lastTimes.Remove(1);
            }
            dataScore[levelIndex].lastTimes.Add(lastTime);
        }
        
        if (handler.hundredpercent && (lastTime < bestHundredpercentTime || bestHundredpercentTime <= 0))
        {
            bestHundredpercentTime = lastTime;
            dataScore[levelIndex].highHundredpercentSave = bestHundredpercentTime;
        }
        else
        {
            if (dataScore[levelIndex].lastTimesHundred.Count >= 3)
            {
                dataScore[levelIndex].lastTimes.Remove(1);
            }
            dataScore[levelIndex].lastTimes.Add(lastTime);
        }
        SaveGameStatus();
    }

    public void LoadGameStatus()
    {
        if (File.Exists(filePath + "/" + FILE_NAME))
        {
            string loadedJson = File.ReadAllText(filePath + "/" + FILE_NAME);
            dataScore = JsonHelper.FromJson<LeaderboardStats>(loadedJson);
            Debug.Log("File loaded successfully");
        }
        else
        {
            ResetGameStatus();
        }
        
    }
    
    public void ResetGameStatus()
    {
        dataScore = new LeaderboardStats[6];

        SaveGameStatus();
        Debug.Log("File not found...Creating");
    }
    
    public void SaveGameStatus()
    {
        string scoreJson = JsonHelper.ToJson(dataScore, true);
        
        File.WriteAllText(filePath + "/" + FILE_NAME, scoreJson);

        Debug.Log("File created and saved");
        Debug.Log(scoreJson);
    }
    
    public void UpdateSceneFromManager()
    {
        bestTime = dataScore[levelIndex].highSave;
        lastTime = dataScore[levelIndex].previousSave;
        bestHundredpercentTime = dataScore[levelIndex].highHundredpercentSave;

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishTrigger")
        {
            finishPanel.SetActive(true);
            gameObject.GetComponent<PlayerManager>().lockMouse();
            StopTimer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("StartTrigger")) timerActive = true;
        
    }
}

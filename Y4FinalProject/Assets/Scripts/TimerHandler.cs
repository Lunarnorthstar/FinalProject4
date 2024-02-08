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
using UnityEngine.SceneManagement;

[Serializable]
public struct LeaderboardStats
{
    public float previousSave;
    public float highSave;
    public float highHundredpercentSave;
}

public class TimerHandler : MonoBehaviour
{

    //public GameScore SOManager;
    
    public float levelTime = 0;
    public GameObject timerDisplay;
    public bool timerActive = false;
    public int significantDecimals = 2;
    public GameObject finishPanel;
    
    string filePath;
    const string FILE_NAME = "PersonalScores.Json"; 
    LeaderboardStats dataScore;
    
    private float bestTime = 10000;
    private float bestHundredpercentTime = 10000;
    private float lastTime = 0;
    private int levelIndex;

    //public GameObject leaderboard;
    //public GameObject hundredpercentLeaderboard;
    public CollectibleHandler handler;



    [Header("Debug")]
    [SerializeField] private bool timerSpeedMultx60 = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        /*
        bestTime = SOManager.highScore;
        lastTime = SOManager.previousScore;
        timerActive = true;
        */
        
        filePath = Application.dataPath;
        dataScore = new LeaderboardStats();
        levelIndex = SceneManager.GetActiveScene().buildIndex - 1;
        
        Debug.Log(filePath);

        LoadGameStatus();
        UpdateSceneFromManager();
        
        //UpdateUI();
        
        /*
        filePath = Application.persistentDataPath;
        if (File.Exists(filePath + "/" + FILE_NAME)) //If the data file exists...
        {
            string loadedJson = File.ReadAllText(filePath + "/" + FILE_NAME); //Read all the text into a string

            bestTime = JsonUtility.FromJson<float>(loadedJson); //De-JSON it and pass it into the float
            

        }
        else //Otherwise (if it doesn't exist)...
        {
            Debug.Log("File not found");
        }
        
        

        if (bestTime > 0)
        {
            leaderboard.GetComponent<TextMeshProUGUI>().text = "TheRunningMan: " + CleanTimeConversion(bestTime - 1) + 
                                                               "\n You: " + CleanTimeConversion(bestTime) +
                                                               "\n TheWalkingMan: " + CleanTimeConversion(100000);
        }
        else
        {
            leaderboard.GetComponent<TextMeshProUGUI>().text = "\n TheWalkingMan: " + CleanTimeConversion(100000);
        }
        */
        

    }

    /*
    private void OnApplicationQuit()
    {
        Debug.Log(bestTime);

        string gameStatusJSON = JsonUtility.ToJson(bestTime); //Convert your time to JSON
        Debug.Log(gameStatusJSON);
        File.WriteAllText(filePath + "/" + FILE_NAME, gameStatusJSON);
    }
    */


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
        dataScore.previousSave = lastTime;
        SaveGameStatus();
        //SOManager.previousScore = lastTime;
        if (lastTime < bestTime || bestTime <= 0)
        {
            bestTime = lastTime;
            
            dataScore.highSave = bestTime;
            SaveGameStatus();
            
            
            //SOManager.highScore = bestTime;
        }
        
        if (handler.hundredpercent && (lastTime < bestHundredpercentTime || bestHundredpercentTime <= 0))
        {
            bestHundredpercentTime = lastTime;
            dataScore.highHundredpercentSave = bestHundredpercentTime;
            SaveGameStatus();
        }
    }
    
    /*public void UpdateUI()
    {
        if (bestTime > 0)
        {
            leaderboard.GetComponent<TextMeshProUGUI>().text = "TheRunningMan: " + CleanTimeConversion(bestTime - 1) + 
                                                               "\n You: " + CleanTimeConversion(bestTime) +
                                                               "\n TheWalkingMan: " + CleanTimeConversion(100000);
        }
        else
        {
            leaderboard.GetComponent<TextMeshProUGUI>().text = "\n TheWalkingMan: " + CleanTimeConversion(100000);
        }

        if (bestHundredpercentTime > 0)
        {
            hundredpercentLeaderboard.GetComponent<TextMeshProUGUI>().text =
                "You: " + CleanTimeConversion(bestHundredpercentTime);
        }
    }*/
    
    public void LoadGameStatus()
    {
        if (File.Exists(filePath + "/" + FILE_NAME))
        {
            string loadedJson = File.ReadAllText(filePath + "/" + FILE_NAME);
            dataScore = JsonUtility.FromJson<LeaderboardStats>(loadedJson);
            Debug.Log("File loaded successfully");
        }
        else
        {
            ResetGameStatus();
        }
        
    }
    
    public void ResetGameStatus()
    {
        dataScore.highSave = 0;
        dataScore.highHundredpercentSave = 0;
        dataScore.previousSave = 0;

        SaveGameStatus();
        Debug.Log("File not found...Creating");
    }
    
    public void SaveGameStatus()
    {
        string scoreJson = JsonUtility.ToJson(dataScore);
        
        File.WriteAllText(filePath + "/" + FILE_NAME, scoreJson);

        Debug.Log("File created and saved");
        Debug.Log(scoreJson);
    }
    
    public void UpdateSceneFromManager()
    {
        bestTime = dataScore.highSave;
        lastTime = dataScore.previousSave;
        bestHundredpercentTime = dataScore.highHundredpercentSave;

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishTrigger")
        {
            StopTimer();
            finishPanel.SetActive(true);
            gameObject.GetComponent<PlayerManager>().lockMouse();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("StartTrigger")) timerActive = true;
        
    }
}

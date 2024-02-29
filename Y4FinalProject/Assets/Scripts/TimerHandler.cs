using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using System.IO;
using System.Linq;
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
    public string highName;
    public float highHundredpercentSave;
    public string highHundredName;
    public List<float> lastTimes;
    public List<string> lastNames;
    public List<float> lastTimesHundred;
    public List<string> lastHundredNames;

}

public class TimerHandler : MonoBehaviour
{
    public float levelTime = 0;
    public GameObject timerDisplay;
    public bool timerActive = false;
    public int significantDecimals = 2;
    public GameObject finishPanel;
    [SerializeField] private GameObject nameInput;

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
        
        nameInput = FindInActiveObjectByTag("NameInput");
        
        
        filePath = Application.dataPath;
        dataScore = new LeaderboardStats[6];

        for (int i = 0; i < dataScore.Length; i++)
        {
            dataScore[i] = new LeaderboardStats();
            dataScore[i].highSave = 0;
            dataScore[i].highName = "";
            dataScore[i].previousSave = 0;
            dataScore[i].highHundredpercentSave = 0;
            dataScore[i].highHundredName = "";
            dataScore[i].lastTimes = new List<float>(3);
            dataScore[i].lastNames = new List<string>(3);
            dataScore[i].lastTimesHundred = new List<float>(3);
            dataScore[i].lastHundredNames = new List<string>(3);
        }
        levelIndex = SceneManager.GetActiveScene().buildIndex - 1;

        Debug.Log(filePath);

        LoadGameStatus();
        UpdateSceneFromManager();


    }

    GameObject FindInActiveObjectByTag(string tag)
    {

        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].CompareTag(tag))
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
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
            dataScore[levelIndex].highName = "ANON";
        }
        else
        {
            if (dataScore[levelIndex].lastTimes.Count >= 3)
            {
                dataScore[levelIndex].lastTimes.RemoveAt(0);
                dataScore[levelIndex].lastNames.RemoveAt(0);
            }
            dataScore[levelIndex].lastTimes.Add(lastTime);
            dataScore[levelIndex].lastNames.Add("ANON");
        }

        if (handler.hundredpercent && (lastTime < bestHundredpercentTime || bestHundredpercentTime <= 0))
        {
            bestHundredpercentTime = lastTime;
            dataScore[levelIndex].highHundredpercentSave = bestHundredpercentTime;
            dataScore[levelIndex].highHundredName = "ANON";
        }
        else if(handler.hundredpercent)
        {
            if (dataScore[levelIndex].lastTimesHundred.Count >= 3)
            {
                dataScore[levelIndex].lastTimesHundred.RemoveAt(0);
                dataScore[levelIndex].lastTimesHundred.RemoveAt(0);
            }
            dataScore[levelIndex].lastTimesHundred.Add(lastTime);
            dataScore[levelIndex].lastHundredNames.Add("ANON");
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

            //GameObject.FindObjectOfType<AudioManager>().endLevel();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StartTrigger")) timerActive = true;

    }

    public void onNameInput()
    {
        string name = nameInput.GetComponent<TMP_InputField>().text;
        name = name.Substring(0, math.min(name.Length, 4));
        Debug.Log(name);

        if (lastTime == bestTime || bestTime <= 0)
        {
            dataScore[levelIndex].highName = name;
        }
        else
        {
            dataScore[levelIndex].lastNames[^1] = name; //This gets the last item in the list
        }

        if (handler.hundredpercent && (lastTime == bestHundredpercentTime || bestHundredpercentTime <= 0))
        {
            dataScore[levelIndex].highHundredName = name;
        }
        else if(handler.hundredpercent)
        {
            dataScore[levelIndex].lastHundredNames[^1] = name;
        }
        SaveGameStatus();
    }
}

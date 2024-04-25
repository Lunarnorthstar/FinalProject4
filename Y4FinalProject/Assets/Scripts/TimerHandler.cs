using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Net.Mime;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private bool bandaid = false;
    public PersistanceCounter PC;
    public float levelTime = 0;
    public GameObject timerDisplay;
    public bool timerActive = false;
    public int significantDecimals = 2;
    public int lastTimesSaved = 10;
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
        if (PC.repeat)
        {
            gameObject.GetComponent<PlayerMovement>().EndCutscene();
        }
        
        
        PC.repeat = true;
        nameInput = FindInActiveObjectByTag("NameInput");
        
        
        filePath = Application.dataPath;
        dataScore = new LeaderboardStats[6];
        
        Debug.Log(filePath);

        for (int i = 0; i < dataScore.Length; i++)
        {
            dataScore[i] = new LeaderboardStats();
            dataScore[i].highSave = 0;
            dataScore[i].highName = "";
            dataScore[i].previousSave = 0;
            dataScore[i].highHundredpercentSave = 0;
            dataScore[i].highHundredName = "";
            dataScore[i].lastTimes = new List<float>(lastTimesSaved);
            dataScore[i].lastNames = new List<string>(lastTimesSaved);
            dataScore[i].lastTimesHundred = new List<float>(lastTimesSaved);
            dataScore[i].lastHundredNames = new List<string>(lastTimesSaved);
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



        string timeReadable = string.Format("{0:0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
        return timeReadable;
    }


    public void ResetTimer()
    {
        levelTime = 0;
    }

    public void StopTimer()
    {
        //ResetRepeat();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GetComponent<PlayerMovement>().controls.PlayerMovement.Disable();
        
        timerActive = false;
        timerDisplay.GetComponent<TextMeshProUGUI>().color = Color.green;
        lastTime = levelTime;
        dataScore[levelIndex].previousSave = lastTime;

        //StopTimer is being called twice
        
            if (lastTime < bestTime || bestTime <= 0) //If the time you just got is better than the best...
            {
                bestTime = lastTime;

                InsertLastTime(dataScore[levelIndex].highSave, dataScore[levelIndex].highName); //Shunt the previous best to the list.


                dataScore[levelIndex].highSave = bestTime; //Convert your new time into the new best
                dataScore[levelIndex].highName = "ANON"; //Placehold the name

            }
            else /*if (dataScore[levelIndex].lastTimes.Count != 0)//If it's not the new best time...*/
            {
                Debug.Log("Not the best time is being called");

                InsertLastTime(lastTime, "Anon"); //Shunt the time into the list (with placeholder name)

            }

            if (handler.hundredpercent && (lastTime < bestHundredpercentTime || bestHundredpercentTime <= 0))
            {

                bestHundredpercentTime = lastTime;
                InsertLastHundredTime(dataScore[levelIndex].highHundredpercentSave, dataScore[levelIndex].highHundredName); //Same as above but for hundred% times




                dataScore[levelIndex].highHundredpercentSave = bestHundredpercentTime;
                dataScore[levelIndex].highHundredName = "ANON";
            }
            else if (handler.hundredpercent)
            {
                InsertLastHundredTime(lastTime, "Anon");
            }
            SaveGameStatus();
    }

    private int findMe;
    private int findMeHundred;
    private void InsertLastTime(float time, string name)
    {
        Debug.Log(time);
        if (time == 0)
        {
            return;
        }

        int fluff = dataScore[levelIndex].lastTimes.Count;
        Debug.Log(fluff);
        
        if (dataScore[levelIndex].lastTimes.Count == 0)
        {
            dataScore[levelIndex].lastTimes.Add(time);
            dataScore[levelIndex].lastNames.Add(name);
            findMe = 0;
            return;
        }

        Debug.Log("WORST TIME IS: " + dataScore[levelIndex].lastTimes[^1]);
        if (time > dataScore[levelIndex].lastTimes[^1] && dataScore[levelIndex].lastTimes.Count < lastTimesSaved)
        {
            dataScore[levelIndex].lastNames.Add(name);
            dataScore[levelIndex].lastTimes.Add(time);
            findMe = dataScore[levelIndex].lastNames.Count - 1;
            return;
        }


        for (int i = 0; i < dataScore[levelIndex].lastTimes.Count; i++)
        {
            if (time < dataScore[levelIndex].lastTimes[i])
            {
                if (dataScore[levelIndex].lastTimes.Count >= lastTimesSaved)
                {
                    dataScore[levelIndex].lastTimes.RemoveAt(lastTimesSaved -1);
                }
                Debug.Log(name);
                dataScore[levelIndex].lastTimes.Insert(i, time);
                dataScore[levelIndex].lastNames.Insert(i, name);
                findMe = i;
                return;
            }
        }
    }

    private void InsertLastHundredTime(float time, string name)
    {
        if (time == 0)
        {
            return;
        }


        if (dataScore[levelIndex].lastTimesHundred.Count == 0)
        {
            dataScore[levelIndex].lastTimesHundred.Insert(0, time);
            dataScore[levelIndex].lastHundredNames.Insert(0, name);
            findMeHundred = 0;
            return;
        }



        if (time > dataScore[levelIndex].lastTimesHundred[^1] && dataScore[levelIndex].lastTimesHundred.Count < lastTimesSaved)
        {
            dataScore[levelIndex].lastHundredNames.Add(name);
            dataScore[levelIndex].lastTimesHundred.Add(time);
            findMe = dataScore[levelIndex].lastHundredNames.Count - 1;
            return;
        }



        int fluff = dataScore[levelIndex].lastTimesHundred.Count;
        Debug.Log(fluff);
        
        if (dataScore[levelIndex].lastTimesHundred.Count == 0)
        {
            dataScore[levelIndex].lastTimesHundred.Insert(0, time);
            dataScore[levelIndex].lastHundredNames.Insert(0, name);
            findMeHundred = 0;
            return;
        }
        
        
        for (int i = 0; i < dataScore[levelIndex].lastTimesHundred.Count; i++)
        {
            if (time < dataScore[levelIndex].lastTimesHundred[i])
            {
                if (dataScore[levelIndex].lastTimesHundred.Count >= lastTimesSaved)
                {
                    dataScore[levelIndex].lastTimesHundred.RemoveAt(lastTimesSaved - 1);
                }
                dataScore[levelIndex].lastTimesHundred.Insert(i, time);
                dataScore[levelIndex].lastHundredNames.Insert(i, name);
                findMeHundred = i;
                return;
            }
        }
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
            if (bandaid)
            {
                return;
            }

            bandaid = true; //Because Unity is dumb and calls this function twice for LITERALLY NO REASON
            
            
            Debug.Log(other.gameObject.name);
            finishPanel.SetActive(true);
            gameObject.GetComponent<PlayerManager>().lockMouse();
            gameObject.GetComponent<PlayerMovement>().controls.Disable();
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
        
        Debug.Log("PLAYER INPUT THE NAME '" + name + "'");
        Debug.Log("TEXT FIELD READS '" + nameInput.GetComponent<TMP_InputField>().text + "'");
        Debug.Log("YOUR TIME IS: " + lastTime);
        
        if ( name != nameInput.GetComponent<TMP_InputField>().text)
        {
            name = name = nameInput.GetComponent<TMP_InputField>().text;
            Debug.Log("Corrected Name");
        }

        Debug.Log("WHAT IS THE FINDME: '" + findMe + "'");

        if (lastTime == bestTime || bestTime <= 0)
        {
            dataScore[levelIndex].highName = name;
        }
        /*else if (dataScore[levelIndex].lastNames.Count == 0)
        {
            dataScore[levelIndex].lastNames.Add(name);
        }*/
        else
        {
            dataScore[levelIndex].lastNames[findMe] = name;
        }
        
        if (handler.hundredpercent && (lastTime == bestHundredpercentTime || bestHundredpercentTime <= 0))
        {
            dataScore[levelIndex].highHundredName = name;
        }
        else if (handler.hundredpercent && (dataScore[levelIndex].lastHundredNames[^1] == "Anon"))
        {
            dataScore[levelIndex].lastHundredNames[^1] = name;
        }
        else if(handler.hundredpercent)
        {
            dataScore[levelIndex].lastHundredNames[findMeHundred] = name;
        }
        SaveGameStatus();
        FindObjectOfType<SecondaryLeaderboard>().LoadGameStatus();
    }

    public void ResetRepeat()
    {
        PC.repeat = false;
    }

    private void OnApplicationQuit()
    {
        Debug.Log("aww");
        ResetRepeat();
    }
}

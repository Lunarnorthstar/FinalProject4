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

    private bool isBest = false;
    private bool isHundredBest = false;
    private bool hundredTimeTooSlow = false;
    public void StopTimer()
    {
        isBest = false;
        isHundredBest = false;
        timeTooSlow = false;
        hundredTimeTooSlow = false;//Just in case...
        //ResetRepeat();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GetComponent<PlayerMovement>().controls.PlayerMovement.Disable();

        timerActive = false;
        timerDisplay.GetComponent<TextMeshProUGUI>().color = Color.green;
        lastTime = levelTime;
        dataScore[levelIndex].previousSave = lastTime;

        if (lastTime < bestTime || bestTime <= 0) //If the time you just got is better than the best...
        {
            bestTime = lastTime; //Make it the best
            isBest = true;

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
            isHundredBest = true;
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

    private int findMe; //This variable is used to track where the name should go when submitted.
    private int findMeHundred; //For the hundered percent lists
    private void InsertLastTime(float time, string name)
    {
        Debug.Log(time);
        if (time == 0) //If time is zero...
        {
            return; //Don't do anything, because that's not a real time (the default value for the #1 spot is 0, so when that is overwritten with the first time this line prevents it from being pushed to the list).
        }

        int fluff = dataScore[levelIndex].lastTimes.Count; //Debug - count the entries in lastTimes...
        Debug.Log(fluff); //And push them to the list.

        if (dataScore[levelIndex].lastTimes.Count == 0) //If there's nothing in the list of last times...
        {
            dataScore[levelIndex].lastTimes.Add(time); //Add the time...
            dataScore[levelIndex].lastNames.Add(name); //And the name. This skips all the insert stuff.
            findMe = 0; //Set the name target to the first element of the list (remember; computers count from 0)
            return; //You're done here, so don't do anything else.
        }

        Debug.Log("WORST TIME IS: " + dataScore[levelIndex].lastTimes[^1]); //Debug. Logs the worst time saved.
        if (time > dataScore[levelIndex].lastTimes[^1] && dataScore[levelIndex].lastTimes.Count < lastTimesSaved) //If your time is the slowest and the list isn't full...
        {
            dataScore[levelIndex].lastNames.Add(name); //Add the time...
            dataScore[levelIndex].lastTimes.Add(time); //And the name. This skips all the insert stuff.
            findMe = dataScore[levelIndex].lastNames.Count - 1; //Set the name target to the last element of the list (remember that the number of elements in the list is NOT counted from 0, so shift it by one)
            return; //You're done here, so don't do anything else.
        }


        for (int i = 0; i < dataScore[levelIndex].lastTimes.Count; i++) //For each saved time in the list...
        {
            if (time < dataScore[levelIndex].lastTimes[i]) //If the time you got is less than that time...
            {
                if (dataScore[levelIndex].lastTimes.Count >= lastTimesSaved) //If the list is full...
                {
                    dataScore[levelIndex].lastTimes.RemoveAt(lastTimesSaved - 1); //Remove the last element of the list.
                }
                dataScore[levelIndex].lastTimes.Insert(i, time); //Insert the time you just got at the point where the saved time becomes slower than the scored time. This moves everything in the list over.
                dataScore[levelIndex].lastNames.Insert(i, name); //Also insert the name. Same deal as above. This keeps the lines sorted by time.
                findMe = i; //Set the name target to the place the new time has been inserted.
                return; //You're done here, so don't do anything else.
            }
        }

        timeTooSlow = true; //If your time doesnt fit on the leaderboard because the leaderboard is full and you scored the slowest time, flip a bool.

    }

    private bool timeTooSlow = false;

    private void InsertLastHundredTime(float time, string name) //All of this is a repeat of the above, but with the hundred percent lists.
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
            findMeHundred = dataScore[levelIndex].lastHundredNames.Count - 1;
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

        hundredTimeTooSlow = true;
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
            gameObject.GetComponent<PlayerManager>().lockMouse(false);
            gameObject.GetComponent<PlayerMovement>().controls.Disable();
            StopTimer();


            //AudioManager.instance.GenerateSound(AudioReference.instance.win, Vector3.zero);
            //GameObject.FindObjectOfType<AudioManager>().endLevel();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StartTrigger")) timerActive = true;

    }



    public void onNameInput()
    {
        string name = nameInput.GetComponent<TMP_InputField>().text; //Get your name from the input field

        //Debug.Log("PLAYER INPUT THE NAME '" + name + "'");
        //Debug.Log("TEXT FIELD READS '" + nameInput.GetComponent<TMP_InputField>().text + "'");
        //Debug.Log("YOUR TIME IS: " + lastTime);

        if (name != nameInput.GetComponent<TMP_InputField>().text)
        {
            name = name = nameInput.GetComponent<TMP_InputField>().text;
            Debug.Log("Corrected Name");
        } //Don't ask.

        //Debug.Log("WHAT IS THE FINDME: '" + findMe + "'");
        
        
        if (!timeTooSlow) //If your time is too slow to have fit on the leaderboard...
        {
            if (isBest)
            {
                dataScore[levelIndex].highName = name; //If you scored a best time, put the name in the best time spot.
            }
            else
            {
                dataScore[levelIndex].lastNames[findMe] = name; //Otherwise, put it where the target has been set.
            }
        }

        if (handler.hundredpercent && !hundredTimeTooSlow)
        {
            if (isHundredBest) //If you scored a 100% time and it was the best...
            {
                dataScore[levelIndex].highHundredName = name; //Put the name in the best hundred percent time spot.
            }
            else//Otherwise, if you scored a hundred percent time...
            {
                dataScore[levelIndex].lastHundredNames[findMeHundred] = name; //Put the name in the right spot.
            }
        }
        
        
        SaveGameStatus(); //Push data to file.
        FindObjectOfType<SecondaryLeaderboard>().LoadGameStatus(); //Pull data from file to show the player.
    }

    public void ResetRepeat()
    {
        PC.repeat = false;
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Aww, goodbye");
        ResetRepeat();
    }
}

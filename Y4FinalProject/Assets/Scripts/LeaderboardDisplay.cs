using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardDisplay : MonoBehaviour
{
    string filePath;
    const string FILE_NAME = "PersonalScores.Json";

    private LeaderboardStats[] dataScore;
    private int selector = 0;
    private LevelSelectBehavior L;

    public TextMeshProUGUI leaderboard;
    public TextMeshProUGUI hundredpercentLeaderboard;
    public TextMeshProUGUI leaderboardTitle;
    public TextMeshProUGUI hundredLeaderboardTitle;

    // Start is called before the first frame update
    void Start()
    {
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



        L = GetComponent<LevelSelectBehavior>();
        filePath = Application.dataPath;
        Debug.Log(filePath);
        LoadGameStatus();
    }

    // Update is called once per frame
    void Update()
    {
        leaderboardTitle.text = "Best Times (Level" + selector + ")";
        hundredLeaderboardTitle.text = "Best 100% Times (Level" + selector + ")";

        if (selector == 0)
        {
            leaderboardTitle.text = "Best Times (Tutorial)";
            hundredLeaderboardTitle.text = "Best 100% Times (Tutorial)";
        }

        leaderboard.text = dataScore[selector].highName + " " + CleanTimeConversion(dataScore[selector].highSave, 2) + "\n Last Times";

        if (dataScore[selector].lastTimes.Count > 0)
        {
            int i = 0;
            foreach (float score in dataScore[selector].lastTimes)
            {
                if (i < dataScore[selector].lastNames.Count)
                {
                    leaderboard.text += "\n" + dataScore[selector].lastNames[i] + " " + CleanTimeConversion(score, 2);
                }
                i++;
            }
        }

        hundredpercentLeaderboard.text = dataScore[selector].highHundredName + " " + CleanTimeConversion(dataScore[selector].highHundredpercentSave, 2) + "\n Last Times";

        if (dataScore[selector].lastTimesHundred.Count > 0)
        {
            int i = 0;
            foreach (float score in dataScore[selector].lastTimesHundred)
            {
                if (i < dataScore[selector].lastHundredNames.Count)
                {
                    hundredpercentLeaderboard.text += "\n" + dataScore[selector].lastHundredNames[i] + " " + CleanTimeConversion(score, 2);
                }
                i++;
            }
        }

        if (purgeActive)
        {
            wipeTime -= Time.deltaTime;
            if (Mathf.FloorToInt(wipeTime) < secondMilestone)
            {
                Debug.Log("WARNING: ALL LEADERBOARD DATA WILL BE IRREVERSIBLY DELETED IN " +
                          secondMilestone + " SECONDS");
                purgeButton.GetComponentInChildren<TextMeshProUGUI>().text = secondMilestone + " ABORT?";
                secondMilestone--;
            }

            if (wipeTime <= 0)
            {
                ResetGameStatus();
                purgeActive = false;
                wipeTime = 10;
                secondMilestone = 9;
            }
        }
    }
    
    public string CleanTimeConversion(float rawTime, int Dplaces)
    {
        int minutes = Mathf.FloorToInt(rawTime / 60);
        int seconds = Mathf.FloorToInt(rawTime - minutes * 60);
        int milliseconds = Mathf.FloorToInt((rawTime - (minutes * 60) - seconds) * (math.pow(10, Dplaces)));



        string timeReadable = string.Format("{0:0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
        return timeReadable;
    }
    
    
    public void LeaderboardNavigate(int move)
    {
        selector += move;
        
        if (selector < 0)
        {
            selector = 0;
        }

        if (selector >= L.levels.Length)
        {
            selector = L.levels.Length - 1;
        }
        
        GameObject.FindObjectOfType<AudioManager>().buttonGeneral();
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
            Debug.Log("No file found");
        }
    }

    private float wipeTime = 10;
    private int secondMilestone = 9;
    private bool purgeActive = false;
    public Button purgeButton;
    public void PurgeScores()
    {
        purgeActive = !purgeActive;

        if (purgeActive)
        {
            StartWipe();
        }
        else
        {
            AbortWipe();
        }
    }

    private void StartWipe()
    {
        Debug.Log("WARNING: ALL LEADERBOARD DATA WILL BE IRREVERSIBLY DELETED IN " + wipeTime + " SECONDS");
    }

    private void AbortWipe()
    {
        purgeActive = false;
        wipeTime = 10;
        secondMilestone = 9;
        Debug.Log("Purge aborted. Your data is safe.");
        purgeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Reset Leaderboards";
    }

    public void ResetGameStatus()
    {
        dataScore = new LeaderboardStats[6];
        for (int i = 0; i < dataScore.Length; i++)
        {
            dataScore[i].lastTimes = new List<float>(3);
            dataScore[i].lastTimesHundred = new List<float>(3);
        }

        string scoreJson = JsonHelper.ToJson(dataScore, true);
        
        File.WriteAllText(filePath + "/" + FILE_NAME, scoreJson);

        Debug.Log("File created and saved");
        Debug.Log(scoreJson);
        
        purgeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Reset Leaderboards";
        Debug.Log("Your leaderboard data has been reset.");
    }
}

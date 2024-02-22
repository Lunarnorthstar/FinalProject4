using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
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
    
    // Start is called before the first frame update
    void Start()
    {
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
        
        
        
        L = GetComponent<LevelSelectBehavior>();
        filePath = Application.dataPath;
        Debug.Log(filePath);
        LoadGameStatus();
    }

    // Update is called once per frame
    void Update()
    {
        selector = L.selection;


        leaderboard.text = dataScore[selector].highSave + "\n Last Times";

        foreach (float time in dataScore[selector].lastTimes)
        {
            if (time != null)
            {
                leaderboard.text += "\n" + time;
            }
        }

        hundredpercentLeaderboard.text = dataScore[selector].highHundredpercentSave + "\n Last Times";

        foreach (float time in dataScore[selector].lastTimesHundred)
        {
            if (time != null)
            {
                leaderboard.text += "\n" + time;
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

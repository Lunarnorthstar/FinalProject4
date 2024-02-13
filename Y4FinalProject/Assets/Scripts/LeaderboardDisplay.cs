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

    private LeaderboardStats dataScore;

    public TextMeshProUGUI leaderboard;
    public TextMeshProUGUI hundredpercentLeaderboard;
    
    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.dataPath;
        Debug.Log(filePath);
        //dataScore.highSave = new float[6];
        //dataScore.highHundredpercentSave = new float[dataScore.highSave.Length];
        LoadGameStatus();
    }

    // Update is called once per frame
    void Update()
    {
        leaderboard.text = dataScore.highSave.ToString();
        hundredpercentLeaderboard.text = dataScore.highHundredpercentSave.ToString();


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
            dataScore = JsonUtility.FromJson<LeaderboardStats>(loadedJson);
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
        dataScore = new LeaderboardStats();

        string scoreJson = JsonUtility.ToJson(dataScore);
        
        File.WriteAllText(filePath + "/" + FILE_NAME, scoreJson);

        Debug.Log("File created and saved");
        Debug.Log(scoreJson);
        
        purgeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Reset Leaderboards";
        Debug.Log("Your leaderboard data has been reset.");
    }
}

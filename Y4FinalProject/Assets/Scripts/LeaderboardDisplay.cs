using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

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
}

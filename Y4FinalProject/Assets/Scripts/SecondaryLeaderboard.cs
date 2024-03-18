using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondaryLeaderboard : MonoBehaviour
{
    string filePath;
    const string FILE_NAME = "PersonalScores.Json";

    private LeaderboardStats[] dataScore;
    private int selector = 0;

    public TextMeshProUGUI leaderboard;
    public TextMeshProUGUI hundredpercentLeaderboard;
    // Start is called before the first frame update
    void Awake()
    {
        selector = SceneManager.GetActiveScene().buildIndex - 1;
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
        
        filePath = Application.dataPath;
        Debug.Log(filePath);
        LoadGameStatus();
    }

    // Update is called once per frame
    void Update()
    {
        leaderboard.text =  dataScore[selector].highName + " " + dataScore[selector].highSave + "\n Last Times";

        if (dataScore[selector].lastTimes.Count > 0)
        {
            int i = 0;
            foreach (float score in dataScore[selector].lastTimes)
            {
                if (i < dataScore[selector].lastNames.Count)
                {
                    leaderboard.text += "\n" + dataScore[selector].lastNames[i] + " " + score;
                }
                i++;
            }
        }

        hundredpercentLeaderboard.text = "Area 100% Record \n" + dataScore[selector].highHundredName + " " + dataScore[selector].highHundredpercentSave + "\n Last Times";

        if (dataScore[selector].lastTimesHundred.Count > 0)
        {
            int i = 0;
            foreach (float score in dataScore[selector].lastTimesHundred)
            {
                if (i < dataScore[selector].lastHundredNames.Count)
                {
                    hundredpercentLeaderboard.text += "\n" + dataScore[selector].lastHundredNames[i] + " " + score;
                }
                i++;
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
}

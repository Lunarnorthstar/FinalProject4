using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MasterDebug : MonoBehaviour
{
    private TextMeshProUGUI debugUI;
    
    [Header("CheckMaxHeight")] 
    public bool RunHeightCheck = true;
    private float startY = 0;
    public float maxY = 0;
    [Header("Move Speed")]
    public float speed = 0;
    [Header("Reset Leaderboards")]
    public bool resetLeaderboardTimeOnStart;
    [Header("Timescale checker")]
    public bool goDangit = false;
    [Header("Cutscene repeat tracker")]
    public bool checkRepeat = false;
    public PersistanceCounter PC;
    
    
    // Start is called before the first frame update
    void Start()
    {
        debugUI = GameObject.FindWithTag("Debug").GetComponent<TextMeshProUGUI>();
        if (resetLeaderboardTimeOnStart)
        {
            gameObject.SendMessage("ResetGameStatus");
        }
        startY = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        debugUI.text = String.Empty;
        speed = Mathf.Abs(GetComponent<PlayerMovement>().HorizontalVelocityf);
        if (goDangit) debugUI.text += "Timescale is " + Time.timeScale;
        
        if (RunHeightCheck) CheckMaxHeight();

        if (checkRepeat) debugUI.text += "Stop repeat is " + PC.repeat;
    }

    void CheckMaxHeight()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            maxY = 0;
        }
        
        
        if (gameObject.transform.position.y > maxY)
        {
            maxY = gameObject.transform.position.y;
        }
    }
}

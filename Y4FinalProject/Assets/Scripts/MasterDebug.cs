using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDebug : MonoBehaviour
{
    [Header("CheckMaxHeight")] 
    public bool RunHeightCheck = true;
    private float startY = 0;
    public float maxY = 0;
    public float speed = 0;
    public bool resetLeaderboardTimeOnStart;
    public bool goDangit = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (resetLeaderboardTimeOnStart)
        {
            gameObject.SendMessage("ResetGameStatus");
        }
        startY = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        speed = Mathf.Abs(GetComponent<PlayerMovement>().HorizontalVelocityf);
        if(goDangit) Debug.Log(Time.timeScale);
        
        if (RunHeightCheck)
        {
            CheckMaxHeight();
        }
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

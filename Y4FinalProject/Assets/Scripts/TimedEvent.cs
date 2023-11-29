using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TimedEvent : MonoBehaviour
{
    private TimerHandler timer;

    [Tooltip("Whether the object activates once every set period (True) or acts continuously once the time is reached (False)")] 
    public bool repeatsDelay = false;

    public int stopAfter = 0;
    private int untilStop;

    [Tooltip("How long between/until activation")] public float delay;
    private bool active = false;
    public Transform[] goTo = new Transform[1];
    private int goingTo = 0;
    public float movespeed = 1;


    // Start is called before the first frame update
    void Awake()
    {
        if (stopAfter == 0)
        {
            stopAfter = goTo.Length;
        }

        untilStop = stopAfter;
        
        timer = GameObject.FindWithTag("Player").GetComponentInParent<TimerHandler>();
        Debug.Log(GameObject.FindWithTag("Player"));
    }

    // Update is called once per frame
    void Update()
    {
        if (repeatsDelay && timer.levelTime % delay <= 0.1 && repeatsDelay && timer.levelTime % delay >= 0.0)
        {
            active = true;
        }
        else if (!repeatsDelay && timer.levelTime >= delay)
        {
            active = true;
        }
        
        if (active)
        {
            RunBehavior();
        }
    }

    public void RunBehavior()
    {
        Vector3 moveIn = goTo[goingTo].position - transform.position; //Get the distance between you and the destination
        moveIn.Normalize(); //Turn it into the direction by normalizing it
        
        transform.Translate(moveIn * movespeed * Time.deltaTime); //Move in that direction.

        if (math.distance(goTo[goingTo].position, transform.position) <= 0.1) //If you're close enough...
        {
            goingTo++; //Switch to the next target.
            if (goingTo >= goTo.Length) //If there is no next target...
            {
                goingTo = 0; //Go back to the first one.
                active = !repeatsDelay; //If you need to wait again, stop being active.
            }
            untilStop--;
            if (untilStop <= 0)
            {
                active = false;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public struct waypoint
{
    public Transform destination;
    [Tooltip("Objects that are off to turn on, and objects that are on to turn off (post move)")] public GameObject[] toggleActive;
    [Tooltip("Whether the object should stop moving here or continue iterating through the waypoints")] public bool stopHere;
    public float waitForSeconds;
}


public class TimedEvent : MonoBehaviour
{
    public waypoint[] waypoints;
    
    public bool startActive = false;
    public float initialDelay = 3;
    public bool loopInfinitely = false;
    [Tooltip("How far from the stopping point this object can be before it stops. Turn up if it doesn't stop at the destination.")] public float goalFlexibility = 0.3f;
    [Space] public float moveSpeed = 1;

    private Rigidbody RB;
    private int currentWaypoint = -1;
    private float eventCountdown = 0;
    private bool moving = false;
    private TimerHandler timer;


    private void Start()
    {
        timer = GameObject.FindWithTag("Player").GetComponentInParent<TimerHandler>();
        
        
        eventCountdown = initialDelay;
        RB = GetComponent<Rigidbody>();
        if (startActive)
        {
            moving = true;
            currentWaypoint = 0;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!moving && timer.timerActive)
        {
            CheckActivate();
            eventCountdown -= Time.deltaTime; //Decrement the countdown.
        }
        else if(timer.timerActive)//If you are supposed to be moving...
        {
            DoEvent(waypoints[currentWaypoint]);
        }
        
        
    }

    private void CheckActivate()
    {
        if (eventCountdown <= 0) //If the timer has hit zero...
        {
            currentWaypoint++; //Increment the waypoint

            if (currentWaypoint >= waypoints.Length) //If you've hit the end of the array...
            {
                if (loopInfinitely) //If you're supposed to loop...
                {
                    currentWaypoint = 0; //Then go back to zero.
                }
                else //If you're not supposed to loop...
                {
                    moving = false; //Stop moving
                    return; //Don't do anything else here.
                }
            }

            moving = true; //After checking the validity of the waypoint, start moving.
        }
    }

    private void DoEvent(waypoint target)
    {
        if (math.distance(target.destination.transform.position, transform.position) < goalFlexibility)
        {
            foreach (GameObject item in target.toggleActive)
            {
                item.SetActive(!item.activeSelf);
            }
            
            
            
            //RB.velocity = Vector3.zero;
            moving = false;
            eventCountdown = target.waitForSeconds;
            return;
        }
        Vector3 moveDirection = target.destination.transform.position - transform.position;
        moveDirection.Normalize();
        
        //RB.velocity = moveDirection * moveSpeed * Time.deltaTime * 50;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}

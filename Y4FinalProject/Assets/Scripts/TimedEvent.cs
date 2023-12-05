using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public struct waypoint
{
    public Transform location;
    [Tooltip("CURRENTLY NON-FUNCTIONAL")] public Vector3 rotation;
    [Tooltip("Objects that are off to turn on, and objects that are on to turn off (post move)")] public GameObject[] toggleActive;
    [Tooltip("Whether the object should stop moving here or continue iterating through the waypoints")] public bool stopHere;
}


public class TimedEvent : MonoBehaviour
{
    private TimerHandler timer;

    [Tooltip("Whether the object activates once every set period (True) or acts continuously once the time is reached (False)")] 
    public bool repeatsDelay = false;
    [Tooltip("How long between/until activation")] public float delay;
    private bool active = false;

    public waypoint[] waypoints = new waypoint[1];

    private int goingTo = 0;
    public float moveSpeed = 1;


    // Start is called before the first frame update
    void Awake()
    {
        timer = GameObject.FindWithTag("Player").GetComponentInParent<TimerHandler>();
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
        float travelDistance = math.distance(transform.position, waypoints[goingTo].location.position);
        Vector3 moveIn = waypoints[goingTo].location.position - transform.position; //Get the distance between you and the destination
        moveIn.Normalize(); //Turn it into the direction by normalizing it
        
        
        
        
        transform.Translate(moveIn * moveSpeed * Time.deltaTime); //Move in that direction.
        //transform.Rotate(waypoints[goingTo].rotation * moveSpeed * Time.deltaTime, Space.World);
        

        if (math.distance(waypoints[goingTo].location.position, transform.position) <= 0.1) //If you're close enough...
        {
            foreach (GameObject item in waypoints[goingTo].toggleActive)
            {
                item.SetActive(!item.activeSelf);
            }
            
            if (waypoints[goingTo].stopHere) //If you're told to stop, stop.
            {
                active = false;
            }
            goingTo++; //Switch to the next target.
            if (goingTo >= waypoints.Length && active) //If there is no next target and you haven't already stopped...
            {
                goingTo = 0; //Go back to the first one.
                active = !repeatsDelay; //If you need to wait again, stop being active.
            }
            
        }
    }
}

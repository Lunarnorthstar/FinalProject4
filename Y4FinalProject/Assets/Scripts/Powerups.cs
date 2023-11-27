using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Polybrush;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    public CameraMove cam;

    public int currentPowerUp;
    public int maxPowerUps;



    [Header("Dash Settings")]
    public float maxDashTime;
    public float dashForce;

    public float accelerationSpeed;
    public AnimationCurve accelerationCurve;
    float currentDashForce;
    int currentDashIndex;


    public void ActivatePowerup()
    {
        switch (currentPowerUp)
        {
            case 1:
                currentDashIndex = 0;
                currentDashForce = 0;
                InvokeRepeating("dash", 0, 0.01f);
                break;
        }

    }

    public void changePowerUp(bool up)
    {
        if (currentPowerUp == maxPowerUps) currentPowerUp = 0;
        else
        {
            if (up) currentPowerUp++;
            else currentPowerUp--;
        }
    }

    public void dash()
    {
        if (currentDashIndex > (maxDashTime * 100))//100 because this function is called 100 times a sec
        {
            CancelInvoke("dash");
            InvokeRepeating("cancelDash", 0, 0.01f);

            return;
        }

        currentDashForce = Mathf.Lerp(currentDashForce, dashForce, accelerationSpeed);
        cam.changeFov(accelerationCurve.Evaluate(currentDashForce));

        transform.Translate(0, 0, dashForce);//transform.forward * dashForce);
        currentDashIndex++;
    }

    void cancelDash()
    {
        if (currentDashForce < 0.02f)
        {
            CancelInvoke("cancelDash");
            cam.changeFov(accelerationCurve.Evaluate(0));
            return;
        }

        currentDashForce = Mathf.Lerp(currentDashForce, 0, accelerationSpeed);
        cam.changeFov(accelerationCurve.Evaluate(currentDashForce));
    }
}

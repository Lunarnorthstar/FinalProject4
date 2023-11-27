using System.Collections;
using System.Collections.Generic;
using Polybrush;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    public int currentPowerUp;
    public int maxPowerUps;

    public void ActivatePowerup(int index)
    {

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
}

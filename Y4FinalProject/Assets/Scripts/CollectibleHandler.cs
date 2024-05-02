using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleHandler : MonoBehaviour
{
    public int collected = 0;
    public TextMeshProUGUI UIDisplay;
    private int maxInStage = 100;
    public bool hundredpercent = false;

    public void Start()
    {
        hundredpercent = false;
        maxInStage = GameObject.FindGameObjectsWithTag("Collectible").Length;
    }

    public void Update()
    {
        UIDisplay.text = String.Format("{0}/{1}", collected, maxInStage);
        bandaid -= Time.deltaTime;
    }


    private float bandaid = 0;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            if (bandaid <= 0)
            {
                bandaid = 0.05f;
                Destroy(other.gameObject);
                collected++;

                if (collected == maxInStage)
                {
                    hundredpercent = true;
                }
            }
        }
    }
}

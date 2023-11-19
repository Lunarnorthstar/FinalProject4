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

    public void Start()
    {
        maxInStage = GameObject.FindGameObjectsWithTag("Collectible").Length;
    }

    public void Update()
    {
        UIDisplay.text = String.Format("{0}/{1}", collected, maxInStage);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            collected++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioReference : MonoBehaviour
{
    [field: Header("Sound Effects")]
    [field: SerializeField] public EventReference buttonClick { get; private set; }
    [field: SerializeField] public EventReference pageTurning { get; private set; }
    [field: SerializeField] public EventReference powerupEquip { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference menuMusic { get; private set; }
    [field: SerializeField] public EventReference gameMusic { get; private set; }

    [field: Header("Powerup Sounds")]
    [field: SerializeField] public EventReference blinkDeploy { get; private set; }
    [field: SerializeField] public EventReference dashDeploy { get; private set; }
    [field: SerializeField] public EventReference shieldDeploy { get; private set; }
    [field: SerializeField] public EventReference grappleDeploy { get; private set; }
    [field: SerializeField] public EventReference glideDeploy { get; private set; }

    [field: Header("Player Sounds")]
    [field: SerializeField] public EventReference hardStep { get; private set; }
    [field: SerializeField] public EventReference robotWhir { get; private set; }
    [field: SerializeField] public EventReference grassStep { get; private set; }
    [field: SerializeField] public EventReference mulchyStep { get; private set; }
    [field: SerializeField] public EventReference woodStep { get; private set; }
    [field: SerializeField] public EventReference metalStep { get; private set; }
    [field: SerializeField] public EventReference slide { get; private set; }



    [field: SerializeField] public EventReference landSoft { get; private set; }
    [field: SerializeField] public EventReference landMedium { get; private set; }
    [field: SerializeField] public EventReference landHard { get; private set; }


    public static AudioReference instance { get; private set; }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There's more than one audio reference script in the scene - this script is found on the audio manager object");
        }
        instance = this;
    }
}



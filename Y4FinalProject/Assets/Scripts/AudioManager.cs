using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public float musicVolume = 1;
    public bool isFXMuted;
    [Header("Audio")]
    public AudioClip menuSong;
    public AudioClip gameMusic;
    public AudioClip buttonClick;
    public AudioClip[] pageTurning;
    public AudioClip spawnIn;
    public AudioClip levelEnd;

    [Space]
    public AudioClip blinkDeploy;
    public AudioClip dashDeploy;
    public AudioClip shieldDeploy;
    public AudioClip grappleDeploy;
    public AudioClip glideDeploy;

    [Space]
    [Tooltip("use 99 for menu")]
    public int currentLevel;

    //PRIVATE VARIABLES
    AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
        switch (currentLevel)
        {
            case 99:
                music.clip = menuSong;
                break;
            default:
                music.clip = gameMusic;
                //                GenerateSound(spawnIn);
                break;
        }

        music.Play();
    }

    void Update()
    {
        music.volume = musicVolume;
    }

    public void powerUpSound(string name)
    {
        switch (name)
        {
            case "dash":
                GenerateSound(dashDeploy);
                break;
            case "blink":
                GenerateSound(blinkDeploy);
                break;
            case "glider":
                GenerateSound(glideDeploy);
                break;
            case "shield":
                GenerateSound(shieldDeploy);
                break;
            case "grapple":
                GenerateSound(grappleDeploy);
                break;
            default:
                buttonGeneral();
                break;
        }
    }

    public void turnPage()
    {
        GenerateSound(pageTurning[Random.Range(0, 2)]);
    }

    public void endLevel()
    {
        GenerateSound(levelEnd);
    }

    public void buttonGeneral()
    {
        GenerateSound(buttonClick);
    }

    public void GenerateSound(AudioClip audioClip)
    {
        if (!isFXMuted)
        {
            GameObject sound = new GameObject();
            sound.transform.parent = transform;

            sound.AddComponent<AudioSource>().clip = audioClip;
            sound.GetComponent<AudioSource>().Play();

            if (audioClip == levelEnd)
            {
                sound.GetComponent<AudioSource>().volume = 0.5f;
                GetComponent<AudioSource>().Stop();
            }

            Destroy(sound, 10f);
        }
    }
}

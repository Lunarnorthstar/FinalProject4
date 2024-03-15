using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public float musicVolume = 1;
    public float sfxVolume;

    [Space]
    public Slider musicSlider;
    public Slider sfxSlider;


    [Header("Audio")]
    EventInstance musicEventInstance;
    //StudioEventEmitter musicEmiiter;

    // public AudioClip menuSong;
    // public AudioClip gameMusic;
    // public AudioClip spawnIn;
    // public AudioClip levelEnd;

    // [Space]
    // public AudioClip blinkDeploy;
    // public AudioClip dashDeploy;
    // public AudioClip shieldDeploy;
    // public AudioClip grappleDeploy;
    // public AudioClip glideDeploy;

    [Space]
    [Tooltip("use 99 for menu")]
    public int currentLevel;

    //PRIVATE VARIABLES
    AudioSource music;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There's more than one audio manager in the scene - this script is found on the audio manager object");
        }
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        //load volume settings

        if (musicSlider && sfxSlider)
        {
            if (PlayerPrefs.HasKey("musicVol") && PlayerPrefs.HasKey("sfxVol"))
            {
                applySliderValues(musicSlider, sfxSlider);
            }
        }

        if (PlayerPrefs.HasKey("musicVol"))
            musicVolume = PlayerPrefs.GetFloat("musicVol");
        else
            PlayerPrefs.SetFloat("musicVol", 0.7f);

        if (PlayerPrefs.HasKey("sfxVol"))
            sfxVolume = PlayerPrefs.GetFloat("sfxVol");
        else
            PlayerPrefs.SetFloat("sfxVol", 0.7f);


        switch (currentLevel)
        {
            case 99:
                musicEventInstance = RuntimeManager.CreateInstance(AudioReference.instance.menuMusic);
                break;
            default:
                musicEventInstance = RuntimeManager.CreateInstance(AudioReference.instance.gameMusic);
                break;
        }

        musicEventInstance.start();
        musicEventInstance.release();
    }

    void Update()
    {
        //        music.volume = musicVolume;
        musicEventInstance.setVolume(musicVolume);


        musicVolume = PlayerPrefs.GetFloat("musicVol");
        sfxVolume = PlayerPrefs.GetFloat("sfxVol");
    }

    public void updateMusicVolume(float amount)
    {
        PlayerPrefs.SetFloat("musicVol", amount);
    }
    public void updateSFXVolume(float amount)
    {
        PlayerPrefs.SetFloat("sfxVol", amount);

    }
    public void applySliderValues(Slider _musicSlider, Slider _sfxSlider)
    {
        _musicSlider.value = PlayerPrefs.GetFloat("musicVol");
        _sfxSlider.value = PlayerPrefs.GetFloat("sfxVol");
    }

    public void powerUpSound(string name)
    {
        // switch (name)
        // {
        //     case "dash":
        //         GenerateSound(dashDeploy);
        //         break;
        //     case "blink":
        //         GenerateSound(blinkDeploy);
        //         break;
        //     case "glider":
        //         GenerateSound(glideDeploy);
        //         break;
        //     case "shield":
        //         GenerateSound(shieldDeploy);
        //         break;
        //     case "grapple":
        //         GenerateSound(grappleDeploy);
        //         break;
        //     default:
        //         buttonGeneral();
        //         break;
        // }
    }

    public void turnPage()
    {
        GenerateSound(AudioReference.instance.pageTurning, Vector3.zero);
    }

    public void endLevel()
    {
        //   GenerateSound(levelEnd);
    }

    public void buttonGeneral()
    {
        GenerateSound(AudioReference.instance.buttonClick, Vector3.zero);
    }

    public void GenerateSound(EventReference sound, Vector3 worldPos)// AudioClip audioClip)
    {
        RuntimeManager.PlayOneShot(sfxVolume, sound, worldPos);

        // GameObject sound = new GameObject();
        // sound.transform.parent = transform;

        // sound.AddComponent<AudioSource>().clip = audioClip;
        // sound.GetComponent<AudioSource>().Play();

        // sound.GetComponent<AudioSource>().volume = sfxVolume;

        // if (audioClip == levelEnd)
        // {
        //     sound.GetComponent<AudioSource>().volume = sfxVolume / 2;
        //     GetComponent<AudioSource>().Stop();
        // }

        // Destroy(sound, 10f);
    }
}


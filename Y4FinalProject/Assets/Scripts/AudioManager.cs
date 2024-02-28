using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip menuSong;
    public AudioClip gameMusic;
    public AudioClip buttonClick;
    public AudioClip[] pageTurning;

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
                break;
        }

        music.Play();
    }

    public void turnPage()
    {
        GenerateSound(pageTurning[Random.Range(0, 2)]);
    }

    public void buttonGeneral()
    {
        GenerateSound(buttonClick);
    }

    public void GenerateSound(AudioClip audioClip)
    {
        GameObject sound = new GameObject();
        sound.transform.parent = transform;

        sound.AddComponent<AudioSource>().clip = audioClip;
        sound.GetComponent<AudioSource>().Play();

        Destroy(sound, 2f);
    }
}

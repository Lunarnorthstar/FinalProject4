using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectBehavior : MonoBehaviour
{
    [Tooltip("THIS MUST MATCH SCENE NAME EXACTLY")] public String[] levels;
    public Texture[] levelPictures;
    public String[] levelTitles;
    [TextArea(10, 15)]
    public String[] levelDescriptions;
    public int selection = 0;

    public RawImage levelImage;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        levelImage.texture = levelPictures[selection];
        title.text = levelTitles[selection];
        description.text = levelDescriptions[selection];
    }

    public void Navigate(int move)
    {
        selection += move;

        if (selection < 0)
        {
            selection = 0;
        }

        if (selection >= levels.Length)
        {
            selection = levels.Length - 1;
        }

        GameObject.FindObjectOfType<AudioManager>().buttonGeneral();
    }

    public void BeginLevel()
    {
        SceneManager.LoadScene(levels[selection]);
    }
}

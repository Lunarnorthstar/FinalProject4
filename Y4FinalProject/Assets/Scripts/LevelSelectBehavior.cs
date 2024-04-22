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
    public Button forwardButton;
    public Button backwardButton;

    // Update is called once per frame
    void Update()
    {
        levelImage.texture = levelPictures[selection];
        title.text = levelTitles[selection];
        description.text = levelDescriptions[selection];

        if (selection == 0)
        {
            backwardButton.gameObject.SetActive(false);
        }
        else
        {
            backwardButton.gameObject.SetActive(true);
        }

        if (selection == levels.Length - 1)
        {
            forwardButton.gameObject.SetActive(false);
        }
        else
        {
            forwardButton.gameObject.SetActive(true);
        }
        
        
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    bool isInLevelMenu;
    Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void play()
    {
        if (!isInLevelMenu)
        {
            isInLevelMenu = true;
            ani.SetBool("play", true);
        }
        else
        {
            isInLevelMenu = false;
            ani.SetBool("play", false);
        }
    }
    public void loadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
}

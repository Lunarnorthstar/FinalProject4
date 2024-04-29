using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();//Note that this does NOT work in the editor - only in the build
        Debug.Log("This button only works in builds of the game - not in the editor. Sorry :(");
    }
}

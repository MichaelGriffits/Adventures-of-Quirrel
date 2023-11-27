using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    //Creates a GameObject in the Unity IDE called current window
    public GameObject CurrentWindow;

    //Function for altering if "CurrentWindow is active"
    public void Back()
    {
        CurrentWindow.SetActive(false);

    }          
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
    
public class OptionsMenuLoader : MonoBehaviour
{
    //Creates a GAMEOBJECt that can be changed in the Unity IDE
    public GameObject OpenWindow;

    //Funtion that opens up the 'OpenWindow'
    public void LoadOptions()
    {
        OpenWindow.SetActive(true);
    }
    
}

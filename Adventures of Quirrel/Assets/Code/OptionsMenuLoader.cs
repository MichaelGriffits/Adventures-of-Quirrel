using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
    
public class OptionsMenuLoader : MonoBehaviour
{
    public GameObject OpenWindow;

    public void LoadOptions()
    {
        OpenWindow.SetActive(true);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
   public GameObject CurrentWindow;

    public void Back()
    {
        CurrentWindow.SetActive(false);

    }        

    
}

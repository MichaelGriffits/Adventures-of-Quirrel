using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    //Function that changes what scene is loaded at a given time
    public void MoveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }   
}

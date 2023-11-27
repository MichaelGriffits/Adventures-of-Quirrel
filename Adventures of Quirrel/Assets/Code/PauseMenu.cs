using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{   
    //Creates a GAMEOBJECT that can be set up in the Unity IDE
    [SerializeField] public GameObject pauseMenu;
    //Creates a GAMEOBJECT that can be set up in the Unity IDE
    [SerializeField] public GameObject endMenu;

    //Function that runs every frame
    //Checks to see if the escape key is pressed
    //If it has then it runs the 'Pause' function
    //Also checks if the score is 25
    //If it is that it runs the 'Finish' function
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        else if (ScoreScript.scoreValue == 25)
        {
            Finish();
        }
    }

    //Functin that gets called if escape key is pressed
    //Stops time and opens the 'pauseMenu'
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    //Function that gets called if score is 25
    //Stops time and opens the 'endMenu' 
    public void Finish()
    {
        endMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    //Function that runs when the resume button is pressed
    //Restarts time and closes the pause menu
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    //Function that runs when the home button is pressed
    //Restarts time and changes the scene to the main menu
    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }

    //Function that runs when the quit button is pressed
    //Closes the application
    public void Quit()
    {   
        Application.Quit();
    }

    //Function that runs when thte restart button is pressed
    //Reloads the scene and resets the score then restarts time
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ScoreScript.scoreValue = 0;
        Time.timeScale = 1f;
    }
}

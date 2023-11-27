using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{ 
    //Creates a GAMEOBJECT that can be set in the Unity IDE
    public GameObject loadingScreen;
    //Creates a SLIDER that can be set in the Unity IDE
    public Slider slider;
    //Creates a TEXT that can be set in the Unity IDE
    public Text progressText;

    //Function that changes the scene to a different desired scen
    public void LoadLevel (int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    //Coroutine that runs to loade up the new scene
    //Changes scene, and opens up the loading screen
    //While the loading screen is open the loading bar is altered by the 'progress' value
    public IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            
            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
}

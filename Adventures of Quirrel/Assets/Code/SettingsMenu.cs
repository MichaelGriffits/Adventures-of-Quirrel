using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    //Creates a AUDIOMIXER that can be set up in the Unity IDE
    public AudioMixer audioMixer;
    //Creates a TMP_DROPDOWN that can be set up in the Unity IDe
    public TMP_Dropdown resolutionDropdown;
    //Creates a list array called 'resolutions'
    Resolution[] resolutions;

    //Function that runs at the start of the code
    //Sets the array to possiblle screen resolution of the device
    //Sets the screen resolution to the hightest possible one for the device
    void Start ()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    //Function that runs when in the Unity IDE
    //Sets the resolution to whate everone the user selects
    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height,Screen.fullScreen);
    }

    //Function that runs in the Unity IDE
    //Sets the audio equal to the slider that the user can change
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume",volume);
    }
    
    //Function that runs in the Unity IDE
    //Sets the quality equal to the dropdown options that the user can change
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //Function that runs in the Unity IDE
    //Sets fulscreen if the user clicks the button
    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}

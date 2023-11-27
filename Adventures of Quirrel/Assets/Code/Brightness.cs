using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;


public class Brightness : MonoBehaviour
{
    //Creats a SLIDER in the Unity IDE
    public Slider brightnessSlider; 
    //Creates a POSTPROCESSPROFILE in the Unity IDE
    public PostProcessProfile brightness;
    //Creates a POSTPROCESSLAYER in the Unity IDE
    public PostProcessLayer layer;

    //Sets AutoExposure in Unity equal to exposure Variable
    AutoExposure exposure;
    
    //Function that runs on start of code
    //Grabs the settings of brightness from Unity
    //Then Adjusts the brightness of that setting with the function "AdjustBrightness"
    void Start()
    {
        brightness.TryGetSettings(out exposure);
        AdjustBrightness(brightnessSlider.value);
    }

    //Function that adjusts the birghtness of the screen
    //If statment to see if slider has been moved or not
    //Sets exposure(brightness) to that varible of movement
    public void  AdjustBrightness(float value)
    {
        if(value != 0)
        {
            exposure.keyValue.value = value;
        }
        else
        {
            exposure.keyValue.value = .05f;
        }
    }
}

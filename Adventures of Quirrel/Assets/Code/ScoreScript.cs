using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    //Creates a STATIC INT that can be set up in the Unity IDe
    public static int scoreValue = 0;
    //Sets score equal to TextMeshProUGUI
    TextMeshProUGUI score;
    
    //Function the runs at the start of the code
    //Sets the score equal to the Text componet of TextMeshProUGUI
    void Start()
    {
        score = GetComponent<TMPro.TextMeshProUGUI>();
    }

    //Function that runs every frame
    //Changes the text to what ever the scoreValue is 
    void Update()
    {
        score.text = scoreValue + "/25";
    }
}

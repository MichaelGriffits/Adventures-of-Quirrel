using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    //Creates a private slider to the script that can be altered in the Unity IDE
    [SerializeField] private Slider slider;

    //Function that runs whenever an enemy is hit
    //Changes the slider to what ever the current health is of the enemy
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
}

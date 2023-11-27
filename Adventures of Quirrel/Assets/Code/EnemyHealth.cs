using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //Creates a private BOOL in the script that can be accesed by the Unity IDE
    [SerializeField]
    private bool damageable = true;
    //Creates a private INT in the script that can be accesed by the Unity IDE
    [SerializeField]
    private int healthAmount = 100;
    //Creates a private FLOAT in the script that can be accesed by the Unity IDE
    [SerializeField]
    private float invulnerabilityTime = .2f;
    
    //Creates a BOOl in the Unity IDE
    public bool giveUpwardForce = true;
    //Creates a private BOOL in the script
    private bool hit;
   
    //Creates a private INT in the script
    private int currentHealth;
    //Sets the varible "playerhealth" to access the script "PlayerHealth"
    PlayerHealth playerHealth;
    //Sets the varible "HpBar" to access the script "EnemyHpBar"
    [SerializeField] EnemyHpBar HpBar;

    //Creates a private MATERIAL in the scrip that can be accessed by the Unity IDE
    [SerializeField] private Material flashMaterial;
    //Creates a private FLOAT in the script that can be accesed by the Unity IDE
    [SerializeField] private float duration;

    //Creates a private SPRITERENDERER in the script
    private SpriteRenderer spriteRenderer;
    //Creates a private MATERIAL in the script
    private Material originalMaterial;
    //Creates a private COROUTINE in the script
    private Coroutine flashRoutine;

    //Function that runs at the start of the script
    //Sets the enemy to the max amount of health when the scene loads
    //Gets the SpriteRenderer to be used
    //Gets the material that the SpriteRenderer uses, so it can switch back to it after the flash ended
    //Gets the script from the player 
    //Sets up the HpBar system for the enemy
    private void Start()
    {
        currentHealth = healthAmount;  
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;  
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        HpBar = GetComponentInChildren<EnemyHpBar>();
        HpBar.UpdateHealthBar(currentHealth, healthAmount);
    }

    //Function that gets called to damage the enemey
    //First checks to see if the player is currently in an invulnerable state; if not it runs the logic
    //Reduces currentHealthPoints by the amount value that was set by whatever script called this function
    //If currentHealthPoints is below zero, enemy is dead, and then we handle all the logic to manage the dead state 
    //Also heals the player and increases the score
    public void Damage(int amount)
    {
        if (damageable && !hit && currentHealth > 0)
        {
            hit = true;
            currentHealth -= amount;
            HpBar.UpdateHealthBar(currentHealth, healthAmount);

            Flash();   

            if (currentHealth <= 0)
            {
                playerHealth.Heal(1);
                currentHealth = 0;
                ScoreScript.scoreValue += 1;
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(TurnOffHit());
            }
        }
    }

    //Coroutine that runs to allow the enemy to receive damage again
    //Wait in the amount of invulnerabilityTime, which by default is .2 seconds
    //Turn off the hit bool so the enemy can receive damage again
    private IEnumerator TurnOffHit()
    {
        yield return new WaitForSeconds(invulnerabilityTime);
        hit = false;
    }

    //Function that runs when enemy is hit
    //If the flashRoutine is not null, then it is currently running
    //In this case, we should stop it first
    //Multiple FlashRoutines the same time would cause bugs
    //Start the Coroutine, and store the reference for it
    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    //Coutoutine that runs to make the nemy flash when hit
    //Swap to the flashMaterial
    //Pause the execution of this function for "duration" seconds
    //After the pause, swap back to the original material
    // Set the routine to null, signaling that it's finished
    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = originalMaterial;

        flashRoutine = null;
    }
}
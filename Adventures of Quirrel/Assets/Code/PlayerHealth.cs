using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    //Bool that manages if the player can receive more damage
    private bool hit;

    //The current amount after receiving damage the enemy has
    private int currentHealth;
    public int numOfHearts;

    //Sprite control of the amount of health the player has
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    //Hom much health the player starts with
    [SerializeField]
    private int healthAmount = 5;

    //Sets the respawn point for the player
    public Transform respawnPoint;

    // The SpriteRenderer that should flash.
    private SpriteRenderer spriteRenderer;
       
    // The material that was in use, when the script started.
    private Material originalMaterial;

    // The currently running coroutine.
    private Coroutine flashRoutine;

    //Material and timefor the falsh effect on hit
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration;

    //How long before the player can be hit again
    [SerializeField] private float invulnerabilityTime = .2f;

    //Function that runs on start of code
    //Sets the enemy to the max amount of health when the scene loads
    // Get the SpriteRenderer to be used
    // Get the material that the SpriteRenderer uses
    private void Start()
    {
        currentHealth = healthAmount;  
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;  
    }
    
    //Function that runs every frame
    //Controls the hearts UI in refrence to the current health of the player
    void Update()
    {
        if (currentHealth > numOfHearts)
        {
            currentHealth = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if(i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    //Function that runs when player takes damage
    //First checks to see if the player is currently in an invulnerable state, sets hit to true
    //Reduces currentHealthPoints by the amount value that was set by whatever script called this method
    //If currentHealthPoints is below zero, player is dead
    //Removes GameObject from the scene then resets score
    //Coroutine that runs to allow the enemy to receive damage again

    public void Damage(int amount)
    {
        if (!hit && currentHealth > 0)
        {
            hit = true;
            currentHealth -= amount;

            Flash();

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                ScoreScript.scoreValue = 0;
            }
            else
            {
                StartCoroutine(TurnOffHit());
            }
        }
    }

    //Function that controls healing for the player
    public void Heal(int amount)
    {
        currentHealth += amount;
    }
     
    //Coroutine that runs to allow the enemy to receive damage again
    //Wait in the amount of invulnerabilityTime
    //Turn off the hit bool so the enemy can receive damage again
    private IEnumerator TurnOffHit()
    {
        yield return new WaitForSeconds(invulnerabilityTime);
        hit = false;
    }

    //Function that controls the flash
    //If the flashRoutine is not null, then it is currently running
    //Start the Coroutine, and store the reference for it
    public void Flash()
        {
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }

            flashRoutine = StartCoroutine(FlashRoutine());
        }

    //Coroutine that does the actual flash
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

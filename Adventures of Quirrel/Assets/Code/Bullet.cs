using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Creates an INT in the Unity IDE
    public int bulletDamage;
    //Creates a FLOAT in the Unity IDE
    private float bulletSpeed = 17f;

    //Creates a private FLOAT in the script
    private float spriteSize;
    //Creates a private TRANSFORM in the script
    private Transform playerTransform;
    //Creates a private TRANSFORM in the script
    private Transform selfTransform;
    //Creates a private VECTOR3 in the script
    private Vector3 playerPosition;
    //Creates a private VECTOR3 in the script
    private Vector3 direction ;
    
    //Sets the varible "playerhealth" to access the script "PlayerHealth"
    PlayerHealth playerHealth;

    //Function that runs on the start of code
    //Gets all the components for the private varibles
    void Start() 
    {
        spriteSize = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        bulletSpeed = 17f;
        playerPosition = playerTransform.position;
        selfTransform = GetComponent<Transform>();
        direction = (playerPosition - selfTransform.position).normalized ;
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    //Function that runs every frame
    //Moves the bullet in the direction it was shot
    void Update () 
    {
        selfTransform.position += direction * bulletSpeed * Time.deltaTime;
    }
    
    //Functions that runs when the collider has a collision
    //Checks to see if the GameObject it has collided with has not got a tag "Enemy"
    //If it does it destroys
    //Then Checks to see if what it collided with was a player
    //If it was it runs the function from the PlayerHealth sript and damages the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            Destroy (gameObject);

            if (collision.gameObject.tag == "Player")
            {
                playerHealth.Damage(1);
            }

        }
    }
}
 

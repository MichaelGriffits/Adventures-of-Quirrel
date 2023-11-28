using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Public variables accessible from the Unity editor
    public int bulletDamage;
    private float bulletSpeed = 17f;

    // Private variables for internal use in the script
    private float spriteSize;
    private Transform playerTransform;
    private Transform selfTransform;
    private Vector3 playerPosition;
    private Vector3 direction;
    private PlayerHealth playerHealth;

    //Start is called before the first frame update
    //Finding and assigning the player's transform
    //Calculating half the sprite size for collision purposes
    //Getting the PlayerHealth script component
    //Calculating the direction towards the player
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        selfTransform = GetComponent<Transform>();
        spriteSize = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        playerPosition = playerTransform.position;
        direction = (playerPosition - selfTransform.position).normalized;
    }

    // Update is called every frame
    // Moving the bullet in the calculated direction
    void Update()
    {
        selfTransform.position += direction * bulletSpeed * Time.deltaTime;
    }

    // OnCollisionEnter2D is called when this collider/rigidbody has begun touching another rigidbody/collider
    // Check if collided with an enemy and ignore collision
    // Check if collided with something other than player or enemy, destroy the bullet
    // If collided with player, damage player and destroy the bullet
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }
        else if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else
        {
            playerHealth.Damage(1);
            Destroy(gameObject);
        }
    }
}
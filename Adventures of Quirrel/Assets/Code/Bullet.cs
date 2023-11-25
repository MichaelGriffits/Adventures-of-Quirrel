using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;

    private float bulletSpeed = 17f;
    private float spriteWithDelta;
    //private Rigidbody2D bulletBody;
    private Transform playerTransform;
    private Transform selfTransform;
    private Vector3 playerPosition;
    private Vector3 direction ;
    PlayerHealth playerHealth;

    // Use this for initialization
    void Start() {
        //bulletBody = GetComponent<Rigidbody2D>();
        spriteWithDelta = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        bulletSpeed = 17f;
        playerPosition = playerTransform.position;
        selfTransform = GetComponent<Transform>();
        direction = (playerPosition - selfTransform.position).normalized ;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();


    }
    // Update is called once per frame
    void Update () {
        selfTransform.position += direction * bulletSpeed * Time.deltaTime;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Checks to see if the GameObject the MeleeWeapon is colliding with has an EnemyHealth script
        if (collision.gameObject.tag != "Enemy")
        {
            Destroy (gameObject);
            //Method that checks to see what force can be applied to the player when melee attacking
            if (collision.gameObject.tag == "Player")
            {
                playerHealth.Damage(1);
            }

        }
    }
}
 

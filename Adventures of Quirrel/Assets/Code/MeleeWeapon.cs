using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    //How much damage the melee attack does
    [SerializeField] private int damageAmount = 20;
    //Reference to Character script which contains the value if the player is facing left or right
    private PlayerMovement character;
    //Reference to the Rigidbody2D on the player
    private Rigidbody2D rb;
    //Reference to the MeleeAttackManager script on the player
    private MeleeAttack MeleeAttack;
    //Reference to the direction the player needs to go in after melee weapon contacts something
    private Vector2 direction;
    //Bool that manages if the player should move after melee weapon colides
    private bool collided;
    //Determines if the melee strike is downwards to perform extra force to fight against gravity
    private bool downwardStrike;

    //Function that runs at the start of the code
    //References the Character script on the player
    //References the Rigidbody2D on the player
    //References the MeleeAttackManager script on the player
    private void Start()
    {
        character = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();
        MeleeAttack = GetComponentInParent<MeleeAttack>();
    }

    //Function that runs once per frame
    //Runs the HandleMovement function
    private void FixedUpdate()
    {
        HandleMovement();
    }

    //Function that runs on collision
    //Checks to see if the GameObject the MeleeWeapon is colliding with has an EnemyHealth script
    //If it is runs the HandleCollision function
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHealth>())
        {
            HandleCollision(collision.GetComponent<EnemyHealth>());
        }
    }

    //Function that handles the collsion 
    //Checks to see if the GameObject allows for upward force and if the strike is downward as well as grounded
    //If so sets direction to up, and changes the bools to true
    //If not then changes the bool for a normal attack
    //Checks to see if the melee attack is a standard melee attack
    //If so then checks what direction the the player is facing and alters the dircetion accordingly
    //Then deals damage to the enemy
    private void HandleCollision(EnemyHealth objHealth)
    {
        if (objHealth.giveUpwardForce && Input.GetAxis("Vertical") < 0 && !character.isGrounded)
        {
            direction = Vector2.up;
            downwardStrike = true;
            collided = true;
        }
        if (Input.GetAxis("Vertical") > 0 && !character.isGrounded)
        {
            direction = Vector2.down;
            collided = true;
        }
        if ((Input.GetAxis("Vertical") <= 0 && character.isGrounded) || Input.GetAxis("Vertical") == 0)
        {
            if (character.IsFacingRight)
            {
                direction = Vector2.left;
            }
            else
            {
                direction = Vector2.right;
            }
            collided = true;
        }
        objHealth.Damage(damageAmount);
        StartCoroutine(NoLongerColliding());
    }

    //Function that Handles the movement of down attacks
    //Checks to see if the GameObject should allow the player to move when melee attack collides
    //If the attack was in a downward direction
    //Propels the player upwards by the amount of upwardsForce in the meleeAttackManager script
    //Else propels the player backwards by the amount of horizontalForce in the meleeAttackManager script
    private void HandleMovement()
    {
        if (collided)
        {
            if (downwardStrike)
            {
                rb.AddForce(direction * MeleeAttack.upwardsForce);
            }
            else
            {
                rb.AddForce(direction * MeleeAttack.defaultForce);
            }
        }
    }

    //Coroutine that turns off all the bools that allow movement from the HandleMovement method
    //Waits in the amount of time setup by the meleeAttackManager script
    //Turns off the collided bool
    //Turns off the downwardStrike bool
    private IEnumerator NoLongerColliding()
    {
        yield return new WaitForSeconds(MeleeAttack.movementTime);
        collided = false;
        downwardStrike = false;
    }
}

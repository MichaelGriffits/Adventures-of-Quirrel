using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumpingEnemy : MonoBehaviour
{
// Movement speed of the enemy (Serialized for accessibility in the Inspector)
    [SerializeField] float speed;
    // Current movement direction of the enemy
    private float MoveDirection = 1;
    // Flag indicating the direction the enemy is facing
    private bool FacingRight = true;
    // Ground check position (Serialized for accessibility in the Inspector)
    [SerializeField] Transform groundCheckPoint;
    // Wall check position (Serialized for accessibility in the Inspector)
    [SerializeField] Transform wallCheckPoint;
    // Circle radius for ground and wall checks (Serialized for accessibility in the Inspector)
    [SerializeField] float circleRadius;
    // Flags indicating if the enemy is checking for ground and walls
    private bool checkingGround;
    private bool checkingWall;
    // X position of the player
    float playerPosition;

    // Reference to the Rigidbody component
    private Rigidbody2D rb2d;
    // Distance to the player
    private float distToPlayer;

    // Jumping height of the enemy (Serialized for accessibility in the Inspector)
    [SerializeField] float jumpingHeight;
    // Line of sight to detect the player (Serialized for accessibility in the Inspector)
    [SerializeField] Vector2 LineOfSite;
    // Flag to check if the enemy can see the player
    private bool canSeePLayer;
    // Position of the player (Serialized for accessibility in the Inspector)
    [SerializeField] Transform playerPos;
    // Ground check position (Serialized for accessibility in the Inspector)
    [SerializeField] Transform groundCheck;
    // Box size used in ground check (Serialized for accessibility in the Inspector)
    [SerializeField] Vector2 boxSize;
    // Layer mask for ground detection
    [SerializeField] LayerMask groundLayer;
    // Layer mask for player detection
    [SerializeField] LayerMask playerLayer;
    // Flag indicating if the enemy is grounded
    private bool isGrounded;
    // Animator component attached to the enemy
    private Animator enemyAnim;
    // Reference to the player GameObject
    public GameObject player;
    // Reference to the PlayerHealth script attached to the player
    PlayerHealth playerHealth;
    // Identifier for the enemy
    public string Enemy;

    //Function that runs at the start of the code
    //Get the Rigidbody2D component attached to this GameObject
    //Get the Animator component attached to this GameObject
    //Get the PlayerHealth component from the GameObject tagged as "Player"
    //Find all GameObjects with the "Enemy" tag
    //Get the Collider2D component attached to this GameObject
    //Loop through all enemy GameObjects, get all Collider2D components from each enemy GameObject
    //Ignore collision between this enemy's collider and other enemy colliders
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Collider2D thisCollider = GetComponent<Collider2D>();

        foreach (GameObject JumpingEnemy in enemies)
        {
            Collider2D[] enemyColliders = JumpingEnemy.GetComponents<Collider2D>();

            foreach (Collider2D enemyCollider in enemyColliders)
            {
                Physics2D.IgnoreCollision(thisCollider, enemyCollider, true);
            }
        }

    }

    //Function that runs once every frame
    //Sets up checks to see if the enemy is grounded, can see the player, is near walls
    //Calculate the distance from the enemy to the player
    //Ignore collision between the player and the enemy
    //Runs the animatoController function
    //If the enemy cannot see the player and is grounded, runs patrol function
    void FixedUpdate()
    {   
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
        canSeePLayer = Physics2D.OverlapBox(transform.position, LineOfSite, 0, playerLayer);
        checkingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);
        checkingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);
        float distanceFromPlayer = playerPos.position.x - transform.position.x;
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        AnimatorController();

        if (!canSeePLayer && isGrounded) 
        {
            Patrol();
        }
      
    }
     
    //Function that performs the Jump attack of the enemy
    //Gets the distance to the player
    //And if the enemy is grounded, performs a jump attack towards the player
    private void JumpAttack()
    {
        float distanceFromPlayer = playerPos.position.x - transform.position.x;

        if (isGrounded)
        {
            rb2d.AddForce(new Vector2(distanceFromPlayer * 11, jumpingHeight), ForceMode2D.Impulse);
        }
    }   

    //Function that changes which way the enemy is moving
    //Checks where the player is relative to the enemy
    //Then depending on that runs the Flip function
    void FlipTowardsPlayer()
    {
        float playerPosition = playerPos.position.x - transform.position.x;
        if(playerPosition< 0 && FacingRight)
        {
            Flip();
        }
        else if(playerPosition>0 && !FacingRight)
        {
            Flip();
        }
    }
    
    //Function that Flips the enemy
    //Inverts the direction the enemy is moving
    //And inverst the sprite of the enemy
    void Flip()
    {
        MoveDirection *= -1;
        FacingRight = !FacingRight;
        transform.Rotate(0, 180, 0);
    }

    //Function that controls the patrolling of the enemy
    //Checks whether or not the enemy is near a Wall or if there is no ground infront
    //If so Flips and patrols in the opposite direction
    void Patrol()
    {
        if(!checkingGround || checkingWall)
       {
            if(FacingRight)
            {
                Flip();
            }
            else if(!FacingRight)
            {
                Flip();
            }
       }
       rb2d.velocity = new Vector2(speed * MoveDirection, rb2d.velocity.y);
    }
    
    //Function that controls the enemy's animations
    void AnimatorController()
    {
        enemyAnim.SetBool("canSeePlayer", canSeePLayer);
        enemyAnim.SetBool("isGrounded", isGrounded);
    }

    //Function that runs on trigger
    //If the collision is with an object that has the tag 'Player'
    //If it is then runs the damage function and deal one damage to the player
    private void OnTriggerEnter2D(Collider2D Collider2D)
    {
        if (Collider2D.gameObject.tag == "Player")
        {
            playerHealth.Damage(1);
        }
    }

    //Draw Gizmos for visualizing objects in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, LineOfSite);
        Gizmos.DrawWireSphere(groundCheckPoint.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);
        Gizmos.color = Color.white;

    }
}
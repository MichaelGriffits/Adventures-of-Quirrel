using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{   
    [SerializeField] float speed;
    private float MoveDirection = 1;
    private bool FacingRight = true;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] float circleRadius;
    private bool checkingGround;
    private bool checkingWall;
    float playerPosition;

    private Rigidbody2D rb2d; 
    private float distToPlayer;
 
    [SerializeField] float jumpingHeight;
    [SerializeField] Vector2 LineOfSite;
    private bool canSeePLayer;
    [SerializeField] Transform playerPos;
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 boxSize;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask playerLayer;
    private bool isGrounded;    
    private Animator enemyAnim;
    public GameObject player;
    PlayerHealth playerHealth;
    public string Enemy;


    // Start is called before the first frame update
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
                // Ignore collision with other enemy colliders
                Physics2D.IgnoreCollision(thisCollider, enemyCollider, true);
            }
        }

    }

    // Update is called once per frame
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
     
    private void JumpAttack()
    {
        float distanceFromPlayer = playerPos.position.x - transform.position.x;

        if (isGrounded)
        {
            rb2d.AddForce(new Vector2(distanceFromPlayer * 11, jumpingHeight), ForceMode2D.Impulse);
            
        }
    }   

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
    
    void Flip()
    {
        MoveDirection *= -1;
        FacingRight = !FacingRight;
        transform.Rotate(0, 180, 0);
    }


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
    
    void AnimatorController()
    {
        enemyAnim.SetBool("canSeePlayer", canSeePLayer);
        enemyAnim.SetBool("isGrounded", isGrounded);
    }

    private void OnTriggerEnter2D(Collider2D Collider2D)
    {
        if (Collider2D.gameObject.tag == "Player")
        {
        playerHealth.Damage(1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, LineOfSite);
        Gizmos.DrawWireSphere(groundCheckPoint.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);
        Gizmos.color = Color.white;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    //Creates a FLOAT in the Unity IDE
    public float speed;
    //Creates a FLOAT in the Unity IDE
    public float LineOfSite;
    //Creates a FLOAT in the Unity IDE
    public float ShootingRange;
    //Creates a FLOAT in the Unity IDE
    public float FireRate;
    //Creates a private FLOAT in the script
    private float NextFireTime;

    //Creates a GameObject in the Unity IDE
    public GameObject pointA;
    //Creates a GameObject in the Unity IDE
    public GameObject pointB;
    //Creates a GameObject in the Unity IDE
    public GameObject bullet;
    //Creates a GameObject in the Unity IDE
    public GameObject bulletParent;

    //Creates a private Transform in the script
    private Transform playerPos; 
    //Creates a private Transform in the script
    private Transform  currentPoint;
    //Creates a private RIGIDBODY2D in the script that is accesable in the Unity IDE
    [SerializeField] private Rigidbody2D rb;   

    //Function that runs on start
    //Sets the variable "playerPos" to the transform of the GameObject with tag 'Player'
    //Sets the variabele "currentPoint" to the transform of the GameObject "pointB"
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        currentPoint = pointB.transform;
    }

    //Function that runs on every frame
    //Calculates the distance from the player
    //If the player is within range and fires a bullet at the player
    //Also runs thorugh the movement of the enemy between the two points
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(playerPos.position, transform.position);

        if (distanceFromPlayer < LineOfSite && distanceFromPlayer > ShootingRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, playerPos.position, speed * Time.deltaTime);
        }
        else if (distanceFromPlayer <= ShootingRange && NextFireTime < Time.time)
        {
            Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
            NextFireTime = Time.time + FireRate;
        }

        Vector2 point = currentPoint.position - transform.position;
        if(currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(0, speed+1);
        }
        else
        {
            rb.velocity = new Vector2(0, -speed+1);
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;
        }
        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
            rb.velocity = new Vector2(0, -speed);

        }


    }

    //Draw Gizmos for visualizing objects in the Scene view
    private void  OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, LineOfSite);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ShootingRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}

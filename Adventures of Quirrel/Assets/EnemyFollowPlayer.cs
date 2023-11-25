using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float speed;
    public float LineOfSite;
    public float ShootingRange;
    public float FireRate;
    private float NextFireTime;

    public GameObject pointA;
    public GameObject pointB;
    public GameObject bullet;
    public GameObject bulletParent;
    private Transform playerPos;
    [SerializeField] private Rigidbody2D rb;    
    private Transform  currentPoint;


    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        currentPoint = pointB.transform;
    }

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

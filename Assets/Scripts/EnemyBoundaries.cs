//////////////////
//By: Dev Dhawan
//Date: 1/14/2020
//Description: Enemy Boundaries and enemy shoot
//////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoundaries : MonoBehaviour
{
    public GameObject proj;

    public float offset;
    private Vector3 difference;
    private float rotz;
    public int temp;

    public float waitBetweenShots;
    private float shotCounter;

    private bool shoot;
    public GameObject player;
    private Quaternion originalPos;
    public Transform firePointRight;
    public Transform firePointLeft;
    public CircleCollider2D boundary;

    private Enemy flipping;
    private bool isFacingRight;


    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            shoot = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            shoot = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            shoot = false;
        }
    }
    void FireAtPlayer()
    {

        difference = player.transform.position - transform.position;
        rotz = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotz + offset);
        bool facing = isFacingRight;
        fire();
        if (isFacingRight != facing)
        {
            Flip();
        }
        transform.rotation = originalPos;
    }
    void fire()
    {
        if (rotz >= 90 && rotz >= 0)
        {
            isFacingRight = false;
            Instantiate(proj, firePointRight.position, transform.rotation);
        }
        if (rotz <= 90 && rotz >= 0)
        {
            isFacingRight = false;
            Instantiate(proj, firePointLeft.position, transform.rotation);
        }
        if (rotz <= -90 && rotz <= 0)
        {
            isFacingRight = true;
            Instantiate(proj, firePointRight.position, transform.rotation);
        }
        if (rotz >= -90 && rotz <= 0)
        {
            isFacingRight = true;
            Instantiate(proj, firePointLeft.position, transform.rotation);
        }
        Debug.Log(isFacingRight);
    }
    void Flip()
    {
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void Start()
    {
        originalPos = transform.rotation;
        shotCounter = waitBetweenShots;
        flipping = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        shotCounter -= Time.deltaTime;
        if (shoot && shotCounter < 0)
        {
            FireAtPlayer();
            shotCounter = waitBetweenShots;
        }
        if (!shoot)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0);
        }
    }
}

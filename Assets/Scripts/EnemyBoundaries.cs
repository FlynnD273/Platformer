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
        fire();
        transform.rotation = originalPos;
    }
    void fire()
    {
        if (rotz >= 90 && rotz >= 0)
        {
            Instantiate(proj, firePointRight.position, transform.rotation);
        }
        if (rotz <= 90 && rotz >= 0)
        {
            Instantiate(proj, firePointLeft.position, transform.rotation);
        }
        if (rotz <= -90 && rotz <=0)
        {
            Instantiate(proj, firePointRight.position, transform.rotation);
        }
        if (rotz >= -90 && rotz <= 0)
        {
            Instantiate(proj, firePointLeft.position, transform.rotation);
        }
    }
    void Start()
    {
        originalPos = transform.rotation;
        shotCounter = waitBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        shotCounter -= Time.deltaTime;
        if (shoot == true && shotCounter < 0)
        {
            FireAtPlayer();
            shotCounter = waitBetweenShots;
        }
        if (shoot == false)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoundaries : MonoBehaviour
{
    public GameObject proj;

    public float offset;
    private Vector3 difference;
    private float rotz;

    private bool shoot;
    private Vector3 playerPos;
    public CircleCollider2D boundary;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerPos = collision.transform.position;
            shoot = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerPos = collision.transform.position;
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
        difference = playerPos - transform.position;
        rotz = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotz + offset);
        Instantiate(proj, transform.position, transform.rotation);
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(6f);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shoot == true)
        {
            FireAtPlayer();
        }
    }
}

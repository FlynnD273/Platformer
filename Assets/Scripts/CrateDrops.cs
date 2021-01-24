using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateDrops : MonoBehaviour
{

    public GameObject proj1;
    public GameObject proj2;
    public int health = 100;

    public int maxDrops;
    private float spawnNumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Death()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        spawnNumber = Random.Range(1, maxDrops);
        for (int i = 0; i < spawnNumber; i++)
        {
            Instantiate(proj1, transform.position, transform.rotation);
        }
        spawnNumber = Random.Range(1, maxDrops);
        for (int i = 0; i < spawnNumber; i++)
        {
            Instantiate(proj2, transform.position, transform.rotation);
        }
        
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Death();
        
    }
}

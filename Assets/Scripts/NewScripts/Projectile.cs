using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Spawn Point for projectiles
    public Transform firePoint;
    //projectile
    public GameObject proj1;
    public ProjectileMotion projectile;
    //Projectile variables
    public int kunai = 20;
    public int addKunai = 20;
    private int temp;
    private bool boolKunai;

    // Start is called before the first frame update
    void Start()
    {
        boolKunai = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if there are avaialble kunai
        if (kunai <= 0)
        {
            boolKunai = false;
        }
        else
        {
            boolKunai = true;
        }
        //checks when to fire
        if (Input.GetKeyDown(KeyCode.Space) && boolKunai == true)
        {
            kunai--;
            Fire();
        }
    }

    //Spawn kunai location
    void Fire ()
    {
        //projectile logic
        Instantiate(proj1, firePoint.position, firePoint.rotation);
    }

    //increase kunai based on event
    public void IncreaseKun(int amount)
    {
        temp = kunai + amount;
        kunai = temp;
    }
}

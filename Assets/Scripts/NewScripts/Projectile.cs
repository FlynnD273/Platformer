//////////////////
//By: Dev Dhawan
//Date: 1/11/2020
//Description: Projectile Logic for 2D game.
//////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform firePoint;
    public GameObject proj1;
    public ProjectileMotion projectile;
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
        if (kunai <= 0)
        {
            boolKunai = false;
        }
        else
        {
            boolKunai = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && boolKunai == true)
        {
            kunai--;
            Fire();
        }
    }

    void Fire ()
    {
        //projectile logic
        Instantiate(proj1, firePoint.position, firePoint.rotation);
    }

    public void IncreaseKun(int amount)
    {
        temp = kunai + amount;
        kunai = temp;
    }
}

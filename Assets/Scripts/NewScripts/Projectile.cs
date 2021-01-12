﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Spawn Point for projectiles
    public Transform firePoint;
    //projectiles
    private int switchProj = 1;
    public int switchLimit = 4;
    public GameObject proj1;
    public GameObject proj2;
    public GameObject proj3;

    public ProjectileMotion projectile;
    
    //Projectile variables
    //Kunai variable
    public int kunai = 20;
    private bool boolKunai;
    //Shurikan variable
    public int shurikan = 30;
    private bool boolShurikan;
    //Energy Varaible (used for fireball)
    public int energyLim = 100;

    private int temp;

    // Start is called before the first frame update
    void Start()
    {
        boolKunai = true;
        projectile = FindObjectOfType<ProjectileMotion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switchProj++;
        }
        //projectile.GetSwitch(switchProj);

        //Checks if there are avaialble kunai
        if (kunai <= 0)
        {
            boolKunai = false;
        }
        else
        {
            boolKunai = true;
        }
        if (shurikan <= 0)
        {
            boolShurikan = false;
        }
        else
        {
            boolShurikan = true;
        }

        //checks when to fire
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Switch();
        }
    }

    //Spawn kunai location
    void Fire (GameObject proj)
    {
        //projectile logic
        Instantiate(proj, firePoint.position, firePoint.rotation);
    }
    //increase kunai based on event
    public void IncreaseKun(int amount)
    {
        temp = kunai + amount;
        kunai = temp;
    }
    public void IncreaseSha(int amount)
    {
        temp = shurikan + amount;
        shurikan = temp;
    }
    public void IncreaseEne(int amount)
    {
        temp = energyLim + amount;
        energyLim = temp;
    }

    void Switch()
    {
        if(switchProj >= switchLimit)
        {
            switchProj = 1;
        }
        if (switchProj == 1)
        {
            if(boolKunai == true)
            {
                kunai--;
                Fire(proj1);
            }
        }
        if (switchProj == 2)
        {
            if(boolShurikan == true)
            {
                shurikan--;
                Fire(proj2);
            }
        }
        if (switchProj == 3)
        {
            if(energyLim >= 1)
            {
                temp = energyLim - 20;
                energyLim = temp;
                Fire(proj3);
            }
            if(energyLim <= 0)
            {
                energyLim = 0;
            }
        }
        
    }
}

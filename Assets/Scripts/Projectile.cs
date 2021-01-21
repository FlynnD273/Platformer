﻿//////////////////
//By: Dev Dhawan and Thomas Allen
//Date: 1/15/2020
//Description: Firing Projectiles.
//////////////////
///Thomas Updated and fixed bugs and used this for the UI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Spawn Point for projectiles
    public Transform firePoint;
    //projectiles
    public int switchProj = 1;
    public int switchLimit = 5;
    public GameObject proj1;
    public GameObject proj2;
    public GameObject proj3;
    public GameObject proj4;

    public ProjectileMotion projectile;
    public NEWPlayerLogic player;
    //public NEWFollowingCamera display;
    
    //Projectile variables
    //Kunai variable
    public static int kunai = 0;
    private static bool boolKunai;
    public float cooldown = 1;
    private float cooldownSt;
    private bool startCount = false;
    //Shurikan variable
    public static int shurikan = 0;
    private static bool boolShurikan;
    public int maxThrow = 4;
    //Energy Varaible (used for fireball)
    public float energyFireBall = 20;
    public float energyMiniBall = 10;
    public float offset;
    private Vector3 difference;
    private float rotz;
    private Quaternion rotateFreeze;
    public GameObject cursur;
    private Vector3 cursurPos;
    private Vector3 cursurOg;
    private static bool boolLargeFireball = false;
    private static bool boolMiniFireball = false;

    private int temp;

    public AudioClip shurikenThrow; //sound for when player throws the shuriken
    public AudioClip kunaiThrow; //sound for when player throws the kunai
    public AudioClip fireballSound; //sound for when player throws a fireball

    //animator for throwing anim
    private Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        boolKunai = true;
        projectile = FindObjectOfType<ProjectileMotion>();
        player = FindObjectOfType<NEWPlayerLogic>();
        myAnim = GetComponent<Animator>();
        //display = FindObjectOfType<NEWFollowingCamera>();
        cursurOg = cursur.transform.position;
        //cooldown timer set
        cooldownSt = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
        //display.SwitchProj(switchProj);
        if (switchProj == 3 || switchProj == 4)
        {
            cursurPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 1);
            cursur.transform.position = cursurPos;
        }
        else
        {
            cursur.transform.position = cursurOg;
        }
        if (Input.GetMouseButtonDown(2) && PauseMenu.isPaused == false)
        {
            switchProj++;
            if (switchProj >= switchLimit)
            {
                switchProj = 1;
            }
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

        if (startCount == true)
        {
            cooldownSt -= Time.deltaTime;
            if (cooldownSt <= 0)
            {
                startCount = false;
                maxThrow = 4;
                cooldownSt = cooldown;
            }
        }

        //checks when to fire
        if (Input.GetMouseButtonDown(0) && PauseMenu.isPaused == false && startCount == false)
        {
            Switch();
        }
    }

    //Spawn kunai location
    void Fire (GameObject proj)
    {
        //projectile logic
        Instantiate(proj, firePoint.position, firePoint.rotation);
        //do the throw anim
        myAnim.SetTrigger("Throw");
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
    
    void MouseTarget()
    {
        cursur.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rotateFreeze = transform.rotation;
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        rotz = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotz + offset);
    }

    void Switch()
    {
        if (switchProj == 1)
        {
            if(boolKunai == true)
            {
                kunai--;
                cooldownSt = cooldown - 0.3f;
                startCount = true;
                Fire(proj1);
                player.GetComponent<AudioSource>().PlayOneShot(kunaiThrow);

            }
        }
        if (switchProj == 2)
        {
            if(boolShurikan == true)
            {
                shurikan--;
                maxThrow--;
                if (maxThrow <= 0)
                {
                    startCount = true;
                }
                Fire(proj2);
                player.GetComponent<AudioSource>().PlayOneShot(shurikenThrow);
            }
        }
        if (switchProj == 3)
        {
            if (boolMiniFireball)
            {
                cooldownSt = cooldown - 0.3f;
                startCount = true;
                MouseTarget();
                if (player.EnergyChange(true, energyMiniBall, switchProj) == true)
                {
                    Instantiate(proj3, firePoint.position, transform.rotation);
                    player.GetComponent<AudioSource>().PlayOneShot(fireballSound);
                }
                transform.rotation = rotateFreeze;
            }
        }
        if (switchProj == 4)
        {
            if (boolLargeFireball)
            {
                cooldownSt = cooldown * 3;
                startCount = true;
                MouseTarget();
                if (player.EnergyChange(true, energyFireBall, switchProj) == true)
                {
                    Instantiate(proj4, firePoint.position, transform.rotation);
                    player.GetComponent<AudioSource>().PlayOneShot(fireballSound);
                }
                transform.rotation = rotateFreeze;
            }
            
        }
    }
}

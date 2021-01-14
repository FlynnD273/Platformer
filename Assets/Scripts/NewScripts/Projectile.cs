using System.Collections;
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
    public NEWPlayerLogic player;
    
    //Projectile variables
    //Kunai variable
    public int kunai = 20;
    private bool boolKunai;
    //Shurikan variable
    public int shurikan = 30;
    private bool boolShurikan;
    //Energy Varaible (used for fireball)
    public float energyMin = 25;
    public float offset;
    private Vector3 difference;
    private float rotz;
    private Quaternion rotateFreeze;
    public GameObject cursur;
    private Vector3 cursurPos;
    private Vector3 cursurOg;

    private int temp;

    // Start is called before the first frame update
    void Start()
    {
        boolKunai = true;
        projectile = FindObjectOfType<ProjectileMotion>();
        player = FindObjectOfType<NEWPlayerLogic>();
        cursurOg = cursur.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (switchProj == 3)
        {
            cursurPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 1);
            cursur.transform.position = cursurPos;
        }
        else
        {
            cursur.transform.position = cursurOg;
        }
        if (Input.GetMouseButtonDown(2))
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
        if (Input.GetMouseButtonDown(0))
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
            cursur.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rotateFreeze = transform.rotation;
            difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            rotz = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotz + offset);

            if (player.EnergyChange(true, energyMin) == true)
            {
                Instantiate(proj3, firePoint.position, transform.rotation);
            }
            transform.rotation = rotateFreeze;
        }
        
    }
}

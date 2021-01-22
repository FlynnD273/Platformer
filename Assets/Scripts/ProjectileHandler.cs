//////////////////////
///Name: Thomas Allen and Dev Dhawan
///Date: 1/22/21
///Desc: Manages projectile firing and ammo
/////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    //Spawn Point for projectiles
    public Transform firePoint;

    [Header("")]
    public static int currentWeapon = 1; //int to represent currently equipped weapon
    public static int maxWeapons = 1; //limit on weapons player can access

    [Header("Weapon Prefabs")]
    public GameObject[] weapons; //array to put weapons prefabs in


    private Animator myAnim;

    [Header("Weapon Sound Effects")]
    public AudioClip shurikenSOund; //sound for when player throws the shuriken
    public AudioClip kunaiSound; //sound for when player throws the kunai
    public AudioClip fireballSound; //sound for when player throws a fireball

    private NEWPlayerLogic playerLogic;

    [Header("Cooldowns")]
    [SerializeField] float fireCooldownKunai = 1; //cooldown to fire kunai
    [SerializeField] float fireCooldownShuriken = 2; //cooldown to fire shuriken;

    [Header("Ammo")]
    public static int kunaiCount = 0; //kunai ammo
    public static int shurikenCount = 0; //shuriken ammo

    /// <summary>
    /// stuff for aiming fireball
    /// </summary>
    private Vector3 difference;
    private float rotz;
    private Quaternion rotateFreeze;
    private float offset = 0;
    public GameObject cursur;

    //are true if player has one or more ammo of respective type.
    private bool hasKunAmmo; 
    private bool hasShuAmmo;

    [SerializeField] int shurikenBurst = 3; //how many shurikens player can shoot before having to "reload"
    [Header("Timer")]
    public float timer; //timer for cooldowns

    private float energyCost = 15;
    private float bigFireballEnergyCost = 60;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>(); //get animator
        playerLogic = FindObjectOfType<NEWPlayerLogic>(); //get player logic script

    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon > maxWeapons)
        {
            currentWeapon = 1;
        }

        //update ammo bools
        if (kunaiCount > 0)
            hasKunAmmo = true;
        else
            hasKunAmmo = false;

        if (shurikenCount > 0)
            hasShuAmmo = true;
        else
            hasShuAmmo = false;

        //update timer
        timer += Time.deltaTime;

        //get mouse click input
        if(Input.GetMouseButtonDown(0))
        {
            
            //switch on teh current weapon to decide which weapon to fire
            switch (currentWeapon)
            {
                case 1: //kunai
                    if (timer >= fireCooldownKunai && hasKunAmmo) //make sure cooldown is over and that they have ammo
                    {
                        //fire
                        Fire(weapons[currentWeapon - 1]); 
                        //do the throw anim
                        myAnim.SetTrigger("Throw");
                        //reduec ammo
                        AmmoHandler(currentWeapon);
                        //player sound
                        gameObject.GetComponent<AudioSource>().PlayOneShot(kunaiSound);
                        //reset timer
                        timer = 0;
                    }
                    break;
                case 2: //suriken
                    if (shurikenBurst > 0 && hasShuAmmo) //make sure player has ammo left in the busrt and that they have ammo overall
                    {
                        //shoot it
                        Fire(weapons[currentWeapon - 1]);
                        //reduce ammo
                        AmmoHandler(currentWeapon);
                        //do the throw anim
                        myAnim.SetTrigger("Throw");
                        //reduce the burst
                        shurikenBurst--;
                        //play the sound
                        gameObject.GetComponent<AudioSource>().PlayOneShot(shurikenSOund);
                    }
                    break;

                case 3: //fireball
                    if (NEWPlayerLogic.energy >= energyCost) //make sure player has energy
                    {
                        //set the target to the mouse
                        MouseTarget();
                        //fire 
                        Fire(weapons[currentWeapon - 1]);
                        //reduce player energy
                        NEWPlayerLogic.energy -= energyCost;
                        //freeze player rotation so that they don't go wonk
                        transform.rotation = rotateFreeze;
                        //play sound
                        gameObject.GetComponent<AudioSource>().PlayOneShot(fireballSound);
                    }
                    break;
                case 4: //big fireball
                    if (NEWPlayerLogic.energy >= bigFireballEnergyCost) //make sure player has energy
                    {
                        //set the target to the mouse
                        MouseTarget();
                        //fire 
                        Fire(weapons[currentWeapon - 1]);
                        //reduce player energy
                        NEWPlayerLogic.energy -= bigFireballEnergyCost;
                        //freeze player rotation so that they don't go wonk
                        transform.rotation = rotateFreeze;
                        //play sound
                        gameObject.GetComponent<AudioSource>().PlayOneShot(fireballSound);
                    }
                    break;
            }
            
        }

        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.E))
        {
            //switches current weapon
            SwitchWeapon();
        }

        if (timer > fireCooldownShuriken)
        {
            //reset timer and burst once the burst is out
            shurikenBurst = 3;
            timer = 0;
        }
    }
    void Fire(GameObject proj)
    {
        //clone prefab into scene
        Instantiate(proj, firePoint.position, firePoint.rotation);
       
    }

    void SwitchWeapon()
    {
        //up index of current weapon
        currentWeapon++;
        //roll over if over the max
        if (currentWeapon > maxWeapons)
        {
            currentWeapon = 1;
        }
    }

    void AmmoHandler(int weapon)
    {
        //reduce ammo to respective weapon
        switch (weapon)
        {
            case 1:
                kunaiCount--;
                break;
            case 2:
                shurikenCount--;
                break;
        }
    }

    public void IncreaseKun(int amount)
    { 
        //up the kunai ammo
        kunaiCount += amount;
    }
    public void IncreaseSha(int amount)
    {
        //up the shuriken count
        shurikenCount += amount;
    }

    void MouseTarget()
    {
        rotateFreeze = transform.rotation; //freeze roation
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; //find diff between position and mouse position
        rotz = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //apply the difference
        transform.rotation = Quaternion.Euler(0f, 0f, rotz + offset); ////set the roation of player
    }

}

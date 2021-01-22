using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    //Spawn Point for projectiles
    public Transform firePoint;

    [Header("")]
    public static int currentWeapon = 1; //int to represent currently equipped weapon
    public int maxWeapons = 1; //limit on weapons player can access

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
    public static int kunaiCount = 30;
    public static int shurikenCount = 30;

    private Vector3 difference;
    private float rotz;
    private Quaternion rotateFreeze;
    private float offset;
    public GameObject cursur;

    private bool hasKunAmmo;
    private bool hasShuAmmo;

    [SerializeField] int shurikenBurst = 3;
    [Header("Timer")]
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        playerLogic = FindObjectOfType<NEWPlayerLogic>();

    }

    // Update is called once per frame
    void Update()
    {
        if (kunaiCount > 0)
            hasKunAmmo = true;
        else
            hasKunAmmo = false;

        if (shurikenCount > 0)
            hasShuAmmo = true;
        else
            hasShuAmmo = false;
        timer += Time.deltaTime;

        if(Input.GetMouseButtonDown(0))
        {
            /*
            if (currentWeapon > 2)
                MouseTarget();
            Fire(weapons[currentWeapon - 1]);
            if (currentWeapon > 2)
                transform.rotation = rotateFreeze;
            AmmoHandler(currentWeapon);
            */
            
            switch (currentWeapon)
            {
                case 1:
                    if (timer >= fireCooldownKunai && hasKunAmmo)
                    {
                        Fire(weapons[currentWeapon - 1]);
                        //do the throw anim
                        myAnim.SetTrigger("Throw");
                        AmmoHandler(currentWeapon);
                        gameObject.GetComponent<AudioSource>().PlayOneShot(kunaiSound);
                        timer = 0;
                    }
                    break;
                case 2:
                    if (shurikenBurst > 0 && hasShuAmmo)
                    {
                        Fire(weapons[currentWeapon - 1]);
                        AmmoHandler(currentWeapon);
                        //do the throw anim
                        myAnim.SetTrigger("Throw");
                        shurikenBurst--;
                        gameObject.GetComponent<AudioSource>().PlayOneShot(shurikenSOund);
                    }
                    break;

                case 3:
                    MouseTarget();
                    Fire(weapons[currentWeapon - 1]);
                    transform.rotation = rotateFreeze;
                    gameObject.GetComponent<AudioSource>().PlayOneShot(fireballSound);
                    break;
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            SwitchWeapon();
        }

        if (timer > fireCooldownShuriken)
        {
            shurikenBurst = 3;
            timer = 0;
        }
    }

    void Fire(GameObject proj)
    {
        //projectile logic
        Instantiate(proj, firePoint.position, firePoint.rotation);
        /*
        //do the throw anim
        myAnim.SetTrigger("Throw");
        */
    }

    void SwitchWeapon()
    {
        currentWeapon++;
        if (currentWeapon > maxWeapons)
        {
            currentWeapon = 1;
        }
    }

    void AmmoHandler(int weapon)
    {
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
        kunaiCount += amount;
    }
    public void IncreaseSha(int amount)
    {
        shurikenCount += amount;
    }

    void MouseTarget()
    {
        rotateFreeze = transform.rotation;
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        rotz = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotz + offset);
    }

}

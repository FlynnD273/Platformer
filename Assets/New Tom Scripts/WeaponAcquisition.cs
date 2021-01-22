using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAcquisition : MonoBehaviour
{
    private NEWPlayerLogic playerLogic;

    bool kunaiUnlocked;
    bool ShurikenUnlocked;
    bool FireballUnlocked;

    public GameObject KunaiUnlocker;
    public GameObject ShurikenUnlocker;
    public GameObject FireballUnlocker;
    private void Start()
    {
        playerLogic = GameObject.Find("Player").GetComponent<NEWPlayerLogic>();
        FireballUnlocker = GameObject.Find("FireBallPickup");
        NEWPlayerLogic.energy = 0;
        NEWPlayerLogic.startRegen = true;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NEWPlayerLogic.energy = 60;
            NEWPlayerLogic.startRegen = false;
        }
    }

    private void Update()
    {
        if (!NEWPlayerLogic.startRegen && !FireballUnlocked)
        {
            NEWPlayerLogic.energy = 0;
            NEWPlayerLogic.startRegen = true;
        }
    }


}

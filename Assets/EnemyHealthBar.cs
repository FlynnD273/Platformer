﻿////////////////////////////
///Name: Thomas Allen
///Date: 1/25/21
///Desc: Interlinks with Melee, ranged, and Dragon AI to manage healthbar on teh enemy
/////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthBar;

    MeleeEnemy meleeEnemy;
    RangedEnemy rangedEnemy;
    DragonEnemy dragonEnemy;


    // Start is called before the first frame update
    void Start()
    {

        meleeEnemy = gameObject.GetComponentInParent<MeleeEnemy>();
        healthBar.maxValue = meleeEnemy.hitPoints;
        rangedEnemy = gameObject.GetComponentInParent<RangedEnemy>();
        healthBar.maxValue = meleeEnemy.hitPoints;
        dragonEnemy = gameObject.GetComponentInParent<DragonEnemy>();
        healthBar.maxValue = meleeEnemy.hitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (meleeEnemy != null)
            healthBar.value = meleeEnemy.hitPoints;
        if (rangedEnemy != null)
            healthBar.value = rangedEnemy.hitPoints;
        if (dragonEnemy != null)
            healthBar.value = dragonEnemy.hitPoints;
    }
}
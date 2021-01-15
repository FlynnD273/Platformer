//////////////////
//By: Thomas Allen
//Date: 1/15/2021
//Description: Manages UI elements
//////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [Header("Health and Energy Bars")]
    [SerializeField] public Slider helthSlider;
    [SerializeField] public Slider energySlider;

    [Header("Scripts from Player")]
    [SerializeField] public NEWPlayerLogic playerLogic;
    [SerializeField] public Projectile projectile;

    [Header("Weapon UI Elements")]
    public Image kunai;
    public Image shurikan;
    public Image energyOrb;
    public Image smallEnergyOrb;
    // Start is called before the first frame update
    void Start()
    {
        playerLogic = GameObject.Find("Player").GetComponent<NEWPlayerLogic>();
        projectile = GameObject.Find("Player").GetComponent<Projectile>();

    }

    public void Update()
    {
        helthSlider.value = NEWPlayerLogic.health;
        energySlider.value = NEWPlayerLogic.energy;

        ProjectileSwitch();
    }

    public void ProjectileSwitch()
    {
        switch (projectile.switchProj)
        {
            case 1:
                kunai.enabled = true;
                shurikan.enabled = false;
                energyOrb.enabled = false;
                smallEnergyOrb.enabled = false;
                break;
            case 2:
                kunai.enabled = false;
                shurikan.enabled = true;
                energyOrb.enabled = false;
                smallEnergyOrb.enabled = false;
                break;
            case 3:
                kunai.enabled = false;
                shurikan.enabled = false;
                energyOrb.enabled = true;
                smallEnergyOrb.enabled = false;
                break;
            case 4:
                kunai.enabled = false;
                shurikan.enabled = false;
                energyOrb.enabled = false;
                smallEnergyOrb.enabled = true;
                break;
            default:
                Debug.LogError("USER REACHED INVALID WEAPON INDEX: " + projectile.switchProj);
                break;
        }
    }


}

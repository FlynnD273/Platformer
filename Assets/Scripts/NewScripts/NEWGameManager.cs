//////////////////
//By: Dev Dhawan
//Date: 1/13/2020
//Description: Game manager for health and energy.
//////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NEWGameManager : MonoBehaviour
{
    private static int energy = 0;
    private static int health = 0;

    private static UnityEvent OnEnergyUpdate = new UnityEvent();
    private static UnityEvent OnHealthUpdate = new UnityEvent();

    public static int Energy
    {
        get => energy;
        set
        {
            energy = value;
            OnEnergyUpdate.Invoke();
        }
    }
    public static int Health
    {
        get => health;
        set
        {
            health = value;
            OnHealthUpdate.Invoke();
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

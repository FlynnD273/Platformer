//////////////////
//By: Dev Dhawan
//Date: 10/20/2020
//Description: This is a script meant to be added to a camera to follow a specified target
//////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWFollowingCamera : MonoBehaviour
{
    public GameObject Target;
    public GameObject healthBar;
    public GameObject energyBar;
    private Vector3 ogPosH;
    private Vector3 ogPosE;

    private Vector3 healthPos;
    private Vector3 energyPos;

    public float smoothVal = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        ogPosH = healthBar.transform.position;
        ogPosE = energyBar.transform.position;
    }

    public void FixedUpdate()
    {
        if(Target != null)
        {
            //figure out where the target is.
            Vector3 newPos = Target.transform.position;
            //maintain camera z level
            newPos.z = transform.position.z;
            //use linear interpolation to smoothly go to the target
            transform.position = Vector3.Lerp(transform.position, newPos, smoothVal);

            healthPos.x = transform.position.x + ogPosH.x;
            healthPos.y = transform.position.y + ogPosH.y;
            healthPos.z = ogPosH.z;
            healthBar.transform.position = healthPos;

            energyPos.x = transform.position.x + ogPosE.x;
            energyPos.y = transform.position.y + ogPosE.y;
            energyPos.z = ogPosE.z;
            energyBar.transform.position = energyPos;
        }
    }

    public void MoveHealthbar(float amount, bool decOrInc)
    {
        float newX;
        if (decOrInc == true)
        {
            newX = ogPosH.x - (amount / 12);
        }
        else
        {
            newX = ogPosH.x + (amount / 12);
            
        }
        ogPosH.x = newX;
    }
    public void MoveEnergybar(float amount, bool decOrInc)
    {
        float newX;
        if (decOrInc == true)
        {
            newX = ogPosE.x - (amount / 20);
        }
        else
        {
            newX = ogPosE.x + (amount / 20);

        }
        ogPosE.x = newX;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

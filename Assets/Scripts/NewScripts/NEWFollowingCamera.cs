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
    private Vector3 ogPosH;

    private Vector3 healthPos;

    public float smoothVal = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        ogPosH = healthBar.transform.position;
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
        }
    }

    public void MoveHealthbar(float amount, bool decOrInc)
    {
        float newX;
        if (decOrInc == true)
        {
            newX = ogPosH.x - (amount / 8);
        }
        else
        {
            newX = ogPosH.x + (amount / 8);
        }
        ogPosH.x = newX;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

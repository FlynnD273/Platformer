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
    public GameObject display;
    public GameObject displayS;
    public GameObject displayK;
    public GameObject displayF;
    public GameObject displayFM;

    private Vector3 ogPosH;
    private Vector3 ogPosE;
    private Vector3 ogPosD;
    private Vector3 ogPosDI;


    private Vector3 healthPos;
    private Vector3 energyPos;
    private Vector3 displayPos;

    public float smoothVal = 0.5f;
    private int switchProj;

    // Start is called before the first frame update
    void Start()
    {
        ogPosH = healthBar.transform.position;
        ogPosE = energyBar.transform.position;
        ogPosD = displayS.transform.position;
        ogPosDI = display.transform.position;
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
            if (switchProj == 5)
        {
            switchProj = 1;
        }

            healthPos.x = transform.position.x + ogPosH.x;
            healthPos.y = transform.position.y + ogPosH.y;
            healthPos.z = ogPosH.z;
            healthBar.transform.position = healthPos;

            energyPos.x = transform.position.x + ogPosE.x;
            energyPos.y = transform.position.y + ogPosE.y;
            energyPos.z = ogPosE.z;
            energyBar.transform.position = energyPos;

            displayPos.x = transform.position.x + ogPosDI.x;
            displayPos.y = transform.position.y + ogPosDI.y;
            displayPos.z = ogPosDI.z;
            display.transform.position = displayPos;

            ProjectileSwitch();
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
            newX = ogPosE.x - (amount / 10);
        }
        else
        {
            newX = ogPosE.x + (amount / 10);
        }
        ogPosE.x = newX;
    }

    public void ProjectileSwitch()
    {
        if (switchProj == 1)
        {
            displayK.SetActive(true);
            displayS.SetActive(false);
            displayFM.SetActive(false);
            displayF.SetActive(false);
        }
        if (switchProj == 2)
        {
            displayK.SetActive(false);
            displayS.SetActive(true);
            displayFM.SetActive(false);
            displayF.SetActive(false);
        }
        if (switchProj == 3)
        {
            displayK.SetActive(false);
            displayS.SetActive(false);
            displayFM.SetActive(true);
            displayF.SetActive(false);
        }
        if (switchProj == 4)
        {
            displayK.SetActive(false);
            displayS.SetActive(false);
            displayFM.SetActive(false);
            displayF.SetActive(true);
        }
    }
    public void SwitchProj(int switchP)
    {
        switchProj = switchP;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

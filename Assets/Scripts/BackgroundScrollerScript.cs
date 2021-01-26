/*
 * Made by Abhi
 * Jn 24, 2021
 * Background parralx effect script
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrollerScript : MonoBehaviour
{
    //the length and the start position of the sprite
    private float length, startpos;
    //the camera that it follows
    public GameObject cam;
    //the intesity of the parrallax effect
    public float parralaxEffect;

    private void Start()
    {
        //getting the x position of the sprite attacked to
        startpos = transform.position.x;
        //getting the horizontal size of the background
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    private void FixedUpdate()
    {
        //checks if out of bound
        float temp = (cam.transform.position.x * (1 - parralaxEffect));

        //how far the background moves as the camera moves
        float dist = (cam.transform.position.x * parralaxEffect);

        //changing the position so that the individual layer moves basedon the intensity of the parralax effect
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        //checks if out of bounds and repeats.
        if (temp > startpos + length) startpos += length;
        if (temp < startpos - length) startpos -= length;
    }
}

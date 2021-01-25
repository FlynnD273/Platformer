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
    private float length, startpos;
    public GameObject cam;
    public float parralaxEffect;

    private void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    private void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parralaxEffect));
        float dist = (cam.transform.position.x * parralaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        if (temp < startpos - length) startpos -= length;
    }
}

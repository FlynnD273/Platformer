using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrollerScript : MonoBehaviour
{
    [SerializeField] float backgroundScrollSpeed = 0.5f;
    Material material;
    Vector2 offset;
    Vector3 zero;
    [SerializeField] GameObject player;
    bool isTouchingWall;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        //transform.position = mainCamera.transform.position;
        offset = new Vector2(backgroundScrollSpeed, 0f);

    }

    private void Update()
    {
        isTouchingWall = player.GetComponent<CooperPlayerController>().isTouchingWall;

        Debug.Log(Input.GetAxis("Horizontal"));
        if ((Input.GetAxis("Horizontal") > 0f || Input.GetAxis("Horizontal") < 0f) && !isTouchingWall) 
        {
            material.mainTextureOffset += offset * Time.deltaTime;
        }
        zero = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        transform.position = zero;
    }
}

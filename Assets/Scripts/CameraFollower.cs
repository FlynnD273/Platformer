///////////////
///By: Thomas Allen
///Date: 10/20/2020
///Description: This is a script meant to be added 
///to a camera to follow a specified target
////////////////



using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    //target of follow 
    public GameObject Target;
    public float smoothVal = 0.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Target != null)
        {
            //figure our where the target is
            Vector3 newPos = Target.transform.position;
            //maintain cam z
            newPos.z = transform.position.z;
            //use linear interpolation to smoothly go to the target
            transform.position = Vector3.Lerp(transform.position, newPos, smoothVal);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Transform[] Waypoints;
    public float Speed;

    private int di;
    private int index;

    private float progress;
    private float distance;

    private Rigidbody2D rb;
    //private Rigidbody2D pRb;
    //private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        progress = 0;
        di = 1;
        index = 0;
        distance = ((Vector2)(Waypoints[index].position - Waypoints[index + di].position)).magnitude;

        GetComponent<CapsuleCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().bounds.size.x, 0.3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(Vector3.Lerp(Waypoints[index].position, Waypoints[index + di].position, progress));
        progress += Speed / Mathf.Max(distance, 1) * Time.deltaTime;

        if (progress > 1)
        {
            Next();
        }
    }

    private void Next ()
    {
        index += di;
        if (index <= 0)
        {
            index = 0;
            di = 1;
        }
        if (index >= Waypoints.Length - 1)
        {
            index = Waypoints.Length - 1;
            di = -1;
        }

        progress = 0;
        distance = ((Vector2)(Waypoints[index].position - Waypoints[index + di].position)).magnitude;
    }


    private void OnDrawGizmos()
    {
        //Handles.DrawAAPolyLine(5, Waypoints.Select(o => o.position).ToArray());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            var p = col.gameObject.GetComponent<PlayerController>();
            p.Platform = rb;
            p.LastPlatform = rb.position;
            p.OnPlatform = true;

            //col.transform.parent = transform;

            //var r = col.gameObject.GetComponent<RelativeJoint2D>();
            //r.enabled = true;
            //r.connectedBody = rb;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerController>().OnPlatform = false;
            //col.transform.parent = null;

            //pRb = null;
            //var r = col.gameObject.GetComponent<Rigidbody2D>();
            //r.enabled = false;
            //r.connectedBody = null;
        }
    }
}

/********************************
 * Author: Flynn Duniho
 * Date: 12/18/2020
 * Purpose: Set the player respawn point
********************************/

using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector2 SpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        SpawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector3)SpawnPoint + new Vector3(0, Mathf.Sin(Time.realtimeSinceStartup) * 0.1f, transform.position.z);

        if (PlayerController.Player.StartPos.Equals(SpawnPoint))
        {
            sr.color = Color.green;
        }
        else
        {
            sr.color = Color.yellow;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerController.Player.StartPos = SpawnPoint;
        }
    }
}

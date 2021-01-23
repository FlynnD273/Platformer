/********************************
 * Author: Flynn Duniho
 * Date: 12/18/2020
 * Purpose: Goes to next level when touching the player
********************************/

using UnityEngine;

public class GoalController : MonoBehaviour
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
        transform.position = (Vector3)SpawnPoint + new Vector3(0, Mathf.Sin(Time.realtimeSinceStartup * 1.2f + 0.3f) * 0.5f, transform.position.z);
        //sr.color = Color.HSVToRGB(((Time.realtimeSinceStartup / 10) % 1), 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            //Next Level
            GameManager.Manager.NextLevel();
        }
    }
}

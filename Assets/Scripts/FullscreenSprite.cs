using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenSprite : MonoBehaviour
{
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    private void LateUpdate()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float cameraHeight = cam.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(cam.aspect * cameraHeight, cameraHeight);
        Vector3 spriteSize = spriteRenderer.sprite.bounds.size;

        Vector2 scale = new Vector2(1, 1);
        if (cameraSize.x / spriteSize.x >= cameraSize.y / spriteSize.y)
        { // Landscape (or equal)
            scale *= cameraSize.x / spriteSize.x;
        }
        else
        { // Portrait
            scale *= cameraSize.y / spriteSize.y;
        }

        transform.localScale = scale * 1.5f;

        transform.position = cam.transform.position / -5;
        transform.localPosition = new Vector3(Mathf.Max(Mathf.Min(transform.position.x, cameraSize.x / 4), cameraSize.x / -4), Mathf.Max(Mathf.Min(transform.position.y, cameraSize.y / 4), cameraSize.y / -4), 50);// + cam.transform.position;
    }
}

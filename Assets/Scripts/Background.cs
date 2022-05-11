using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    /*
        Moves the background to the left/right based on the
        position of the camera.
        An improvement can be made by making this work with
        a single background image instead of two, and mapping
        the position each has to be at according to the camera's
        size.
        This works well as of now, but there is some weird behavior
        whenever the camera fully covers one of the images (since
        the other tries to get within boundary.)
        It's not noticeableby the player.
    */
    private SpriteRenderer sprite;
    private Vector2 initialPosition;
    private int offset;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector3 topRightBound = Camera.main.ScreenToWorldPoint(
            new Vector3(
                Screen.width,
                Screen.height,
                Camera.main.transform.position.z
            )
        );

        Vector3 botLeftBound = Camera.main.ScreenToWorldPoint(
            new Vector3(
                0,
                0,
                Camera.main.transform.position.z
            )
        );

        if (transform.position.x + (sprite.size.x / 2) < botLeftBound.x)
        {
            transform.position = initialPosition + new Vector2(
                sprite.size.x * (offset++ * 2),
                0
            );
        }

        else if (transform.position.x - (sprite.size.x / 2) > topRightBound.x)
        {
            transform.position = initialPosition + new Vector2(
                sprite.size.x * (offset-- * 2),
                0
            );
        }
    }
}

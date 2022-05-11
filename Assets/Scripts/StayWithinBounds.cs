using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit: YT @Press Start - Unity - Keeping The Player Within Screen Boundaries

public class StayWithinBounds : MonoBehaviour
{
    public bool constrainPosY;
    public bool constrainNegY;
    public bool constrainPosX;
    public bool constrainNegX;

    private Vector2 upperRightScreenBound;
    private Vector2 lowerLeftScreenBound;

    // Update is called once per frame
    void LateUpdate()
    {
        upperRightScreenBound = Camera.main.ScreenToWorldPoint(
            new Vector3(
                Screen.width,
                Screen.height,
                Camera.main.transform.position.z
            )
        );
        lowerLeftScreenBound = Camera.main.ScreenToWorldPoint(
            new Vector3(
                0,
                0,
                Camera.main.transform.position.z
            )
        );

        Vector3 viewPos = transform.position;

        float minX = constrainNegX ? lowerLeftScreenBound.x : viewPos.x;
        float maxX = constrainPosX ? upperRightScreenBound.x : viewPos.x;

        float minY = constrainNegY ? lowerLeftScreenBound.y : viewPos.y;
        float maxY = constrainPosY ? upperRightScreenBound.y : viewPos.y;

        viewPos.x = Mathf.Clamp(viewPos.x, minX, maxX);
        viewPos.y = Mathf.Clamp(viewPos.y, minY, maxY);
        transform.position = viewPos;
    }
}

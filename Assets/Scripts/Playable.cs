using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class Playable : MonoBehaviour
{
    [Header("Focused Attributes")]
    public bool isFocused = false;
    public Light2D focusedLight;

    protected virtual void Update()
    {

    }

    public void turnSwapLightOff()
    {
        focusedLight.enabled = false;
    }

    public void turnSwapLightOn()
    {
        focusedLight.enabled = true;
    }
}

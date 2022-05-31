using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class Torch : Interactable
{
    [Header("Animation")]
    #region animation
    public Animator animator;
    public float anim_delay;
    public bool isIgnited = false;
    #endregion

    #region lights
    public Light2D off_light;
    public Light2D ignited_light;
    #endregion

    #region water
    [Header("Water")]
    [SerializeField] private int waterThreshold;
    private int _waterCount = 0;
    #endregion

    [Header("Misc")]
    private AudioSource source;

    public Collider2D safety_zone;

    void Start()
    {
        source = GetComponent<AudioSource>();

        if (isIgnited)
        {
            StartCoroutine(lightUp(false));
        }
    }

    override public void TriggerInteraction(GameObject actor)
    {
        if (actor.name == "Spirit" && !isIgnited)
        {
            StartCoroutine(lightUp(true));
        }
        if (actor.tag == "Moth")
        {
            lightOff();
        }
    }

    private IEnumerator lightUp(bool delay)
    {
        if (delay)
        {
            yield return new WaitForSecondsRealtime(anim_delay);

            // Only play sound when the full animation is playing
            source.Play();
        }

        Physics2D.IgnoreLayerCollision(
            gameObject.layer,
            LayerMask.GetMask("Water"),
            false
        );

        // Play animation
        animator.SetBool("isOn", true);

        // Turn off dim light
        off_light.enabled = false;

        // Turn on new light and enable safe zone
        ignited_light.enabled = true;
        safety_zone.enabled = true;
        isIgnited = true;
    }

    private void lightOff()
    {
        Physics2D.IgnoreLayerCollision(
            gameObject.layer,
            LayerMask.GetMask("Water"),
            true
        );
        
        // Play animation
        animator.SetBool("isOn", false);

        // Turn on dim light
        off_light.enabled = true;

        // Turn off lit light and disable safe zone
        ignited_light.enabled = false;
        safety_zone.enabled = false;
        isIgnited = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Water"))
        {
            _waterCount++;
            if (_waterCount >= waterThreshold)
            {
                lightOff();
            }
        } 
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Water")) {
            _waterCount--;
        }
    }

}

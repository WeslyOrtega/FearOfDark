using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMovement : Playable
{

    public float speed = 10f;

    #region animation
    public Animator animator;
    private bool isFliped = false; // true = facing to the left
    #endregion

    #region interaction
    public GameObject interactionRadius;
    private bool isInteracting = false;
    #endregion

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        if (!this.isFocused) return;

        // Gets the current animation and compares the name
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Ignite")) return;

        float x_dir = Input.GetAxis("Horizontal");
        float y_dir = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(x_dir * this.speed, y_dir * this.speed);

        if (Input.GetButtonDown("Interact"))
        {
            isInteracting = true;
            rb.velocity = new Vector2(); // Reset the velocity
            OnInteract();
        }
        else
        {
            isInteracting = false;
        }

        animate();
    }

    private void OnInteract()
    {
        // Get all the colliders in range
        Collider2D[] inRange = Physics2D.OverlapCircleAll(
            transform.position,
            interactionRadius.transform.localScale.x
        );

        foreach (Collider2D collider in inRange)
        {
            // Check which of those objects can be interacted with
            Interactable interactable = collider.gameObject.GetComponent<Interactable>();
            if (interactable)
            {
                interactable.TriggerInteraction(gameObject);
            }
        }
    }

    private void animate()
    {
        #region flip sprite based on velocity
        if (rb.velocity.x < 0 && !isFliped)
        {
            transform.Rotate(0, 180, 0);
            isFliped = true;
        }
        else if (rb.velocity.x > 0 && isFliped)
        {
            transform.Rotate(0, 180, 0);
            isFliped = false;
        }
        #endregion

        #region ignition
        if (isInteracting)
        {
            animator.SetTrigger("IgniteAction");
        }
        #endregion
    }

}

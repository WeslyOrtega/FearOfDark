using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit: Movement + jumping is a modified version of
// YT @Dawnosaur Studios - "Improve Your Platformer with Forces"

public class KidMovement : Playable
{
    #region walk variables
    [Header("Walking")]
    public float walk_speed; // Player walking max speed
    public float walk_acceleration; // How fast the player will stop moving
    public float walk_deceleration; //How fast the player will come to a stop
    #endregion

    #region jump variables
    [Header("Jumping")]
    public GameObject feet_area;
    public float jump_force;
    public float jump_release_multiplier;

    // Amount of time a player can press jump before touching ground
    public float jump_press_buffer_time;
    public float gravity_scale;
    public float falling_gravity_scale;

    private bool isJumping = false;
    private float last_pressed_jump;
    private bool has_jumped = true;
    #endregion

    [Header("Other")]
    [SerializeField] private GameObject interactionRadius;

    #region animation
    public Animator animator;
    private bool isFliped = false; // true = facing to the left
    private bool isScared;
    private bool isRunning;
    #endregion

    #region misc
    private Rigidbody2D rb;
    private int touchingLights = 0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        last_pressed_jump = jump_press_buffer_time; // Prevents jump upon load
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        // Update whether character should be scared
        if (!checkTouchingLight())
        {
            this.isScared = true;
        }
        else
        {
            this.isScared = false;
        }

        // Only read actions when focused and receiving light
        if (this.isFocused && !this.isScared)
        {
            if (Input.GetButtonDown("Jump"))
            {
                has_jumped = false;
                last_pressed_jump = 0;
                onJumpDown();
            }
            // Give player a window before touchign ground to press jump
            else if (last_pressed_jump < jump_press_buffer_time
                && !has_jumped)
            {
                onJumpDown();
            }
            else if (Input.GetButtonUp("Jump"))
            {
                onJumpUp();
            }

            if (Input.GetButtonDown("Interact"))
            {
                OnInteract();
            }
        }

        last_pressed_jump += Time.deltaTime;


        if (checkCanJump() && last_pressed_jump > .05)
        {
            isJumping = false;
        }

        // Handle all animations
        animate();
    }

    void FixedUpdate()
    {
        float target_speed = Input.GetAxisRaw("Horizontal") * walk_speed;

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = falling_gravity_scale;
        }
        else
        {
            rb.gravityScale = gravity_scale;
        }

        // Clear speed if the character is not in focus
        if (!this.isFocused || this.isScared)
        {
            target_speed = 0;
        }

        if (target_speed != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        // Distance from target to current
        float diff = target_speed - rb.velocity.x;

        // Get whether the player is trying to stop
        float accelRate = (Mathf.Abs(target_speed) > 0.01f) ?
            walk_acceleration : walk_deceleration;

        // Add force
        float movement = diff * accelRate;
        rb.AddForce(movement * Vector2.right);
    }

    private bool checkCanJump()
    {
        // Check if the feet area is touching ground
        if (Physics2D.OverlapBox(
                feet_area.transform.position,
                feet_area.transform.lossyScale,
                0,
                LayerMask.GetMask(new string[] { "Ground" }))
            )
        {
            return true;
        }
        return false;
    }

    private void onJumpDown()
    {
        // Check if the player is in contact with the ground.
        if (checkCanJump())
        {
            // In case player still has some downwards velocity
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);

            has_jumped = true;
            isJumping = true;
            last_pressed_jump = 0;
        }
    }

    private void onJumpUp()
    {
        // Give the player a downwards force when releasing jump
        if (rb.velocity.y > 0 && isJumping)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * jump_release_multiplier, ForceMode2D.Impulse);
        }
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
        if (rb.velocity.x < -0.01f && !isFliped)
        {
            transform.Rotate(0, 180, 0);
            isFliped = true;
        }
        else if (rb.velocity.x > 0.01f && isFliped)
        {
            transform.Rotate(0, 180, 0);
            isFliped = false;
        }
        #endregion

        #region scared
        if (this.isScared)
        {
            animator.SetBool("isScared", true);
        }
        else
        {
            animator.SetBool("isScared", false);
        }
        #endregion

        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isRunning", isRunning);
    }

    private bool checkTouchingLight()
    {
        // Check if the player is touching at least one light
        return touchingLights > 0;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check whether the player is touching light
        if (collider.tag == "LightSafety")
        {
            // Increment the number of lights touching the player
            touchingLights++;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        // Check if the player left a light
        if (collider.tag == "LightSafety")
        {
            // Decrement the number of lights touching the player
            touchingLights--;
        }
    }

}

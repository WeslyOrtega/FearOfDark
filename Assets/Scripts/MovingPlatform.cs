using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // The points the object will move towards.
    public GameObject[] points;
    public float speed = 1;

    // Whether the object should restart the cycle after reaching
    // The final point
    public bool loop;

    // Amount of time to wait before heading to the next point
    public float delay_between;

    // Whether the object should wait for the player before starting
    // to move. Will not stop upon player leaving.
    public bool needsPlayer;

    // Timer variable. Used to keep track of how long the object needs
    // to wait to meet the @delay_between parameter
    private float time_before_start = 0;
    private Rigidbody2D rb;
    private int currentTarget = 0; // Heading towards this point
    private bool canMove = false; // Whether the object is allowed to move

    public Animator animator;
    private bool isSteppedOn;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!needsPlayer)
        {
            canMove = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        animate();

        // Update time that has passed since stopped
        time_before_start -= Time.deltaTime;

        if (currentTarget >= points.Length // If ran out of points
            || !canMove // If not allowed to move
            || time_before_start > 0) // If timer hasn't passed
        {
            // If object is set to loop
            if (loop)
            {
                // Note that, even if the other two conditions are true,
                // this will not change anything
                currentTarget %= points.Length;
            }
            return;
        }

        // In case that the object is not set in the editor
        if (points[currentTarget] is null)
        {
            Debug.LogError("Platform point #" + currentTarget + "is missing.");
            return;
        }

        // Get position of target
        Vector2 targetPos = points[currentTarget].transform.position;

        // Check if object reached the target
        if (Vector2.Distance(transform.position, targetPos) == 0)
        {
            time_before_start = delay_between; // Reset timer
            currentTarget++;
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            points[currentTarget].transform.position,
            speed * Time.deltaTime
        );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the object collides with the object, set the player
        // as a child of the object. This ensures that the player
        // will move with the object.
        if (collision.collider.CompareTag("Player"))
        {
            canMove = true;
            collision.gameObject.transform.SetParent(transform);
        }

        if (collision.collider.name == "Kid")
        {
            isSteppedOn = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Remove player as child if the player exits the object
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }

        if (collision.collider.name == "Kid")
        {
            isSteppedOn = false;
        }
    }

    private void animate()
    {

        if (isSteppedOn)
        {
            animator.SetBool("isSteppedOn", true);
        }
        else
        {
            animator.SetBool("isSteppedOn", false);
        }

        if (canMove)
        {
            animator.SetBool("isAwake", true);
        }
    }
}

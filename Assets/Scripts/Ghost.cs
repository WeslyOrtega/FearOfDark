using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    #region target
    [Header("Target")]
    public GameObject target;
    public GameObject playerDetectionRange;
    public float chaseSpeed;
    private LevelManager levelManager;
    #endregion

    #region fear
    [Header("Fear")]
    public GameObject afraidRange;
    public float afraidSpeed;
    public float afraidCooldown;
    private float remainingCooldown;
    private Vector2 prevDirection;
    #endregion

    #region animation
    [Header("Animation")]
    // For animation
    public PolygonCollider2D[] colliders;
    private int currColider;
    private bool isFliped;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currColider = 0;
        colliders[currColider].enabled = true;
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction;
        Collider2D fearObj = checkFear();

        if (fearObj) // If there is something that scares the ghost
        {
            direction = fearObj.transform.position - transform.position;
            direction.Normalize();
            direction *= afraidSpeed * Time.fixedDeltaTime * -1;
            remainingCooldown = afraidCooldown;
            prevDirection = direction;
        }
        else if (remainingCooldown > 0) // If the ghost is already running
        {
            remainingCooldown -= Time.fixedDeltaTime;
            direction = prevDirection;
        }
        else // Try to chase the player
        {

            if (!isPlayerWithinRange()) return;

            direction = target.transform.position - transform.position;
            direction.Normalize();
            direction *= chaseSpeed * Time.fixedDeltaTime;
        }

        transform.position = (Vector2)transform.position + direction;
        animate(direction);
    }

    private bool isPlayerWithinRange()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(
            playerDetectionRange.transform.position,
            playerDetectionRange.transform.lossyScale.x / 2,
            LayerMask.GetMask(new string[] { "Player" })
        );

        foreach (Collider2D obj in inRange)
        {
            if (obj.name == "Kid")
            {
                return true;
            }
        }

        return false;
    }

    private Collider2D checkFear()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(
            afraidRange.transform.position,
            afraidRange.transform.lossyScale.x / 2,
            LayerMask.GetMask(new string[] { "Player", "Background" })
        );

        foreach (Collider2D obj in inRange)
        {
            if (obj.name == "Spirit" || obj.tag == "LightSafety")
            {
                return obj;
            }
        }

        return null;
    }

    public void nextCollider()
    {
        colliders[currColider].enabled = false;
        currColider = (currColider + 1) % colliders.Length;
        colliders[currColider].enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Kid")
        {
            levelManager.ResetScene();
        }
    }

    private void animate(Vector2 direction)
    {
        if (direction.x < 0 && !isFliped)
        {
            transform.Rotate(0, 180, 0);
            isFliped = true;
        }
        else if (direction.x > 0 && isFliped)
        {
            transform.Rotate(0, 180, 0);
            isFliped = false;
        }
    }
}

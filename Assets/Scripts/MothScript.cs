using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothScript : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private GameObject interactionRadius;
    [SerializeField] private GameObject lightRange;

    private Animator animator;
    private bool isFlipped = true;

    private LevelManager levelManager;
    private GameObject spirit;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        levelManager = FindObjectOfType<LevelManager>();
        spirit = FindObjectOfType<SpiritMovement>().gameObject;
    }

    void Update()
    {
        Collider2D torch = FindTorch();
        Vector3 target;
        if (torch is not null)
        {
            target = torch.transform.position;

            if (Vector2.Distance(transform.position, torch.transform.position)
                < interactionRadius.transform.lossyScale.x / 2)
            {
                torch.GetComponent<Torch>().TriggerInteraction(gameObject);
            }
        }
        else
        {
            target = spirit.transform.position;
        }

        Animate(target - transform.position);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private Collider2D FindTorch()
    {

        Collider2D[] inRange = Physics2D.OverlapCircleAll(
            lightRange.transform.position,
            lightRange.transform.lossyScale.x / 2,
            LayerMask.GetMask(new string[] { "Background" })
        );

        Collider2D nearest = null;
        foreach (Collider2D obj in inRange)
        {
            Torch t = obj.GetComponent<Torch>();
            if (t is not null && t.isIgnited)
            {
                if (
                    nearest is null
                    || Vector2.Distance(transform.position, obj.transform.position)
                    < Vector2.Distance(transform.position, nearest.transform.position)
                    )
                {
                    nearest = obj;
                }
            }
        }

        return nearest;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Spirit")
        {
            levelManager.ResetScene();
        }
    }

    void Animate(Vector3 movementDir)
    {
        if (movementDir.x < 0 && !isFlipped)
        {
            transform.Rotate(0, 180, 0);
            isFlipped = true;
        }
        else if (movementDir.x > 0 && isFlipped)
        {
            transform.Rotate(0, 180, 0);
            isFlipped = false;
        }
    }
}
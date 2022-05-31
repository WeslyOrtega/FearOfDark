using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothScript : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] public GameObject interactionRadius;
    [SerializeField] public GameObject lightRange;
    private LevelManager levelManager;
    private GameObject spirit;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        spirit = FindObjectOfType<SpiritMovement>().gameObject;
    }

    void Update()
    {
        Collider2D torch = FindTorch();
        if (torch is not null)
        {
            transform.position = Vector2.MoveTowards(transform.position, torch.transform.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, torch.transform.position)
                < interactionRadius.transform.lossyScale.x / 2)
            {
                torch.GetComponent<Torch>().TriggerInteraction(gameObject);
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, spirit.transform.position, speed * Time.deltaTime);
        }
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
}
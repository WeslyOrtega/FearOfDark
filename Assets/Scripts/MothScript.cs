using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MothScript : MonoBehaviour
{
  [SerializeField] private GameObject spirit; 
  [SerializeField] private float speed = 1.5f; 
  Torch script;
  public GameObject interactionRadius;
  public GameObject lightRange;
  private LevelManager levelManager;
  /*void Awake()
  {
      script = torch.GetComponent<Torch>();
  }*/

 // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

  void FixedUpdate()
  {      
    Collider2D torch = FindTorch();
    if(torch)
    {
        script = torch.GetComponent<Torch>();
        if(script.isIgnited){
            transform.position = Vector2.MoveTowards(transform.position, torch.transform.position, speed * Time.deltaTime);
        }
        else
        {
        transform.position = Vector2.MoveTowards(transform.position, spirit.transform.position, speed * Time.deltaTime);
        }
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
    else
    {
        transform.position = Vector2.MoveTowards(transform.position, spirit.transform.position, speed * Time.deltaTime);
    }
    


    /*if(script.isIgnited){
    transform.position = Vector2.MoveTowards(transform.position, torch.transform.position, speed * Time.deltaTime);
    }
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
        }*/
  }

  private Collider2D FindTorch()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(
            lightRange.transform.position,
            lightRange.transform.lossyScale.x / 2,
            LayerMask.GetMask(new string[] { "Player", "Background" })
        );

        foreach (Collider2D obj in inRange)
        {
            if (obj.tag == "Torch")
            {
                return obj;
            }
        }

        return null;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.name);
        if (collider.name == "Spirit")
        {
            Debug.Log("Entered");
            levelManager.ResetScene();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MothScript : MonoBehaviour
{
  [SerializeField] private GameObject torch; 
  [SerializeField] private float speed = 1.5f; 
  Torch script;
  public GameObject interactionRadius;

  void Awake()
  {
      script = torch.GetComponent<Torch>();
  }

  void FixedUpdate()
  {      
    if(script.isIgnited){
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
        }
  }
}
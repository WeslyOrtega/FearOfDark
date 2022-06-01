using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{

    [SerializeField] private bool canSwitchOff;
    [SerializeField] private Interactable[] toBeTriggered;

    private Animator animator;
    private bool isSwitched = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void TriggerInteraction(GameObject actor)
    {
        if (actor.name == "Kid")
        {
            Toggle();
        }
    }

    private void Toggle()
    {
        if (!isSwitched)
        {
            animator.SetBool("SwitchedOn", true);
            isSwitched = true;
            foreach (Interactable i in toBeTriggered)
            {
                i.TriggerInteraction(gameObject);
            }
        }
        else if (isSwitched && canSwitchOff)
        {
            animator.SetBool("SwitchedOn", false);
            isSwitched = false;
            foreach (Interactable i in toBeTriggered)
            {
                i.TriggerInteraction(gameObject);
            }
        }
    }
}

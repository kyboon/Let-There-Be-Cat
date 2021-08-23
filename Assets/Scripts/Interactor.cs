using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{

    List<InteractableProp> interactableProps = new List<InteractableProp>();
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (interactableProps.Count > 0)
            {
                interactableProps[interactableProps.Count - 1].Interact();
            }

            if (animator != null)
                animator.SetTrigger("Interact");
        }
    }
    public void RegisterInteractable(InteractableProp prop)
    {
        // hide the prompt on all older props
        foreach (InteractableProp oldProp in interactableProps)
            oldProp.hidePrompt();

        // show prompt on new prop
        interactableProps.Add(prop);
        prop.showPrompt();
    }

    public void DeregisterInteractable(InteractableProp prop)
    {
        interactableProps.Remove(prop);

        // show prompt for the last prop
        if (interactableProps.Count > 0)
        {
            interactableProps[interactableProps.Count - 1].showPrompt();
        }
    }
}

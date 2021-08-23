using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    private InteractableProp interactable;

    void Start()
    {
        interactable = GetComponentInParent<InteractableProp>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactable != null && collision.gameObject.layer == 3)
        {
            Interactor interactor = collision.GetComponent<Interactor>();
            if (interactor != null)
                interactable.PlayerEntered(interactor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interactable != null && collision.gameObject.layer == 3)
        {
            Interactor interactor = collision.GetComponent<Interactor>();
            if (interactor != null)
                interactable.PlayerLeft(interactor);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableProp : MonoBehaviour
{
    public GameObject promptObject;

    [HideInInspector]
    public bool hasInteracted = false;
    private Interactor interactor;

    [HideInInspector]
    public bool canOnlyInteractOnce = true;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        hidePrompt();
    }

    public virtual void Interact()
    {
        if (canOnlyInteractOnce)
        {
            hasInteracted = true;
            hidePrompt();
            if (interactor != null)
                interactor.DeregisterInteractable(this);
        }
        Debug.Log("Interacted with " + gameObject.ToString());
    }

    public void PlayerEntered(Interactor interactor)
    {
        if (!hasInteracted)
        {
            this.interactor = interactor;
            interactor.RegisterInteractable(this);
            showPrompt();
        }
    }
    public void PlayerLeft(Interactor interactor)
    {
        interactor.DeregisterInteractable(this);
        hidePrompt();
    }
    public void showPrompt()
    {
        if (promptObject != null && !hasInteracted)
        {
            promptObject.SetActive(true);
        }
    }
    public void hidePrompt()
    {
        if (promptObject != null)
        {
            promptObject.SetActive(false);
        }
    }
}

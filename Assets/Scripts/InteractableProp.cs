using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableProp : MonoBehaviour
{
    public GameObject promptObject;
    private bool hasInteracted = false;
    private Interactor interactor;
    // Start is called before the first frame update
    void Start()
    {
        hidePrompt();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Interact()
    {
        hasInteracted = true;
        hidePrompt();
        if (interactor != null)
            interactor.DeregisterInteractable(this);
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

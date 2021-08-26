using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartInteractor : InteractableProp
{
    public override void Interact()
    {
        base.Interact();

        SceneManager.LoadSceneAsync(1);
    }
}

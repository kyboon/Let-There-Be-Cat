using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInteractor : InteractableProp
{
    public int sceneNumber = 0;
    public override void Interact()
    {
        base.Interact();

        if (sceneNumber == -1)
        {
            Application.Quit();
        } else 
        SceneManager.LoadSceneAsync(sceneNumber);
    }
}

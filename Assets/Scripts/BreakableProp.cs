using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProp : InteractableProp
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        base.Interact();
        Destroy(gameObject, 0.1f);
    }
}

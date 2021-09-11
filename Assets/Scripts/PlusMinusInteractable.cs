using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlusMinusInteractable : InteractableProp
{
    // Start is called before the first frame update
    public Slider slider;
    public float interval = 0.1f;

    void Awake()
    {
        this.canOnlyInteractOnce = false;
    }

    public override void Interact()
    {
        base.Interact();

        slider.value = Mathf.Clamp01(slider.value += interval);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInteractable : InteractableProp
{
    public bool isOn = true;
    private SpriteRenderer sp;

    public Sprite onSprite;
    public Sprite offSprite;

    void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        isOn = PlayerPrefs.GetInt("ShowCutscene", 1) == 1;


        this.canOnlyInteractOnce = false;

        updateSprite();
    }
    public override void Interact()
    {
        base.Interact();

        isOn = !isOn;

        PlayerPrefs.SetInt("ShowCutscene", isOn ? 1 : 0);
        updateSprite();
    }

    private void updateSprite()
    {
        if (isOn)
            sp.sprite = onSprite;
        else
            sp.sprite = offSprite;
    }

    private void OnMouseDown()
    {
        Interact();

        Debug.Log("On mouse down");
    }
}

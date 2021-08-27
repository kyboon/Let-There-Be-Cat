using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProp : InteractableProp
{
    // Start is called before the first frame update
    public List<Sprite> breakSprites = new List<Sprite>();
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public AnimatorOverrideController animOverride;
    private bool hasBreakAnimation = false;

    public ParticleSystem breakParticle;

    protected override void Start()
    {
        base.Start();
        if (animator != null && animOverride != null)
        {
            animator.enabled = false;
            animator.runtimeAnimatorController = animOverride;
            hasBreakAnimation = true;
        }
        canOnlyInteractOnce = breakSprites.Count + (hasBreakAnimation ? 1 : 0) <= 1; // if more than one sprites, can interact more than once
        hasInteracted = breakSprites.Count + (hasBreakAnimation ? 1 : 0) == 0; // if no break sprites, assume interacted
    }

    public override void Interact()
    {
        base.Interact();

        if (breakSprites.Count == 0 && animator != null)
        {
            animator.enabled = true;
            animator.SetTrigger("Break");
        }

        if (spriteRenderer != null && breakSprites.Count > 0)
        {
            spriteRenderer.sprite = breakSprites[0];
            breakSprites.RemoveAt(0);
        }

        if (breakSprites.Count == 0 && breakParticle != null)
            breakParticle.Play(true);

        canOnlyInteractOnce = breakSprites.Count + (hasBreakAnimation ? 1 : 0) <= 1;

        addScore();
    }
}

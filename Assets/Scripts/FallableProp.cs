using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FallListener
{
    public void Broken(GameObject prop);
}
public class FallableProp : InteractableProp
{
    // Start is called before the first frame update
    private bool isFallen = false;
    [HideInInspector]
    public bool isBroken = false;
    public float fallRotateSpeed = 270f;
    public Animator animator;
    public AnimatorOverrideController animOverride;

    public int breakAudioIndex = -1;

    public FallListener listener;

    protected override void Start()
    {
        base.Start();
        if (animator != null && animOverride != null)
        {
            animator.runtimeAnimatorController = animOverride;
        }
    }
    public override void Interact()
    {
        base.Interact();
        StartCoroutine(StartFall());
    }

    IEnumerator StartFall()
    {
        yield return new WaitForSeconds(0.3f);
        isFallen = true;
        gameObject.layer = 7; // change layer to fell prop
    }

    public void FixedUpdate()
    {
        if (isFallen && !isBroken)
        {
            transform.Rotate(0, 0, fallRotateSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBroken) return;

        if (collision.gameObject.layer == 8) // if touched ground
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isBroken = true;

            if (animator != null)
            {
                animator.SetTrigger("Break");
            }

            addScore();

            if (listener != null)
            {
                listener.Broken(gameObject);
            }

            if (breakAudioIndex >= 0)
            {
                AudioManager.instance.PlaySound(breakAudioIndex);
            }
        }
    }
}

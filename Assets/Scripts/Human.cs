using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform playerTransform;

    public float grabRange = 1f;
    public float pursueRange = 6f;
    public float lostTrackRange = 8f;
    public float pursueSpeed = 1f;
    private bool pursuing = false;

    private Animator animator;

    void Start()
    {
        playerTransform = PlayerManager.instance.gameObject.transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            float distance = Mathf.Abs(playerTransform.position.x - transform.position.x);
            float direction = (playerTransform.position.x - transform.position.x > 0) ? 1 : -1;

            if (!pursuing && distance < pursueRange)
            {
                pursuing = true;
                if (animator != null)
                    animator.SetBool("Walking", true);
            } else if (pursuing && distance > lostTrackRange)
            {
                pursuing = false;
                if (animator != null)
                    animator.SetBool("Walking", false);
            }

            if (pursuing)
            {
                transform.Translate(direction * pursueSpeed * Time.fixedDeltaTime, 0, 0);
                transform.localScale = new Vector3(direction, 1, 1);
            } else
            {
                // TODO Idle walk around
            }
        }
    }
}

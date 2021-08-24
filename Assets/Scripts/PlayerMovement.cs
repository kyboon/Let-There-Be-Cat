using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D controller;
    private float horizontalMovement = 0f;
    public float horizontalSpeed = 30f;
    private Animator animator;

    private bool jump = false;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal") * horizontalSpeed;

        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
        }

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMovement * Time.fixedDeltaTime, jump);
        jump = false;
    }
}

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

    private float lastActive = 0f;
    public float idleCooldown = 5f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        lastActive = Time.time;
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
        }

        if (horizontalMovement != 0 || jump)
        {
            lastActive = Time.time;
        }

        if (Time.time - lastActive > idleCooldown)
        {
            lastActive = Time.time; 
            if (animator != null)
            {
                animator.SetInteger("IdleRandomSeed", Random.Range(0, 3));
                animator.SetTrigger("IdleMore");
            }
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMovement * Time.fixedDeltaTime, jump);
        jump = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : InteractableProp
{
    public Animator animator;
    public AnimatorOverrideController animOverride;
    private bool isAttached = false;
    private bool isBroken = false;
    private bool isStopped = false;
    public Transform attachTransform;

    public float initialAngularSpeed = 180f;
    private float angularSpeed = 0f;
    public float angularAccelerationRatio = 6f;
    public float angularSpeedDamping = 10f;
    public float detachForceRatio = 500f;
    public GameObject playerGO;

    public ParticleSystem sparkParticle;

    protected override void Start()
    {
        base.Start();
        if (animator != null && animOverride != null)
        {
            animator.runtimeAnimatorController = animOverride;
        }

        playerGO = PlayerManager.instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact()
    {
        base.Interact();
        isAttached = false;
        isBroken = true;
        PlayerManager.instance.gameObject.transform.parent = null;
        if (animator != null)
        {
            animator.SetTrigger("Break");
        }
        addScore();
        CharacterController2D cc2d = playerGO.GetComponent<CharacterController2D>();
        cc2d.disableControlFor(1);
        PlayerMovement pm = playerGO.GetComponent<PlayerMovement>();
        pm.enabled = true;
        playerGO.transform.rotation = Quaternion.identity;
        Rigidbody2D rb = playerGO.GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.AddForce(new Vector3((playerGO.transform.position.x - transform.position.x) * detachForceRatio, 0, 0));
        Debug.Log(playerGO.transform.position.x - transform.position.x);

        Animator catAnimator = playerGO.GetComponent<Animator>();
        catAnimator.SetBool("Hanging", false);
        if (sparkParticle != null)
            sparkParticle.Play();

        AudioManager.instance.PlaySound(2);
    }


    private void FixedUpdate()
    {
        bool shouldRotate = false;
        if (isBroken && !isStopped)
        {
            shouldRotate = true;
        }

        if (isAttached)
        {
            if (playerGO != null && attachTransform != null)
            {
                playerGO.transform.position = attachTransform.position;
                playerGO.transform.rotation = attachTransform.rotation;
            }

            shouldRotate = true;
        }

        if (shouldRotate)
        {
            float currentRotation = transform.rotation.eulerAngles.z;
            int direction = (currentRotation > 0 && currentRotation < 180) ? -1 : 1;
            float rotationOffset = Mathf.Abs(Mathf.Abs(currentRotation - 180) - 180);
            float angularAcceleration = angularAccelerationRatio * rotationOffset;
            angularSpeed += direction * angularAcceleration * Time.fixedDeltaTime;
            if (isBroken)
            {
                int dampDirection = angularSpeed > 0 ? -1 : 1;
                angularSpeed += dampDirection * Time.fixedDeltaTime * angularSpeedDamping;

                if (Mathf.Abs(angularSpeed) < 0.1 && rotationOffset < 10)
                    isStopped = true;
            }
            transform.Rotate(0, 0, angularSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBroken || isAttached) return;

        if (collision.gameObject.layer == 3) // if touched player
        {
            isAttached = true;

            Rigidbody2D rb = playerGO.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            float direction = rb.velocity.x < 0 ? -1 : 1;

            angularSpeed = direction * initialAngularSpeed;

            PlayerMovement pm = playerGO.GetComponent<PlayerMovement>();
            pm.enabled = false;

            Animator catAnimator = playerGO.GetComponent<Animator>();
            catAnimator.SetBool("Hanging", true);

        }
    }
}

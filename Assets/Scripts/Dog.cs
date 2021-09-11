using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Dog : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 spawnPosition;
    private bool reachedOtherEnd = false;
    private bool reachedBone = false;
    private bool isEscaping = false;
    public float speed = 10f;
    public Animator animator;
    public SpriteRenderer sp;
    public AnimatorOverrideController boneOverride;
    public DogBowl bowl;

    public bool shouldTransferCam = false;
    private bool transferredCam = false;

    public CinemachineVirtualCamera cinemachine;
    //public 
    void Start()
    {
        spawnPosition = transform.position;
        if (shouldTransferCam)
        {
            cinemachine = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
            cinemachine.Follow = transform;
            cinemachine.LookAt = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!reachedOtherEnd)
        {
            transform.Translate(new Vector3(speed * Time.fixedDeltaTime, 0));

            if (transform.position.x - spawnPosition.x > 40)
                reachedOtherEnd = true;
            animator.SetBool("Running", true);

            if (shouldTransferCam && !transferredCam && transform.position.x - spawnPosition.x > 20)
            {
                PlayerManager.instance.setInvincible(false);
                transferredCam = true;
                cinemachine.Follow = PlayerManager.instance.transform;
                cinemachine.LookAt = PlayerManager.instance.transform;
            }
        } else if (!reachedBone)
        {
            transform.Translate(new Vector3(-speed * Time.fixedDeltaTime, 0));
            animator.SetBool("Running", true);
            sp.flipX = true;

            if (transform.position.x - spawnPosition.x < 21)
            {
                animator.SetBool("Running", false);
                reachedBone = true;
                StartCoroutine(waitForBone());
            }
        } else if (isEscaping)
        {
            transform.Translate(new Vector3(-speed * Time.fixedDeltaTime, 0));
            animator.SetBool("Running", true);
            sp.flipX = true;
            if (transform.position.x < spawnPosition.x)
            {
                isEscaping = false;
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator waitForBone()
    {
        yield return new WaitForSeconds(1f);
        // empty bowl
        bowl.takeBone();
        animator.runtimeAnimatorController = boneOverride;
        AudioManager.instance.PlaySound(16);
        StartCoroutine(waitToEscape());
    }

    public IEnumerator waitToEscape()
    {
        yield return new WaitForSeconds(1f);
        isEscaping = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == 10) // if touched human
        {
            // slip human
            Human human = collision.gameObject.GetComponent<Human>();
            if (human != null && human.TrySlip())
            {
                PlayerManager.instance.addScore(3);
            }
        }
    }
}

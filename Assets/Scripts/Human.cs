using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform playerTransform;

    public SpriteRenderer spriteRenderer;

    public float grabRange = 1f;
    public float pursueRange = 6f;
    public float lostTrackRange = 8f;
    public float pursueSpeed = 1f;
    private bool pursuing = false;
    private bool grabbing = false;
    public float grabDelay = 0.5f;
    public float postGrabDelay = 0.5f;

    public bool isSlip = false;
    public float slipRecoverTime = 3f;

    public float speedRandomRange = 0.3f;

    public bool grabAllHeight = false;
    public bool isRanged = false;

    public GameObject netPrefab;

    public GameObject questionMark;
    public GameObject exclamationMark;

    private Animator animator;
    public AnimatorOverrideController animOverride;

    public int grabSoundIndex = -1;

    void Start()
    {
        playerTransform = PlayerManager.instance.gameObject.transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator != null && animOverride != null)
        {
            animator.runtimeAnimatorController = animOverride;
        }

        pursueSpeed += Random.Range(-speedRandomRange, speedRandomRange);
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator playGrabAnimation()
    {
        yield return new WaitForSeconds(grabDelay);
        float catHeightDiff = playerTransform.position.y - transform.position.y;
        if (catHeightDiff < -0.5)
            animator.SetInteger("GrabHeight", 0);
        else if (catHeightDiff > 0.5)
            animator.SetInteger("GrabHeight", 2);
        else
            animator.SetInteger("GrabHeight", 1);
        animator.SetTrigger("Grab");

        if (grabSoundIndex != -1)
        {
            AudioManager.instance.PlaySound(grabSoundIndex);
        }

        if (isRanged)
        {
            StartCoroutine(shootNet());
            StartCoroutine(grabDone(false));
        }
        else
        {
            StartCoroutine(grabDone(true));
        }
    }

    IEnumerator shootNet()
    {
        yield return new WaitForSeconds(0.5f);

        if (netPrefab != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            GameObject netGO = Instantiate(netPrefab, transform.position + direction, transform.rotation);
            Net netScript = netGO.GetComponent<Net>();

            netScript.Fire(new Vector2(direction.x, direction.y));
        }
    }

    IEnumerator grabDone(bool grabMelee)
    {
        yield return new WaitForSeconds(postGrabDelay);
        grabbing = false;

        if (grabMelee)
        {
            //float direction = (playerTransform.position.x - transform.position.x > 0) ? 1 : -1;
            float horizontalDistance = playerTransform.position.x - transform.position.x;

            // horizontal grab direction check
            if ((!spriteRenderer.flipX && horizontalDistance > 0) || (spriteRenderer.flipX && horizontalDistance < 0))
            {
                // horizontal grab range check
                if (Mathf.Abs(horizontalDistance) <= grabRange)
                {
                    float verticalDistance = playerTransform.position.y - transform.position.y;

                    if (grabAllHeight)
                    {
                        if (verticalDistance < 1.5)
                        {
                            PlayerManager.instance.Damage();
                        }
                    }
                    else
                    {
                        int grabHeight = animator.GetInteger("GrabHeight");
                        if ((verticalDistance < -0.5 && grabHeight == 0)  // cat is on ground and human grabbing low
                            || (verticalDistance < 0.5 && grabHeight == 1) // cat is mid air and human grabbing mid 
                            || (verticalDistance < 1.5 && grabHeight == 2)) // cat is high up but still within reach and human grabbing high
                        {
                            PlayerManager.instance.Damage();
                        }
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            if (grabbing || isSlip)
                return;
            float distance = Mathf.Abs(playerTransform.position.x - transform.position.x);
            float direction = (playerTransform.position.x - transform.position.x > 0) ? 1 : -1;

            if (distance < grabRange * 0.9 && (!isRanged || distance > 1))
            {
                StartCoroutine(playGrabAnimation());
                grabbing = true;
            } else if (!pursuing && distance < pursueRange)
            {
                exclamationMark.SetActive(true);
                StartCoroutine(hideMark(exclamationMark));
                pursuing = true;
            } else if (pursuing && distance > lostTrackRange)
            {
                questionMark.SetActive(true);
                StartCoroutine(hideMark(questionMark));
                pursuing = false;
            }
            if (grabbing)
            {
                animator.SetBool("Walking", false);
                spriteRenderer.flipX = direction < 0;
            }
            else if (pursuing)
            {
                transform.Translate(direction * pursueSpeed * Time.fixedDeltaTime, 0, 0);
                spriteRenderer.flipX = direction < 0;
                if (animator != null)
                    animator.SetBool("Walking", true);
            } 
            else 
            {
                // TODO Idle walk around
                if (animator != null)
                    animator.SetBool("Walking", false);
            }
        }
    }
    IEnumerator hideMark(GameObject mark)
    {
        yield return new WaitForSeconds(1);
        if (mark != null)
            mark.SetActive(false);
    }

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 cubePosition = transform.position + (spriteRenderer.flipX ? -1 : 1) * new Vector3(grabRange / 2, 0, 0);
        Vector3 cubeSize = new Vector3(grabRange, 3, 0);
        Gizmos.DrawWireCube(cubePosition, cubeSize);
    }*/

    public bool TrySlip()
    {
        if (isSlip) // can't slip if already slipped
            return false;

        isSlip = true;
        animator.SetBool("Fall", true);
        StartCoroutine(RecoverFromSlip());

        AudioManager.instance.PlaySound(12);

        return true;
    }
    IEnumerator RecoverFromSlip()
    {
        yield return new WaitForSeconds(slipRecoverTime);
        animator.SetBool("Fall", false);
        isSlip = false;
    }
}

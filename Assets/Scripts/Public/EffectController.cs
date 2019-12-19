using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    private Animator animator;
    private Damager damager;
    public AnimationClip animationClip = null;
    public bool destoryAfterLooped = true;
    public float duration = 4f;
    public float hitStopDelyTime = 0f;
    public Vector3 velocity = Vector3.zero;

    [HideInInspector]
    public Animator parentAnimator = null;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        damager = GetComponent<Damager>();
        Play();

        if (destoryAfterLooped)
        {
            duration = animationClip.length;
        }
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        if (velocity != Vector3.zero)
        {
            transform.Translate(velocity * Time.deltaTime);
        }
    }

    void OnDestroy()
    {
        if (parentAnimator)
        {
            parentAnimator.enabled = true;
            parentAnimator.speed = 1f;
        }
    }

    public void Play()
    {
        if (animator != null && animationClip != null)
        {
            animator.Play(animationClip.name);
        }
    }

    // HitStop process
    private bool hitStopFinished = true;
    public void HitStop(float useTime)
    {
        if (hitStopFinished)
        {
            //Debug.Log("HitStopStart");
            StartCoroutine(HitStopCoroutine(useTime, hitStopDelyTime));
        }
    }
    private IEnumerator HitStopCoroutine(float useTime, float delayTime)
    {
        hitStopFinished = false;
        if (delayTime > 0)
        {
            yield return new WaitForSeconds(delayTime);
        }
        damager.DisableDamage();
        //animator.enabled = false;
        //parentAnimator.enabled = false;
        animator.speed = 0;
        parentAnimator.speed = 0;
        yield return new WaitForSeconds(useTime);

        //Debug.Log("HitStopFinished");
        damager.EnableDamage();
        //animator.enabled = true;
        //parentAnimator.enabled = true;
        animator.speed = 1f;
        parentAnimator.speed = 1f;
        hitStopFinished = true;
    }
}

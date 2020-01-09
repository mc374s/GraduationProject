﻿using System.Collections;
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
    public Vector3 startvelocity = Vector3.zero;
    public Vector3 acceleration = Vector3.zero;
    public Vector3 maxVelocity = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    public float maxMoveDistance = 500;

    private Vector3 startPosition = Vector3.zero;
    private float minDistanceFromParent = 6;

    [HideInInspector]
    public Animator parentAnimator = null;

    protected readonly string waitStateName = "Waiting";
    protected readonly int hashNext = Animator.StringToHash("next");
    private int baseLayerIndex = 0;


    private bool destoryFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        damager = GetComponent<Damager>();
        AudioSource audioSource = GetComponent<AudioSource>();
        if (destoryAfterLooped)
        {
            duration = animationClip.length;
        }
        if (audioSource)
        {
            duration += audioSource.clip.length;
        }


        //Destroy(gameObject, duration);
        StartCoroutine(Utility.DelayCoroutine(PlayNext, duration));

        baseLayerIndex = animator.GetLayerIndex("Base Layer");

        startPosition = transform.position;
        Play();

        velocity = startvelocity;
    }

    // Update is called once per frame
    void Update()
    {
        velocity += acceleration;
        velocity = Vector3.Min(velocity, maxVelocity);
        if (velocity != Vector3.zero)
        {
            transform.Translate(velocity * Time.deltaTime * animator.speed);
            //Debug.Log(velocity * Time.deltaTime * animator.speed);
            if ((transform.position - startPosition).sqrMagnitude > maxMoveDistance * maxMoveDistance)
            {
                velocity = Vector3.zero;
            }
        }
        if (destoryFlag)
        {
            AnimatorStateInfo curruntAnimatorState = animator.GetCurrentAnimatorStateInfo(baseLayerIndex);
            if (curruntAnimatorState.IsName(waitStateName)/* || curruntAnimatorState.tagHash == hashLast*/)
            {
                Destroy(gameObject);
                destoryFlag = false;
            }
        }
    }

    //private IEnumerator Delay(float useTime, float delayTime)

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

        if (parentAnimator)
        {
            parentAnimator.speed = 0;
        }
        //if ((parentAnimator.transform.position - transform.position).sqrMagnitude < minDistanceFromParent * minDistanceFromParent)
        //{
        //    parentAnimator.speed = 0;
        //}
        yield return new WaitForSeconds(useTime);

        //Debug.Log("HitStopFinished");
        damager.EnableDamage();
        //animator.enabled = true;
        //parentAnimator.enabled = true;
        animator.speed = 1f;
        if (parentAnimator)
        {
            parentAnimator.speed = 1f;
        }
        hitStopFinished = true;
    }

    public void PlayNext()
    {
        if (animator != null && animationClip != null)
        {
            animator.SetTrigger(hashNext);
            destoryFlag = true;
        }
    }
}

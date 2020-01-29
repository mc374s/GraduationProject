using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSMB : StateMachineBehaviour
{
    private CharacterController2D characterController = null;

    //public bool hitStop
    public GameObject attackEffect = null;
    public bool effectAsChild = false;

    public GameObject montionEffect = null;
    public bool effectSetCenter = false;

    public Vector3 effectOffset = Vector3.zero;

    public bool updateCustomFunc = true;

    GameObject effectClone = null;
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (characterController == null)
        {
            characterController = animator.gameObject.GetComponentInParent<CharacterController2D>();
        }
        characterController.ResetMoveVector();


        Transform targetPoint = characterController.montionRightPoint;
        if (effectSetCenter)
        {
            targetPoint = characterController.transform;
        }
        if (attackEffect)
        {
            if (effectAsChild)
            {
                effectClone = Instantiate(attackEffect, targetPoint);
                if (effectClone.GetComponent<EffectController>() != null)
                {
                    effectClone.GetComponent<EffectController>().parentAnimator = animator;
                }
            }
            else
            {
                effectClone = Instantiate(attackEffect, characterController.rightPoint.position, characterController.rightPoint.rotation);
            }
            effectClone.transform.position += effectOffset;
        }
        if (montionEffect)
        {
            GameObject effectClone = Instantiate(montionEffect, targetPoint);
            if (effectClone.GetComponent<EffectController>() != null)
            {
                effectClone.GetComponent<EffectController>().parentAnimator = animator;
            }
            effectClone.transform.position += effectOffset;
        }
        //effectClone.GetComponent<Damager>().OnDamageableHit = characterController.OnDamagerDamageableHit;
        //if (!characterController.IsFacingLeft)
        //{
        //    effectClone.GetComponent<SpriteRenderer>().flipX = false;
        //}

    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateCustomFunc)
        {
            characterController.Attack();
        }
    }

    //OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (effectAsChild && effectClone != null)
        {
            Destroy(effectClone);
        }
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    playerController.ResetMoveVector();
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}

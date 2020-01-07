using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSMB : StateMachineBehaviour
{
    //public bool hitStop
    public GameObject attackEffect;
    public bool effectAsChild = false;

    private CharacterController2D characterController = null;
    GameObject effectClone = null;
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (characterController == null)
        {
            characterController = animator.gameObject.GetComponentInParent<CharacterController2D>();
        }
        characterController.ResetMoveVector();

        if (effectAsChild)
        {
            effectClone = Instantiate(attackEffect, characterController.montionRightPoint);
            if (effectClone.GetComponent<EffectController>() != null)
            {
                effectClone.GetComponent<EffectController>().parentAnimator = animator;
            }
        }
        else
        {
            effectClone = Instantiate(attackEffect, characterController.rightPoint.position, characterController.rightPoint.rotation);
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
        characterController.Attack();
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

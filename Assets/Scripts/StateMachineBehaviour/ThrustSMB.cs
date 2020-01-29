using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustSMB : StateMachineBehaviour
{
    public GameObject attackEffect;
    public bool effectAsChild = false;
    private CharacterController2D characterController = null;
    GameObject effectClone = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (characterController == null)
        {
            characterController = animator.gameObject.GetComponentInParent<CharacterController2D>();
        }
        characterController.ResetMoveVector();

        if (attackEffect)
        {
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
        }
        characterController.InvulnerableOn();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterController.ThrustUpdate();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

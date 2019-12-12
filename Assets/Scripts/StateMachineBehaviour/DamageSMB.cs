using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSMB : StateMachineBehaviour
{
    public GameObject hitEffect;
    private CharacterController2D characterController = null;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (characterController == null)
        {
            characterController = animator.gameObject.GetComponentInParent<CharacterController2D>();
        }
        characterController.ResetMoveVector();

        GameObject effectClone = Instantiate(hitEffect, characterController.transform.position + new Vector3(Random.Range(-2, 2), Random.Range(2, 8), 0), Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
        if (characterController.IsFacingLeft)
        {
            effectClone.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterController.input.Release();
        characterController.DamageUpdate();
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

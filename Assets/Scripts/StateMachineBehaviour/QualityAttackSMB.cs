using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityAttackSMB : StateMachineBehaviour
{
    public bool KilledMontion = false;
    public GameObject attackEffect;
    public bool effectAsChild = true;
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
        }
        else
        {
            effectClone = Instantiate(attackEffect, characterController.montionRightPoint.position, characterController.montionRightPoint.rotation);
        }
        effectClone.GetComponent<Damager>().targetObject = characterController.focusedObject;
        //effectClone.GetComponent<Damager>().OnDamageableHit = characterController.OnDamagerDamageableHit;
        if (effectClone.GetComponent<EffectController>() != null)
        {
            effectClone.GetComponent<EffectController>().parentAnimator = animator;
        }
        if (characterController.IsFacingLeft)
        {
            effectClone.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (KilledMontion)
        {
            CameraController.Instance.ZoomTo(-50, characterController.focusedObject.transform.position, 0.3f, 0f);
            //Time.timeScale = 0.2f;
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterController.QualityAttackUpdate();
        //Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (KilledMontion)
        {
            characterController.QualityAttackFinish();
            CameraController.Instance.ResetZoom(0.2f, 1f);
            CameraController.Instance.InduceStress(1);
        }
        if (effectAsChild && effectClone != null)
        {
            Destroy(effectClone);
        }
    }

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

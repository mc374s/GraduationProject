using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEnemyController : EnemyController
{

    public readonly int hashAttacking = Animator.StringToHash("attacking");
    public readonly int hashUsingSkill = Animator.StringToHash("usingSkill");
    public readonly int hashSkillType = Animator.StringToHash("skillType");
    public readonly int hashAction = Animator.StringToHash("action");
    public readonly int hashNext = Animator.StringToHash("next");

    [SerializeField]
    protected float verticalSpeed = 50;

    public override void VerticalMovment()
    {
        //base.VerticalMovment();
        moveVector.y = input.Vertical * verticalSpeed;
    }

    public override void Attack()
    {
        if (animator.GetInteger(hashSkillType) == 3)
        {
            DropAttackUpdate();
            return;
        }
        if (input.Skill.Down)
        {
            animator.SetTrigger(hashUsingSkill);
            if (input.Vertical > 0)
            {
                animator.SetInteger(hashSkillType, 3);
            }
            else if (input.Vertical < 0)
            {
                animator.SetInteger(hashSkillType, 2);
            }
            else
            {
                animator.SetInteger(hashSkillType, 1);
            }
        }
    }

    public void DropAttackUpdate()
    {
        // DropAttackStart()
        if (jumpCounter < jumpCounterMax)
        {
            ++jumpCounter;
            moveVector.y = 0;
            moveVector.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }
        if (character2D.IsGrounded)
        {
            jumpCounter = 0;
            animator.SetInteger(hashSkillType, 0);
        }
        else
        {
            if (character2D.Velocity.y > -50)
            {
                HorizatalMovment();
            }
            else
            {
                moveVector.x = 0;
            }
            base.VerticalMovment();
        }
    }

}

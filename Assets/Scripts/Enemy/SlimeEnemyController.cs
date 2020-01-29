﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemyController : EnemyController
{
    protected readonly int hashAttack = Animator.StringToHash("attack");
    public override void Attack()
    {
        if (input.Attack.Down)
        {
            ResetMoveVector();
            animator.SetTrigger(hashAttack);
        }
    }
}

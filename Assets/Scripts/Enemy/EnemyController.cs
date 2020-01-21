using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemyController : CharacterController2D
{
    protected readonly int hashHorizontalSpeed = Animator.StringToHash("horizontalSpeed");
    protected readonly int hashVerticalSpeed = Animator.StringToHash("verticalSpeed");
    protected readonly int hashGrounded = Animator.StringToHash("grounded");
    protected readonly int hashCeilinged = Animator.StringToHash("ceilinged");
    protected readonly int hashAttack = Animator.StringToHash("attack");
    protected readonly int hashUsingSkill = Animator.StringToHash("usingSkill");
    protected readonly int hashSkillType = Animator.StringToHash("skillType");
    protected readonly int hashAction = Animator.StringToHash("action");
    protected readonly int hashHurt = Animator.StringToHash("hurt");
    protected readonly int hashKnockDown = Animator.StringToHash("knockDown");
    protected readonly int hashDead = Animator.StringToHash("dead");
    protected readonly int hashAttacking = Animator.StringToHash("attacking");

    public Damageable damageable;
    public LayerMask layerWhenKnockDown;
    private int initLayer;
    private int newLayer;

    
    [SerializeField]
    protected float verticalSpeed = 30;

    // Start is called before the first frame update
    void Start()
    {
        character2D = GetComponent<Character2D>();
        initLayer = gameObject.layer;
        newLayer = Mathf.RoundToInt(Mathf.Log(layerWhenKnockDown.value, 2));
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //input.Release();
    //}
    private void LateUpdate()
    {
        input.Release();
    }

    private void FixedUpdate()
    {
        character2D.Move(moveVector * Time.fixedDeltaTime);

        animator.SetFloat(hashHorizontalSpeed, character2D.Velocity.x);
        animator.SetFloat(hashVerticalSpeed, character2D.Velocity.y);
        animator.SetBool(hashGrounded, character2D.IsGrounded);
    }

    public override void HorizatalMovment()
    {
        base.HorizatalMovment();
        moveVector.y = input.Vertical * verticalSpeed;
    }

    public override void Jump()
    {
        if (character2D.IsGrounded)
        {
            jumpCounter = 0;
        }
        if (input.Jump.Down && ++jumpCounter < jumpCounterMax)
        {
            moveVector.y = 0;
            moveVector.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }
    }

    public override void Attack()
    {
        if (input.Attack.Down)
        {
            ResetMoveVector();
            animator.SetTrigger(hashAttack);
        }
        if (input.Skill.Down)
        {
            animator.SetTrigger(hashUsingSkill);
            if (input.Horizontal != 0)
            {
                animator.SetInteger(hashSkillType, 2);
            }
            else if (input.Vertical > 0)
            {
                animator.SetInteger(hashSkillType, 3);
            }
            else
            {
                animator.SetInteger(hashSkillType, 1);
            }
        }
    }

    protected readonly string hurtStateName = "Hurt";
    public override void OnHurt()
    {
        //animator.SetTrigger(hashHurt);
        animator.Play(hurtStateName, 1);
        //if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        //{
        //}
    }
    private Damager damagerRecord;
    public void OnHurt(Damager damager,Damageable damageable)
    {
        if (/*damagerRecord != damager*/true)
        {
            animator.SetTrigger(hashHurt);
            damagerRecord = damager;
            if (gameObject.layer == newLayer)
            {
                damageable.IsKnockDown = true;
                animator.SetBool(hashKnockDown, true);
            }
        }
    }

    public override void OnKnockDown(/*Damager damager, Damageable damageable*/)
    {
        animator.SetBool(hashKnockDown, true);
        gameObject.layer = newLayer;
    }

    public override void OnDie()
    {
        animator.SetTrigger(hashDead);
        gameObject.layer = initLayer;
        Destroy(gameObject, 2);
    }

    public override void DamageUpdate()
    {
        if (!damageable.IsKnockDown)
        {
            animator.SetBool(hashKnockDown, false);
            gameObject.layer = initLayer;
        }
    }

    public void QualityAttackedStart()
    {
        damageable.KnockDownWait = true;
        damageable.IsKnockDownFinish = false;
    }
    public void QualityAttackedFinish()
    {
        damageable.IsKnockDownFinish = true;
    }

#if UNITY_EDITOR
    //float height;
    //void OnEnable()
    //{
    //    height = Global.debugUIStartY;
    //    Global.debugUIStartY += 20;
    //}
    //void OnGUI()
    //{
    //    if (Global.isDebugMenuOpen)
    //    {
    //        GUILayout.BeginArea(new Rect(Global.debugUIStartX, height, 200, 100));
    //        GUILayout.Label("Enemy MoveVector: " + moveVector.ToString());

    //        GUILayout.EndArea();

    //    }
    //}

#endif
}

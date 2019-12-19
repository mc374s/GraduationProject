using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController2D
{
    [SerializeField]
    private float jumpHoldIncrement = 2;
    [SerializeField]
    private float jumpTime = 0.35f;
    private float jumpTimer = 0;
    private bool isJumping = false;

    [SerializeField]
    private float dodgeRollSpeed = 10f;
    private float dodgeRollVelocityX = 0;

    protected readonly int hashHorizontalSpeed = Animator.StringToHash("horizontalSpeed");
    protected readonly int hashVerticalSpeed = Animator.StringToHash("verticalSpeed");
    protected readonly int hashGrounded = Animator.StringToHash("grounded");
    protected readonly int hashCeilinged = Animator.StringToHash("ceilinged");
    protected readonly int hashAttack = Animator.StringToHash("attack");
    protected readonly int hashUsingSkill = Animator.StringToHash("usingSkill");
    protected readonly int hashSkillType = Animator.StringToHash("skillType");
    protected readonly int hashAction = Animator.StringToHash("action");
    protected readonly int hashDodgeRoll = Animator.StringToHash("roll");
    protected readonly int hashHurt = Animator.StringToHash("hurt");
    protected readonly int hashKnockDown = Animator.StringToHash("knockDown");
    protected readonly int hashDead = Animator.StringToHash("dead");
    protected readonly int hashAttacking = Animator.StringToHash("attacking");

    public Damageable damageableReference;

    // Start is called before the first frame update
    void Start()
    {
        character2D = GetComponent<Character2D>();
    }

    // Update is called once per frame
    void Update()
    {
        input.Gain();
        if (!SceneController.Instance.SceneFiledCheck(gameObject))
        {
            OnDie();
        }
    }

    private void FixedUpdate()
    {
        character2D.Move(moveVector * Time.fixedDeltaTime);

        animator.SetFloat(hashHorizontalSpeed, character2D.Velocity.x);
        animator.SetFloat(hashVerticalSpeed, character2D.Velocity.y);
        animator.SetBool(hashGrounded, character2D.IsGrounded);
        animator.SetBool(hashCeilinged, character2D.IsCeilinged);
    }

    public override void Jump()
    {
        if (character2D.IsGrounded)
        {
            jumpCounter = 0;
        }
        if (input.Jump.Down && jumpCounter < jumpCounterMax)
        {
            ++jumpCounter;
            isJumping = true;
            jumpTimer = jumpTime;
            moveVector.y = 0;
            moveVector.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }
    }
    public override void JumpUpdate()
    {
        if (input.Jump.Held && isJumping)
        {
            if (jumpTimer > 0)
            {
                moveVector.y += jumpHoldIncrement * Time.deltaTime;
                jumpTimer -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (input.Jump.Up)
        {
            jumpTimer = 0;
            isJumping = false;
        }
        //if (character2D.IsCeilinged)
        //{
        //    moveVector.y = 0;
        //}
    }

    public override void DodgeRoll()
    {
        if (input.Roll.Down)
        {
            dodgeRollVelocityX = character2D.spriteFaceLeft ? -dodgeRollSpeed : dodgeRollSpeed;
            animator.SetTrigger(hashDodgeRoll);
        }
    }
    public override void DodgeRollUpdate()
    {
        moveVector.x = dodgeRollVelocityX;
        if (moveVector.y < 0)
        {
            InvulnerableOff();
        }
        if (Global.isBattling)
        {
            //Debug.Log("Px: " + transform.position.x + ", BLx: " + Global.borderLeft + ", BRx: " + Global.borderRight);
            if (transform.position.x < Global.borderLeft + 1)
            {
                moveVector.x = 0;
                InvulnerableOff();
                transform.position = new Vector3(Global.borderLeft + 1, transform.position.y, transform.position.z);
            }
            if (transform.position.x > Global.borderRight - 1)
            {
                moveVector.x = 0;
                InvulnerableOff();
                transform.position = new Vector3(Global.borderRight - 1, transform.position.y, transform.position.z);
            }
        }
    }
    public override void InvulnerableOn()
    {
        GetComponent<Collider2D>().enabled = false;
        //damageableReference.EnableInvulnerability();
    }
    public override void InvulnerableOff()
    {
        GetComponent<Collider2D>().enabled = true;
        //damageableReference.in;
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
            if (input.Horizontal > 0)
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

    public void QualityAttackDetection(GameObject targetObject)
    {
        if (input.Action.Down && !animator.GetBool(hashAttacking))
        {
            focusedObject = targetObject;

            if (focusedObject == null)
            {
                return;
            }
            focusedObject.GetComponent<EnemyController>().QualityAttackedStart();
            Debug.Log("QuilityAttackDetected: " + focusedObject.name);
            ResetMoveVector();
            animator.SetTrigger(hashAction);
            animator.SetBool(hashAttacking, true);
            animator.SetInteger(hashSkillType, Random.Range(1, 4));
        }
    }

    public override void QualityAttackUpdate()
    {
        //Debug.Log("QualityAttackUpdate: " + animator.GetInteger(hashSkillType));
    }
    public override void QualityAttackFinish()
    {
        animator.SetBool(hashAttacking, false);
        focusedObject.GetComponent<EnemyController>().QualityAttackedFinish();

        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        Debug.Log("QuilityAttackFinish: " + focusedObject.name);
    }


    public override void OnHurt()
    {
        animator.SetTrigger(hashHurt);
    }
    private Damager damagerRecord;
    public void OnHurt(Damager damager, Damageable damageable)
    {
        if (damagerRecord != damager)
        {
            animator.SetTrigger(hashHurt);
            damagerRecord = damager;
        }
    }
    public override void OnKnockDown(/*Damager damager, Damageable damageable*/)
    {
        animator.SetBool(hashKnockDown, true);
    }

    public override void OnDie()
    {
        animator.SetTrigger(hashDead);
        //Destroy(gameObject, 2);
        SceneController.Instance.ReloadCurrentScene(1);
    }

    public override void DamageUpdate()
    {
        if (!damageableReference.IsKnockDown)
        {
            animator.SetBool(hashKnockDown, false);
        }
    }

    public override void Respawn()
    {

    }

    public override void OnAttackHit()
    {
        
    }

#if UNITY_EDITOR

    float height;
    void OnEnable()
    {
        height = Global.debugUIStartY;
        Global.debugUIStartY += 20;
    }
    void OnGUI()
    {
        if (Global.isDebugMenuOpen)
        {
            GUILayout.BeginArea(new Rect(Global.debugUIStartX, height, 200, 100));
            GUILayout.Label("MoveVector: " + moveVector.ToString());

            GUILayout.EndArea();

        }
    }
    //private void OnDrawGizmosSelected()
    //{
    //    //Handles.color = new Color(0, 1.0f, 0, 0.2f);
    //    //Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov, viewDistance);

    //}

#endif
}

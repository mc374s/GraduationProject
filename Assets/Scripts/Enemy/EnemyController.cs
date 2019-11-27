using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemyController : CharacterController2D
{
    protected readonly int hashHorizontalSpeed = Animator.StringToHash("horizontalSpeed");
    protected readonly int hashGrounded = Animator.StringToHash("grounded");
    protected readonly int hashAttack = Animator.StringToHash("attack");
    protected readonly int hashHurt = Animator.StringToHash("hurt");
    protected readonly int hashKnockDown = Animator.StringToHash("knockDown");
    protected readonly int hashDead = Animator.StringToHash("dead");

    public Damageable damageable;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        character2D = GetComponent<Character2D>();
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
        //animator.SetFloat(hashVerticalSpeed, character2D.Velocity.y);
        animator.SetBool(hashGrounded, character2D.IsGrounded);
    }


    public override void HorizatalMovment()
    {
        moveVector.x = input.Horizontal * horzontalSpeed;
    }
    public override void FacingUpdate()
    {
        if ((moveVector.x < 0 && !character2D.spriteFaceLeft || moveVector.x > 0 && character2D.spriteFaceLeft))
        {
            character2D.Flip();
        }
    }

    public override void VerticalMovment()
    {
        moveVector.y += gravity * Time.deltaTime;
        if (character2D.IsGrounded && moveVector.y < 0)
        {
            moveVector.y = 0;
        }
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

    public override void ResetMoveVector()
    {
        moveVector = Vector2.zero;
    }

    public override void Attack()
    {
        if (input.Attack.Down)
        {
            ResetMoveVector();
            animator.SetTrigger(hashAttack);
        }
    }
    public override void OnHurt()
    {
        animator.SetTrigger(hashHurt);
    }
    private Damager damagerRecord;
    public void OnHurt(Damager damager,Damageable damageable)
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
        Destroy(gameObject, 2);
    }

    public override void DamageUpdate()
    {
        if (!damageable.IsKnockDown)
        {
            animator.SetBool(hashKnockDown, false);
        }
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
            GUILayout.Label("Enemy MoveVector: " + moveVector.ToString());

            GUILayout.EndArea();

        }
    }
    private void OnDrawGizmosSelected()
    {
        //bool spriteFaceLeft = true;
        //float viewFov = 300;
        //float viewDirection = 0;
        //float viewDistance = 10;
        ////draw the cone of view
        //Vector3 forward = character2D.spriteFaceLeft ? Vector2.left : Vector2.right;
        //forward = Quaternion.Euler(0, 0, character2D.spriteFaceLeft ? -viewDirection : viewDirection) * forward;

        //if (GetComponent<SpriteRenderer>().flipX) forward.x = -forward.x;

        //Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);

        //Handles.color = new Color(0, 1.0f, 0, 0.2f);
        //Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov, viewDistance);

        ////Draw attack range
        ////Handles.color = new Color(1.0f, 0, 0, 0.1f);
        ////Handles.DrawSolidDisc(transform.position, Vector3.back, meleeRange);
    }

#endif
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemyController : CharacterController2D
{
    public readonly int hashHorizontalSpeed = Animator.StringToHash("horizontalSpeed");
    public readonly int hashVerticalSpeed = Animator.StringToHash("verticalSpeed");
    public readonly int hashGrounded = Animator.StringToHash("grounded");
    public readonly int hashCeilinged = Animator.StringToHash("ceilinged");
    public readonly int hashHurt = Animator.StringToHash("hurt");
    public readonly int hashKnockDown = Animator.StringToHash("knockDown");
    public readonly int hashDead = Animator.StringToHash("dead");

    public Damageable damageable;
    public LayerMask layerWhenKnockDown;
    protected int initLayer;
    protected int newLayer;

    // Start is called before the first frame update
    protected virtual void Start()
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
    protected virtual void LateUpdate()
    {
        input.Release();
    }

    protected virtual void FixedUpdate()
    {
        character2D.Move(moveVector * Time.fixedDeltaTime);

        animator.SetFloat(hashHorizontalSpeed, character2D.Velocity.x);
        animator.SetFloat(hashVerticalSpeed, character2D.Velocity.y);
        animator.SetBool(hashGrounded, character2D.IsGrounded);
    }

    protected readonly string hurtStateName = "Hurt";
    public override void OnHurt()
    {
        //animator.SetTrigger(hashHurt);
        animator.Play(hurtStateName, 1);
    }
    protected Damager damagerRecord;
    public virtual void OnHurt(Damager damager,Damageable damageable)
    {
        if (/*damagerRecord != damager*/true)
        {
            if(damager.tag == "normal")
            {
                animator.SetTrigger(hashHurt);
            }
            else animator.SetTrigger(hashHurtSpecial);
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

    public virtual void QualityAttackedStart()
    {
        damageable.KnockDownWait = true;
        damageable.IsKnockDownFinish = false;
    }
    public virtual void QualityAttackedFinish()
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

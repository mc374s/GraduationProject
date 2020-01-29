using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Character2D))]
public class CharacterController2D : MonoBehaviour
{
    public Animator animator;
    protected Character2D character2D;
    [HideInInspector]
    public WrappedInput input = new WrappedInput();

    protected Vector2 moveVector;
    public Vector2 MoveVector { get { return moveVector; } set { moveVector = value; } }

    [SerializeField]
    protected float gravity = -30;
    public float grivatyScale = 1;
    [SerializeField]
    protected float horzontalSpeed = 10;

    [SerializeField]
    protected float jumpHeight = 20;
    [SerializeField]
    protected int jumpCounterMax = 2;
    protected int jumpCounter = 0;

    public Transform leftPoint;
    public Transform rightPoint;
    public Transform montionRightPoint;
    [HideInInspector]
    public GameObject focusedObject;

    public UnityEvent HitEventBridge;

    public bool IsFacingLeft { get { return character2D.spriteFaceLeft; } }

    void Start()
    {
        Debug.Log("CharacterController2D Start");
    }

    public virtual void HorizatalMovment()
    {
        moveVector.x = input.Horizontal * horzontalSpeed;
    }
    public virtual void VerticalMovment()
    {
        moveVector.y += gravity * grivatyScale * Time.deltaTime;
        if (character2D.IsGrounded && moveVector.y < 0)
        {
            moveVector.y = 0;
        }
        if (character2D.IsCeilinged && moveVector.y > 0)
        {
            moveVector.y = 0;
        }
    }
    public virtual void FacingUpdate()
    {
        if ((moveVector.x < 0 && !character2D.spriteFaceLeft || moveVector.x > 0 && character2D.spriteFaceLeft))
        {
            character2D.Flip();
        }
    }
    public virtual void Jump() { }
    public virtual void JumpUpdate() { }
    public virtual void DodgeRoll() { }
    public virtual void DodgeRollUpdate() { }
    public virtual void ResetMoveVector()
    {
        moveVector = Vector2.zero;
    }
    public virtual void Attack() { }
    public virtual void QualityAttackUpdate() { }
    public virtual void QualityAttackFinish() { }
    public virtual void DamageUpdate() { }
    public virtual void OnHurt() { }
    public virtual void OnKnockDown() { }
    public virtual void OnDie() { }
    public virtual void Respawn() { }
    public virtual void OnAttackHit() { }
    public virtual void InvulnerableOn() { }
    public virtual void InvulnerableOff() { }
    public virtual void Thrust() { }
    public virtual void ThrustUpdate() { }
}

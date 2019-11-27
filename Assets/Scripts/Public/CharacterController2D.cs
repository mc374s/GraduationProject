using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Character2D))]
public class CharacterController2D : MonoBehaviour
{
    protected Animator animator;
    protected Character2D character2D;
    [HideInInspector]
    public WrappedInput input = new WrappedInput();

    protected Vector2 moveVector;
    public Vector2 MoveVector { get { return moveVector; } set { moveVector = value; } }

    [SerializeField]
    protected float gravity = -30;
    [SerializeField]
    protected float horzontalSpeed = 10;

    [SerializeField]
    protected float jumpHeight = 20;
    [SerializeField]
    protected int jumpCounterMax = 2;
    protected int jumpCounter = 0;

    public Transform leftPoint;
    public Transform rightPoint;
    [HideInInspector]
    public Transform attackEffectPoint;
    public bool IsFacingLeft { get { return character2D.spriteFaceLeft; } }

    public virtual void HorizatalMovment() { }
    public virtual void VerticalMovment() { }
    public virtual void FacingUpdate() { }
    public virtual void Jump() { }
    public virtual void JumpUpdate() { }
    public virtual void DodgeRoll() { }
    public virtual void DodgeRollUpdate() { }
    public virtual void ResetMoveVector() { }
    public virtual void Attack() { }
    public virtual void DamageUpdate() { }
    public virtual void OnHurt() { }
    public virtual void OnKnockDown() { }
    public virtual void OnDie() { }
    public virtual void Respawn() { }
    public virtual void OnAttackHit() { }
    public virtual void InvulnerableOn() { }
    public virtual void InvulnerableOff() { }
}

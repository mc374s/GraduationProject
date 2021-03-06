﻿using System;
using UnityEngine;
using UnityEngine.Events;

public class Damager : MonoBehaviour
{
    [Serializable]
    public class DamagableEvent : UnityEvent<Damager, Damageable>
    { }


    [Serializable]
    public class NonDamagableEvent : UnityEvent<Damager>
    { }

    //call that from inside the onDamageableHIt or OnNonDamageableHit to get what was hit.
    public Collider2D LastHit { get { return m_LastHit; } }

    public int damage = 1;
    public int hitTimes = 1;
    public float hitIntervalTime = 0.1f;
    private float hitIntervalTimer = 0;
    public Vector2 offset = new Vector2(1.5f, 1f);
    public Vector2 size = new Vector2(2.5f, 1f);
    [Tooltip("If this is set, the offset x will be changed base on the sprite flipX setting. e.g. Allow to make the damager alway forward in the direction of sprite")]
    public bool offsetBasedOnSpriteFacing = true;
    [Tooltip("SpriteRenderer used to read the flipX value used by offset Based OnSprite Facing")]
    public SpriteRenderer spriteRenderer;
    [Tooltip("If disabled, damager ignore trigger when casting for damage")]
    public bool canHitTriggers;
    public bool disableDamageAfterHit = false;
    [Tooltip("If set, the player will be forced to respawn to latest checkpoint in addition to loosing life")]
    public bool forceRespawn = false;
    [Tooltip("If set, an invincible damageable hit will still get the onHit message (but won't loose any life)")]
    public bool ignoreInvincibility = false;
    public LayerMask hittableLayers;
    public DamagableEvent OnDamageableHit;
    public NonDamagableEvent OnNonDamageableHit;
    public UnityEvent OnAllHitFinish;

    protected bool m_SpriteOriginallyFlipped;
    protected bool m_CanDamage = true;
    protected ContactFilter2D m_AttackContactFilter;
    protected Collider2D[] m_AttackOverlapResults = new Collider2D[10];
    protected Transform m_DamagerTransform;
    protected Collider2D m_LastHit;

    [HideInInspector]
    public GameObject targetObject = null;

    void Awake()
    {
        m_AttackContactFilter.layerMask = hittableLayers;
        m_AttackContactFilter.useLayerMask = true;
        m_AttackContactFilter.useTriggers = canHitTriggers;

        if (offsetBasedOnSpriteFacing && spriteRenderer != null)
            m_SpriteOriginallyFlipped = spriteRenderer.flipX;

        m_DamagerTransform = transform;
    }

    public void EnableDamage()
    {
        //if (hitTimes > 0)
        //{
        //}
        //hitIntervalTimer = 0;
        m_CanDamage = true;
    }

    public void DisableDamage()
    {
        m_CanDamage = false;
    }

    void FixedUpdate()
    {
        if (!m_CanDamage || hitTimes <= 0)
        {
            return;
        }
        if (hitIntervalTimer > 0)
        {
            hitIntervalTimer -= Time.fixedDeltaTime;
            return;
        }

        Vector2 scale = m_DamagerTransform.lossyScale;

        Vector2 facingOffset = Vector2.Scale(offset, scale);
        if (offsetBasedOnSpriteFacing && spriteRenderer != null && spriteRenderer.flipX != m_SpriteOriginallyFlipped)
            facingOffset = new Vector2(-offset.x * scale.x, offset.y * scale.y);

        Vector2 scaledSize = Vector2.Scale(size, scale);

        Vector2 pointA = (Vector2)m_DamagerTransform.position + facingOffset - scaledSize * 0.5f;
        Vector2 pointB = pointA + scaledSize;

        int hitCount = Physics2D.OverlapArea(pointA, pointB, m_AttackContactFilter, m_AttackOverlapResults);
        //Physics2D.GetContacts()

        for (int i = 0; i < hitCount; i++)
        {
            m_LastHit = m_AttackOverlapResults[i];
            Damageable damageable = m_LastHit.GetComponent<Damageable>();

            if ((targetObject == null && damageable) || (targetObject != null && damageable && targetObject == damageable.gameObject))
            {
                OnDamageableHit.Invoke(this, damageable);
                damageable.TakeDamage(this, ignoreInvincibility);
                if (disableDamageAfterHit)
                    DisableDamage();
            }
            else
            {
                OnNonDamageableHit.Invoke(this);
            }

            if (i == hitCount - 1)
            {
                if (hitTimes > 0)
                {
                    hitIntervalTimer = hitIntervalTime;
                    --hitTimes;
                    if (hitTimes < 1)
                    {
                        DisableDamage();
                        OnAllHitFinish.Invoke();
                    }
                }
            }
        }
    }
}
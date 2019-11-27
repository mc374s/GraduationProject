using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [Serializable]
    public class HealthEvent : UnityEvent<Damageable>
    { }

    [Serializable]
    public class DamageEvent : UnityEvent<Damager, Damageable>
    { }

    [Serializable]
    public class HealEvent : UnityEvent<int, Damageable>
    { }

    public int startingHealth = 5;
    public int safeHealth = 5;
    public bool invulnerableAfterDamage = true;
    public float invulnerabilityDuration = 3f;
    public bool disableOnDeath = false;
    public float knockDownTime = 4f;
    protected float knockDownTimer;
    [Tooltip("An offset from the obejct position used to set from where the distance to the damager is computed")]
    public Vector2 centreOffset = new Vector2(0f, 1f);
    public HealthEvent OnHealthSet;
    public DamageEvent OnTakeDamage;
    public DamageEvent OnKnockDown;
    public DamageEvent OnDie;
    public HealEvent OnGainHealth;

    protected bool m_Invulnerable;
    protected float m_InulnerabilityTimer;
    [SerializeField]
    protected int m_CurrentHealth;
    protected int m_CurrentSkillEnergy;
    protected Vector2 m_DamageDirection;
    protected bool m_ResetHealthOnSceneReload;
    protected bool m_ResetSkillEnergyOnSceneReload;

    public int CurrentHealth
    {
        get { return m_CurrentHealth; }
    }


    public bool IsKnockDown { get; set; }
    protected bool isAutoHealing = false;
    protected int healSpeed = 60;

    void OnEnable()
    {
        m_CurrentHealth = startingHealth;

        OnHealthSet.Invoke(this);

        DisableInvulnerability();
    }

    void OnDisable()
    {
    }

    void Update()
    {
        if (m_Invulnerable)
        {
            m_InulnerabilityTimer -= Time.deltaTime;

            if (m_InulnerabilityTimer <= 0f)
            {
                m_Invulnerable = false;
            }
        }
        if (IsKnockDown)
        {
            knockDownTimer -= Time.deltaTime;
            if (knockDownTimer <= 0f)
            {
                IsKnockDown = false;
                isAutoHealing = true;
            }
        }
        if (isAutoHealing)
        {
            m_CurrentHealth += (int)(healSpeed * Time.deltaTime);
            if (m_CurrentHealth>safeHealth)
            {
                m_CurrentHealth = safeHealth;
                isAutoHealing = false;
            }
        }
    }

    public void EnableInvulnerability(bool ignoreTimer = false)
    {
        m_Invulnerable = true;
        //technically don't ignore timer, just set it to an insanly big number. Allow to avoid to add more test & special case.
        m_InulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
    }

    public void DisableInvulnerability()
    {
        m_Invulnerable = false;
    }

    public Vector2 GetDamageDirection()
    {
        return m_DamageDirection;
    }

    public void TakeDamage(Damager damager, bool ignoreInvincible = false)
    {
        if ((m_Invulnerable && !ignoreInvincible) || m_CurrentHealth <= 0)
            return;

        //we can reach that point if the damager was one that was ignoring invincible state.
        //We still want the callback that we were hit, but not the damage to be removed from health.
        if (!m_Invulnerable)
        {
            if (invulnerableAfterDamage)
            {
                EnableInvulnerability();
            }
            m_CurrentHealth -= damager.damage;
            isAutoHealing = false;
            OnHealthSet.Invoke(this);
        }

        m_DamageDirection = transform.position + (Vector3)centreOffset - damager.transform.position;

        OnTakeDamage.Invoke(damager, this);

        if (m_CurrentHealth <= 0 && !IsKnockDown)
        {
            OnKnockDown.Invoke(damager, this);
            m_CurrentHealth = 1;
            knockDownTimer = knockDownTime;
            EnableInvulnerability();
            IsKnockDown = true;
        }
        if (m_CurrentHealth <= 0 && IsKnockDown)
        {
            OnDie.Invoke(damager, this);
            m_ResetHealthOnSceneReload = true;
            EnableInvulnerability();
            if (disableOnDeath) gameObject.SetActive(false);
        }
    }

    public void GainHealth(int amount)
    {
        m_CurrentHealth += amount;

        if (m_CurrentHealth > startingHealth)
            m_CurrentHealth = startingHealth;

        OnHealthSet.Invoke(this);

        OnGainHealth.Invoke(amount, this);
    }

    public void SetHealth(int amount)
    {
        m_CurrentHealth = amount;

        if (m_CurrentHealth <= 0)
        {
            OnDie.Invoke(null, this);
            m_ResetHealthOnSceneReload = true;
            EnableInvulnerability();
            if (disableOnDeath) gameObject.SetActive(false);
        }

        OnHealthSet.Invoke(this);
    }



}

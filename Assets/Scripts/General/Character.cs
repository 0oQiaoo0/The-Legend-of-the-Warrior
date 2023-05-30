using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;
    [Header("受伤无敌")]
    public float invulnerableDuration;
    [HideInInspector] public float invulnerableCounter;
    public bool invulnerable;

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;
    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
    }
    private void Update()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }
    public void TakeDamage(Attack attacker)
    {
        if (!invulnerable)
        {
            if (currentHealth <= attacker.damage)
            {
                currentHealth = 0;
                //触发死亡
                OnDie?.Invoke();
            }
            else
            {
                currentHealth -= attacker.damage;
                TriggerInvulnerable();
                //执行受伤
                OnTakeDamage?.Invoke(attacker.transform);
            }
            OnHealthChange?.Invoke(this);
        }
    }
    /// <summary>
    /// 触发受伤无敌
    /// </summary>
    private void TriggerInvulnerable()
    {
        invulnerable = true;
        invulnerableCounter = invulnerableDuration;
    }
}

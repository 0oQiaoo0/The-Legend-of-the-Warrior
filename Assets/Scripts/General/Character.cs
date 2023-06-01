using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;
    [Header("受伤无敌")]
    public float invulnerableDuration;
    [HideInInspector] public float invulnerableCounter;
    public bool invulnerable;

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Character> OnPowerChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;
    private void Start()
    {
        currentPower = maxPower;
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
        OnPowerChange?.Invoke(this);
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
        RecoverPower();
    }

    private void RecoverPower()
    {
        if (currentPower + Time.deltaTime * powerRecoverSpeed >= maxPower) currentPower = maxPower;
        else currentPower += Time.deltaTime * powerRecoverSpeed;

        OnPowerChange?.Invoke(this);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            Debug.Log("water");
            //死亡、更新血量
            OnDie?.Invoke();
            currentHealth = 0;
            OnHealthChange.Invoke(this);
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

    public void PowerSpend(float cost)
    {
        currentPower -= cost;
        OnPowerChange?.Invoke(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("��������")]
    public float maxHealth;
    public float currentHealth;
    [Header("�����޵�")]
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
                //��������
                OnDie?.Invoke();
            }
            else
            {
                currentHealth -= attacker.damage;
                TriggerInvulnerable();
                //ִ������
                OnTakeDamage?.Invoke(attacker.transform);
            }
            OnHealthChange?.Invoke(this);
        }
    }
    /// <summary>
    /// ���������޵�
    /// </summary>
    private void TriggerInvulnerable()
    {
        invulnerable = true;
        invulnerableCounter = invulnerableDuration;
    }
}

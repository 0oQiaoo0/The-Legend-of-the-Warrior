using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("�¼�����")]
    public VoidEventSO newGameEvent;
    [Header("��������")]
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;
    [Header("�����޵�")]
    public float invulnerableDuration;
    [HideInInspector] public float invulnerableCounter;
    public bool invulnerable;

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Character> OnPowerChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;
    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
    }
    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
    }
    private void Start()
    {
        //��ҳ�ʼ״̬����&���ɳ���ʱ����״̬����
        currentPower = maxPower;
        currentHealth = maxHealth;
    }
    private void NewGame()
    {
        //���¿�ʼ��Ϸʱ���״̬����
        currentPower = maxPower;
        currentHealth = maxHealth;
        //OnHealthChange?.Invoke(this);//������Ҫ�ٸ���¼�
        //OnPowerChange?.Invoke(this);
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
            //����������Ѫ��
            OnDie?.Invoke();
            currentHealth = 0;
            OnHealthChange?.Invoke(this);
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

    public void PowerSpend(float cost)
    {
        currentPower -= cost;
        OnPowerChange?.Invoke(this);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;

    [Header("ÊÂ¼þ¼àÌý")]
    public CharacterEventSO healthEvent;
    public CharacterEventSO powerEvent;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        powerEvent.OnEventRaised += OnPowerEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        powerEvent.OnEventRaised -= OnPowerEvent;
    }

    private void OnHealthEvent(Character character)
    {
        var persentage = character.currentHealth / character.maxHealth;
        playerStateBar.OnHealthChange(persentage);
    }

    private void OnPowerEvent(Character character)
    {
        var persentage = character.currentPower / character.maxPower;
        playerStateBar.OnPowerChange(persentage);
    }

    public void PowerLack()
    {
        playerStateBar.PowerLack();
    }

    public void PowerLackEnd()
    {
        playerStateBar.PowerLackEnd();
    }
}

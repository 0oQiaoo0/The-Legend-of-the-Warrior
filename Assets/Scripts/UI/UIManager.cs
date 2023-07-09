using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;

    [Header("事件监听")]
    public CharacterEventSO healthEvent;
    public CharacterEventSO powerEvent;
    public SceneLoadEventSO unloadEvent;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        powerEvent.OnEventRaised += OnPowerEvent;
        unloadEvent.LoadRequestEvent += OnUnloadEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        powerEvent.OnEventRaised -= OnPowerEvent;
        unloadEvent.LoadRequestEvent -= OnUnloadEvent;
    }

    private void OnUnloadEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
        playerStateBar.gameObject.SetActive(!isMenu);
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

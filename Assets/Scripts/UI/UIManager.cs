using General;
using SO;
using UnityEngine;
using Utilities;

namespace UI
{
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
            var percentage = character.currentHealth / character.maxHealth;
            playerStateBar.OnHealthChange(percentage);
        }

        private void OnPowerEvent(Character character)
        {
            var percentage = character.currentPower / character.maxPower;
            playerStateBar.OnPowerChange(percentage);
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
}

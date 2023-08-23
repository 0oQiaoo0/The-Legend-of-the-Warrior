using General;
using SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Utilities;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public PlayerStateBar playerStateBar;
        [Header("事件监听")]
        public CharacterEventSO healthEvent;
        public CharacterEventSO powerEvent;
        public SceneLoadEventSO sceneUnloadedEvent;
        public VoidEventSO loadGameEvent;
        public VoidEventSO gameOverEvent;
        public VoidEventSO backToMenuEvent;

        [Header("组件")]
        public GameObject mainCanvas;
        public GameObject gameOverPanel;
        public GameObject restartButton;
        
        private void OnEnable()
        {
            healthEvent.OnEventRaised += OnHealthEvent;
            powerEvent.OnEventRaised += OnPowerEvent;
            sceneUnloadedEvent.LoadRequestEvent += OnSceneUnloadedEvent;
            loadGameEvent.OnEventRaised += OnLoadGameEvent;
            gameOverEvent.OnEventRaised += OnGameOverEvent;
            backToMenuEvent.OnEventRaised += OnLoadGameEvent;
        }

        private void OnDisable()
        {
            healthEvent.OnEventRaised -= OnHealthEvent;
            powerEvent.OnEventRaised -= OnPowerEvent;
            sceneUnloadedEvent.LoadRequestEvent -= OnSceneUnloadedEvent;
            loadGameEvent.OnEventRaised -= OnLoadGameEvent;
            gameOverEvent.OnEventRaised -= OnGameOverEvent;
            backToMenuEvent.OnEventRaised -= OnLoadGameEvent;
        }

        private void OnGameOverEvent()
        {
            gameOverPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(restartButton);
        }

        private void OnLoadGameEvent()
        {
            gameOverPanel.SetActive(false);
        }

        private void OnSceneUnloadedEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
        {
            var isMenu = sceneToLoad.sceneType == SceneType.Menu;
            // playerStateBar.gameObject.SetActive(!isMenu);
            mainCanvas.GetComponent<CanvasGroup>().alpha = isMenu ? 0 : 1;
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

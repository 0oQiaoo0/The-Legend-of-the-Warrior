using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        public GameObject newGameButton;

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(newGameButton);
        }

        public void ExitGame()
        {
            Debug.Log("Quit!");
            Application.Quit();
        }
    }
}

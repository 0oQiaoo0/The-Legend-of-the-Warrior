using DG.Tweening;
using SO;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FadeCanvas : MonoBehaviour
    {
        [Header("事件监听")]
        public FadeEventSO fadeEvent;

        public Image fadeImage;

        private void OnEnable()
        {
            fadeEvent.OnEventRaised += OnFadeEvent;
        }

        private void OnDisable()
        {
            fadeEvent.OnEventRaised -= OnFadeEvent;
        }

        private void OnFadeEvent(Color color,float duration)
        {
            fadeImage.DOBlendableColor(color, duration);
        }
    }
}

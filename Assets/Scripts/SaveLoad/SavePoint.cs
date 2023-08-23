using SO;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace SaveLoad
{
    public class SavePoint : MonoBehaviour, IInteractable
    {
        [Header("广播")]
        public VoidEventSO saveGameEvent;
        [Header("参数")]
        public SpriteRenderer spriteRenderer;
        public Sprite darkSprite;
        public Sprite lightSprite;
        public GameObject lightObj;
        public bool isDone;

        private void OnEnable()
        {
            spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
            lightObj.SetActive(isDone);
        }

        public void TriggerAction()
        {
            if (isDone) return;
            
            isDone = true;
            spriteRenderer.sprite = lightSprite;
            lightObj.SetActive(true);
            //TODO:保存数据
            saveGameEvent.RaiseEvent();
            gameObject.tag = "Untagged";
        }
    }
}

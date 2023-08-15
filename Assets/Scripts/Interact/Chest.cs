using Audio;
using UnityEngine;
using Utilities;

namespace Interact
{
    public class Chest : MonoBehaviour, IInteractable
    {
        private SpriteRenderer _spriteRenderer;
        private AudioDefinition _audioDefinition;

        public Sprite openSprite;
        public Sprite closeSprite;
        public bool isDone;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _audioDefinition = GetComponent<AudioDefinition>() ?? null;
        }

        private void OnEnable()
        {
            _spriteRenderer.sprite = isDone ? openSprite : closeSprite;
        }

        public void TriggerAction()
        {
            if (!isDone)
            {
                OpenChest();
            }
        }

        private void OpenChest()
        {
            gameObject.tag = "Finish";
            _spriteRenderer.sprite = openSprite;
            isDone = true;
            if(_audioDefinition) _audioDefinition.PlayAudioClip();
        }
    }
}

using SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("事件监听")]
        public PlayAudioEventSO fxEvent;
        public PlayAudioEventSO bgmEvent;
        public VoidEventSO afterSceneLoadedEvent;
        [Header("组件")]
        public AudioSource bgmSource;
        public AudioSource fxSource;

        private void OnEnable()
        {
            fxEvent.OnEventRaised += OnFXEvent;
            bgmEvent.OnEventRaised += OnBGMEvent;
            afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
        }

        private void OnDisable()
        {
            fxEvent.OnEventRaised -= OnFXEvent;
            bgmEvent.OnEventRaised -= OnBGMEvent;
            afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
        }

        private void OnBGMEvent(AudioClip clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }

        private void OnFXEvent(AudioClip clip)
        {
            fxSource.clip = clip;
            fxSource.Play();
        }

        private void OnAfterSceneLoadedEvent()
        {
            PlayNewBGM();
        }
        private void PlayNewBGM()
        {
            var obj = GameObject.FindWithTag("BGM");
            if (!obj) return;

            OnBGMEvent(obj.GetComponent<AudioDefinition>().audioClip);
        }
    }
}

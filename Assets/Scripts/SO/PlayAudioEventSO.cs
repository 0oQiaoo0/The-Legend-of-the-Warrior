using UnityEngine;
using UnityEngine.Events;

namespace SO
{
    [CreateAssetMenu(menuName = "Event/PlayAudioEventSO")]
    public class PlayAudioEventSO : ScriptableObject
    {
        public UnityAction<AudioClip> OnEventRaised;

        public void RaiseEvent(AudioClip audioClip)
        {
            OnEventRaised?.Invoke(audioClip);
        }
    }
}

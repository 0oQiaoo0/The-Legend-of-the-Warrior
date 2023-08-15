using General;
using UnityEngine;
using UnityEngine.Events;

namespace SO
{
    [CreateAssetMenu(menuName = "Event/CharacterEventSO")]
    public class CharacterEventSO : ScriptableObject
    {
        public UnityAction<Character> OnEventRaised;

        public void RaiseEvent(Character character)
        {
            OnEventRaised?.Invoke(character);
        }
    }
}

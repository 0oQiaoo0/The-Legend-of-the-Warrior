using UnityEngine;
using UnityEngine.Events;

namespace SO
{
    [CreateAssetMenu(menuName = "Events/FloatEventSO")]
    public class FloatEventSO : ScriptableObject
    {
        public UnityAction<float> OnEventRaised;
        
        public void RaiseEvent(float value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

namespace SO
{
    [CreateAssetMenu(menuName = "Event/FadeEventSO")]
    public class FadeEventSO : ScriptableObject
    {
        public UnityAction<Color, float> OnEventRaised;

        public void RaiseEvent(Color color,float duration)
        {
            OnEventRaised?.Invoke(color, duration);
        }
        /// <summary>
        /// 逐渐变黑
        /// </summary>
        /// <param name="duration"></param>
        public void FadeIn(float duration)
        {
            RaiseEvent(Color.black, duration);
        }
        /// <summary>
        /// 逐渐变透明
        /// </summary>
        /// <param name="duration"></param>
        public void FadeOut(float duration)
        {
            RaiseEvent(Color.clear, duration);
        }
    }
}

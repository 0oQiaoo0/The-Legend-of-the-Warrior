using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{
    public UnityAction<Color, float> OnEventRaised;

    public void RaiseEvent(Color color,float duration)
    {
        OnEventRaised?.Invoke(color, duration);
    }
    /// <summary>
    /// Öð½¥±äºÚ
    /// </summary>
    /// <param name="duration"></param>
    public void FadeIn(float duration)
    {
         RaiseEvent(Color.black, duration);
    }
    /// <summary>
    /// Öð½¥±äÍ¸Ã÷
    /// </summary>
    /// <param name="duration"></param>
    public void FadeOut(float duration)
    {
        RaiseEvent(Color.clear, duration);
    }
}

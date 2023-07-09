using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="sceneToLoad">Ҫ���صĳ���</param>
    /// <param name="posToGo">Player��Ŀ������</param>
    /// <param name="fadeScreen">�Ƿ��뽥��</param>
    public void RaiseEvent(GameSceneSO sceneToLoad,Vector3 posToGo,bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(sceneToLoad, posToGo, fadeScreen);
    }
}

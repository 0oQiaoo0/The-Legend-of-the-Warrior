using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public SceneLoadEventSO loadEventSO;

    public GameSceneSO SceneToGo;

    public Vector3 positionToGo;
    public void TriggerAction()
    {
        loadEventSO.RaiseEvent(SceneToGo, positionToGo, true);
    }
}

using SO;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Transition
{
    public class TeleportPoint : MonoBehaviour, IInteractable
    {
        public SceneLoadEventSO loadEventSO;

        [FormerlySerializedAs("SceneToGo")] public GameSceneSO sceneToGo;

        public Vector3 positionToGo;
        public void TriggerAction()
        {
            loadEventSO.RaiseEvent(sceneToGo, positionToGo, true);
        }
    }
}

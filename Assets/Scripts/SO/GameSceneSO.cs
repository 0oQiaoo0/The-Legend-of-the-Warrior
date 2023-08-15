using UnityEngine;
using UnityEngine.AddressableAssets;
using Utilities;

namespace SO
{
    [CreateAssetMenu(menuName = "Game Scene/GameSceneSO")]
    public class GameSceneSO : ScriptableObject
    {
        public SceneType sceneType;

        public AssetReference sceneReference;
    }
}

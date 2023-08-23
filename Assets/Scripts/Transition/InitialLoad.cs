using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Transition
{
    public class InitialLoad : MonoBehaviour
    {
        public AssetReference persistentScene;

        private void Awake()
        {
            Addressables.LoadSceneAsync(persistentScene);
        }
    }
}

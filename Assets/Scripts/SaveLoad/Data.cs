using System.Collections.Generic;
using SO;
using UnityEngine;
using Utilities;

namespace SaveLoad
{
    public class Data
    {
        public string sceneToSave;
        public Dictionary<string, SerializableVector3> CharacterPosDict = new();
        public Dictionary<string, float> FloatSavedData = new();

        public void SaveGameScene(GameSceneSO savedScene)
        {
            sceneToSave = JsonUtility.ToJson(savedScene);
            Debug.Log("Save scene: " + sceneToSave);
        }

        public GameSceneSO GetSavedScene()
        {
            var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
            JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
            return newScene;
        }
    }
}

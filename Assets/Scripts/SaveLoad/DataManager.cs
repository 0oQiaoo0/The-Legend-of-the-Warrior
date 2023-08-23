using System.Collections.Generic;
using SO;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace SaveLoad
{
    [DefaultExecutionOrder(-10)]
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;
        [Header("事件监听")]
        public VoidEventSO saveGameEvent;

        public VoidEventSO loadGameEvent;

        private List<ISavable> _savableList = new();

        private string _jsonFolder;

        private Data _saveData;
        private void Awake()
        {
            if(!Instance) 
                Instance = this;
            else 
                Destroy(gameObject);

            _saveData = new Data();

            _jsonFolder = Application.persistentDataPath + "/SaveData/";
            
            ReadSavedData();
        }

        private void OnEnable()
        {
            saveGameEvent.OnEventRaised += SaveGame;
            loadGameEvent.OnEventRaised += LoadGame;
        }

        private void OnDisable()
        {
            saveGameEvent.OnEventRaised -= SaveGame;
            loadGameEvent.OnEventRaised -= LoadGame;
        }

        private void Update()
        {
            // if (Keyboard.current.lKey.wasPressedThisFrame)
            // {
            //     LoadGame();
            // }
        }

        private void SaveGame()
        {
            foreach (var savable in _savableList)
            {
                savable.GetSaveData(_saveData);
            }
            
            var resultPath = _jsonFolder + "SaveData.sav";
            
            var jsonData = JsonConvert.SerializeObject(_saveData);

            if (!File.Exists(resultPath))
            {
                Directory.CreateDirectory(_jsonFolder);
            }
            
            File.WriteAllText(resultPath, jsonData);
            
            // foreach(var item in _saveData.CharacterPosDict)
            // {
            //     Debug.Log(item.Key + " " + item.Value);
            // }
        }

        private void LoadGame()
        {
            foreach (var savable in _savableList)
            {
                savable.LoadData(_saveData);
            }
        }

        private void ReadSavedData()
        {
            var resultPath = _jsonFolder + "SaveData.sav";
            
            if (File.Exists(resultPath))
            {
                var stringData = File.ReadAllText(resultPath);

                var jsonData = JsonConvert.DeserializeObject<Data>(stringData);
                _saveData = jsonData;
            }
        }
        
        public void RegisterSaveData(ISavable savable)
        {
            if(_savableList.Contains(savable)) return;
            
            _savableList.Add(savable);
        }
        
        public void UnRegisterSaveData(ISavable savable)
        {
            _savableList.Remove(savable);
        }
        
        
    }
}

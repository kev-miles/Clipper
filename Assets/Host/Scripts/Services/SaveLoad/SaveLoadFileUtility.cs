using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Host.Infrastructure.FileManagement;
using Host.Infrastructure.Interfaces;
using UniRx;
using UnityEngine;

namespace Host.Scripts.Services.SaveLoad
{
    public class SaveLoadFileUtility : ISaveLoadFileUtility
    {
        //TODO: Move to persistentDataPath
        private static readonly string SaveFolder = $"{UnityEngine.Application.dataPath}/Saves";
        private readonly Dictionary<int, string> _saveSlotPaths = new Dictionary<int, string>(); 
        private readonly Dictionary<int, Dictionary<string, string>> _savedDataDictionary = new Dictionary<int, Dictionary<string, string>>();

        public SaveLoadFileUtility()
        {
            CheckSavesFolder();
        }
        
        public Dictionary<int, Dictionary<string, string>> SavedDataDictionary() => _savedDataDictionary;

        public IObservable<Unit> WriteFile(SaveLoadObject saveObject)
        {
            return Observable.Create<Unit>(emitter =>
            {
                _savedDataDictionary[saveObject.SlotId] = saveObject.SaveData;
                var jsonObject = JsonUtility.ToJson(saveObject.ToSerializable());
                var fileName = String.Format("{0}{1}{2}",
                        saveObject.SaveData[SaveDataKeys.UserName],
                        saveObject.SlotId.ToString(),
                        ("save.txt"));
                
                File.WriteAllText($"{SaveFolder}/{fileName}", jsonObject);
                
                emitter.OnNext(Unit.Default);
                emitter.OnCompleted();

                return Disposable.Empty;
            });
        }
        
        public IObservable<Unit> ReadFile(int slotId)
        {
            return Observable.Create<Unit>(emitter =>
            {
                foreach (var file in Directory.EnumerateFiles(SaveFolder, "*.txt"))
                {
                    if (!Path.GetFileName(file).First().Equals(slotId.ToString().First())) continue;
                    
                    var fileContent = File.ReadAllText(file);
                    var saveObject = JsonUtility.FromJson<SerializableSaveLoadObject>(fileContent).ToSaveLoadObject();
                    UpdateSaveDataPaths(saveObject.SlotId, file);
                    UpdateSaveDataDictionary(saveObject);
                }
                
                emitter.OnNext(Unit.Default);
                emitter.OnCompleted();

                return Disposable.Empty;
            });
        }

        public IObservable<Unit> ReadFiles()
        {
            return Observable.Create<Unit>(emitter =>
            {
                _savedDataDictionary.Clear();
                foreach (var file in Directory.EnumerateFiles(SaveFolder, "*.txt"))
                {
                    var fileContent = File.ReadAllText(file);
                    var saveObject = JsonUtility.FromJson<SerializableSaveLoadObject>(fileContent).ToSaveLoadObject();
                    UpdateSaveDataPaths(saveObject.SlotId, file);
                    UpdateSaveDataDictionary(saveObject);
                }

                emitter.OnNext(Unit.Default);
                emitter.OnCompleted();

                return Disposable.Empty;
            });
        }

        public IObservable<Unit> DeleteFile(int slotId)
        {
            return Observable.Create<Unit>(emitter =>
            {
                if (_saveSlotPaths.ContainsKey(slotId))
                {
                    var file = _saveSlotPaths[slotId];
                    File.Delete(file);
                    _saveSlotPaths.Remove(slotId);
                    _savedDataDictionary.Remove(slotId);
                }

                emitter.OnNext(Unit.Default);
                emitter.OnCompleted();

                return Disposable.Empty;
            });
        }

        private void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }
        
        private void CheckSavesFolder()
        {
            if (!Directory.Exists(SaveFolder))
                Directory.CreateDirectory(SaveFolder);
        }

        private void UpdateSaveDataDictionary(SaveLoadObject saveObject)
        {
            if(_savedDataDictionary.ContainsKey(saveObject.SlotId))
                _savedDataDictionary[saveObject.SlotId] = saveObject.SaveData;
            else
                _savedDataDictionary.Add(saveObject.SlotId, saveObject.SaveData);
        }
        
        private void UpdateSaveDataPaths(int slotId, string path)
        {
            if (_savedDataDictionary.ContainsKey(slotId))
                _saveSlotPaths[slotId] = path;
            else
                _saveSlotPaths.Add(slotId, path);
        }
    }
}
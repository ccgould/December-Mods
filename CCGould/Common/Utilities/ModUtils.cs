﻿using MAC.FireExtinguisherHolder.Config;
using Oculus.Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Common.Utilities
{
    internal static class ModUtils
    {

        private static MonoBehaviour _coroutineObject;


        public static void Save<SaveDataT>(SaveDataT newSaveData, string fileName, string saveDirectory, Action onSaveComplete = null)
        {
            if (newSaveData != null)
            {
                var saveDataJson = JsonConvert.SerializeObject(newSaveData, Formatting.Indented);

                if (!Directory.Exists(saveDirectory))
                {
                    Directory.CreateDirectory(saveDirectory);
                }

                File.WriteAllText(Path.Combine(saveDirectory, fileName), saveDataJson);

                onSaveComplete?.Invoke();
            }
        }

        public static void LoadSaveData<TSaveData>(string fileName, string saveDirectory, Action<TSaveData> onSuccess) where TSaveData : new()
        {
            var save = File.ReadAllText(Path.Combine(saveDirectory, fileName));
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            var json = JsonConvert.DeserializeObject<TSaveData>(save, jsonSerializerSettings);
            onSuccess?.Invoke(json);
        }

        private static Coroutine StartCoroutine(IEnumerator coroutine)
        {
            if (_coroutineObject == null)
            {
                var go = new GameObject();
                _coroutineObject = go.AddComponent<Mod.ModSaver>();
            }

            return _coroutineObject.StartCoroutine(coroutine);
        }
    }
}

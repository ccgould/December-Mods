using Common.Utilities;
using MAC.FireExtinguisherHolder.Mono;
using SMLHelper.V2.Utility;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace MAC.FireExtinguisherHolder.Config
{
    internal static class Mod
    {
        private static ModSaver _saveObject;
        private static FEHolderSaveData _fEHolderSaveData;
        internal static string ModName => "FEHolder";
        internal static string BundleName => "feholdermodbundle";

        internal const string SaveDataFilename = "FEHolderSaveData.json";

        internal static event Action<FEHolderSaveData> OnDataLoaded;
        internal static void Save()
        {
            if (!IsSaving())
            {
                _saveObject = new GameObject().AddComponent<ModSaver>();

                FEHolderSaveData newSaveData = new FEHolderSaveData();

                var drills = GameObject.FindObjectsOfType<FEHolderController>();

                foreach (var drill in drills)
                {
                    drill.Save(newSaveData);
                }

                _fEHolderSaveData = newSaveData;

                ModUtils.Save<FEHolderSaveData>(_fEHolderSaveData, SaveDataFilename, GetSaveFileDirectory(), OnSaveComplete);
            }
        }

        internal static bool IsSaving()
        {
            return _saveObject != null;
        }

        public static void OnSaveComplete()
        {
            _saveObject.StartCoroutine(SaveCoroutine());
        }

        private static IEnumerator SaveCoroutine()
        {
            while (SaveLoadManager.main != null && SaveLoadManager.main.isSaving)
            {
                yield return null;
            }
            GameObject.DestroyImmediate(_saveObject.gameObject);
            _saveObject = null;
        }

        public static string GetSaveFileDirectory()
        {
            return Path.Combine(SaveUtils.GetCurrentSaveDataDir(), ModName);
        }

        public class ModSaver : MonoBehaviour
        {
        }

        internal static FEHolderSaveDataEntry GetSaveData(string id)
        {
            LoadData();

            var saveData = GetSaveData();

            foreach (var entry in saveData.Entries)
            {
                if (entry.ID == id)
                {
                    return entry;
                }
            }

            return new FEHolderSaveDataEntry() { ID = id };
        }

        internal static FEHolderSaveData GetSaveData()
        {
            return _fEHolderSaveData ?? new FEHolderSaveData();
        }

        public static void LoadData()
        {
            QuickLogger.Info("Loading Save Data...");
            ModUtils.LoadSaveData<FEHolderSaveData>(SaveDataFilename, GetSaveFileDirectory(), (data) =>
            {
                _fEHolderSaveData = data;
                QuickLogger.Info("Save Data Loaded");
                OnDataLoaded?.Invoke(_fEHolderSaveData);
            });
        }
    }
}

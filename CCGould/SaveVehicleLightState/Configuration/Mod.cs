using Common.Utilities;
using MAC.SaveVehicleLightState.Mod;
using Oculus.Newtonsoft.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Utility;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace MAC.SaveVehicleLightState.Configuration
{
    internal static class Mod
    {

        #region Private Members
        private static ModSaver _saveObject;
        private static SaveData _saveData;
        private static readonly string SaveDataFilename = $"{ModName}SaveData.json";
        #endregion

        #region Internal Properties
        internal static string ModName => "SaveVehicleLightState";
        internal static string MODFOLDERLOCATION => GetModPath();
        internal static ModConfiguration Configuration { get; set; }
        internal static event Action<SaveData> OnDataLoaded;
        #endregion

        #region Internal Methods
        public static void Save()
        {
            if (!IsSaving())
            {
                _saveObject = new GameObject().AddComponent<ModSaver>();

                SaveData newSaveData = new SaveData();

                var drills = GameObject.FindObjectsOfType<VehicleManager>();

                foreach (var drill in drills)
                {
                    drill.Save(newSaveData);
                }

                _saveData = newSaveData;

                ModUtils.Save<SaveData>(_saveData, SaveDataFilename, GetSaveFileDirectory(), OnSaveComplete);
            }
        }

        internal static bool IsSaving()
        {
            return _saveObject != null;
        }

        internal static void OnSaveComplete()
        {
            _saveObject.StartCoroutine(SaveCoroutine());
        }

        internal static void LoadConfiguration()
        {
            // == Load Configuration == //
            string configJson = File.ReadAllText(Mod.ConfigurationFile().Trim());

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            //LoadData
            Configuration = JsonConvert.DeserializeObject<ModConfiguration>(configJson, settings);
        }
        internal static string ConfigurationFile()
        {
            return Path.Combine(MODFOLDERLOCATION, "mod.json");
        }
        internal static void SetModEnabled(bool eValue)
        {
            Configuration.Config.Enabled = eValue;
            SaveConfiguration();
        }

        internal static string GetSaveFileDirectory()
        {
            return Path.Combine(SaveUtils.GetCurrentSaveDataDir(), ModName);
        }

        internal static SaveDataEntry GetSaveData(string id)
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

            return new SaveDataEntry() { ID = id };
        }

        internal static SaveData GetSaveData()
        {
            return _saveData ?? new SaveData();
        }

        internal static void LoadData()
        {
            QuickLogger.Info("Loading Save Data...");
            ModUtils.LoadSaveData<SaveData>(SaveDataFilename, GetSaveFileDirectory(), (data) =>
            {
                _saveData = data;
                QuickLogger.Info("Save Data Loaded");
                OnDataLoaded?.Invoke(_saveData);
            });
        }
        #endregion

        #region Private Methods
        private static IEnumerator SaveCoroutine()
        {
            while (SaveLoadManager.main != null && SaveLoadManager.main.isSaving)
            {
                yield return null;
            }
            GameObject.DestroyImmediate(_saveObject.gameObject);
            _saveObject = null;
        }
        private static string GetModPath()
        {
            return Path.Combine(GetQModsPath(), ModName);
        }
        private static string GetQModsPath()
        {
            return Path.Combine(Environment.CurrentDirectory, "QMods");
        }
        private static void SaveConfiguration()
        {
            var output = JsonConvert.SerializeObject(Configuration, Formatting.Indented);
            File.WriteAllText(Mod.ConfigurationFile().Trim(), output);
        }
        #endregion
    }

    internal class Options : ModOptions
    {
        private const string ToggleID = "DropAllOnDeathEnabled";
        private bool _enabled;

        public Options() : base("Drop All On Death Settings")
        {
            ToggleChanged += OnToggleChanged;
            _enabled = Mod.Configuration.Config.Enabled;
        }

        public void OnToggleChanged(object sender, ToggleChangedEventArgs e)
        {
            if (e.Id != ToggleID) return;
            _enabled = e.Value;
            Mod.SetModEnabled(e.Value);
        }

        public override void BuildModOptions()
        {
            AddToggleOption(ToggleID, "Enable Drop All On Death", _enabled);
        }
    }
}

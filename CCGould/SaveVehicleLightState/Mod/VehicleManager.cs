using Common.Utilities;
using MAC.SaveVehicleLightState.Configuration;
using UnityEngine;

namespace MAC.SaveVehicleLightState.Mod
{
    internal class VehicleManager : MonoBehaviour, IProtoEventListener
    {
        private SaveDataEntry _saveData;
        public string PrefabID { get; set; }
        public ToggleLights Toggle { get; set; }

        internal void Initialize(string id, ToggleLights toggle)
        {
            PrefabID = id;
            Toggle = toggle;
        }


        internal void Save(SaveData newSaveData)
        {
            var prefabIdentifier = GetComponent<PrefabIdentifier>();
            var id = prefabIdentifier.Id;

            if (_saveData == null)
            {
                _saveData = new SaveDataEntry();
            }
            _saveData.ID = id;
            _saveData.Value = Toggle.GetLightsActive();
            newSaveData.Entries.Add(_saveData);
        }


        public void OnProtoSerialize(ProtobufSerializer serializer)
        {
            if (!Configuration.Mod.IsSaving())
            {
                QuickLogger.Info($"Saving {Configuration.Mod.ModName}");
                Configuration.Mod.Save();
                QuickLogger.Info($"Saved {Configuration.Mod.ModName}");
            }
        }

        public void OnProtoDeserialize(ProtobufSerializer serializer)
        {

        }

        //private static Dictionary<string, bool> Vehicle { get; set; } = new Dictionary<string, bool>();

        //internal static bool IsTracked(string id)
        //{
        //    return Vehicle.ContainsKey(id);
        //}

        //internal static void SetLightState(string id, bool value)
        //{
        //    if (Vehicle.ContainsKey(id))
        //    {
        //        Vehicle[id] = value;
        //    }
        //    else
        //    {
        //        QuickLogger.Info($"{id} wasn't found in the list of vehicles");
        //    }
        //}

        //internal static void AddVehicle(string id, bool value)
        //{
        //    if (!IsTracked(id))
        //    {
        //        Vehicle.Add(id, value);
        //    }
        //}

        //internal static void Save(string id)
        //{

        //}

        //internal static bool Load(string id)
        //{
        //    return true;
        //}

        public void SetToggle(bool value)
        {
            QuickLogger.Info($"Setting Lights Active to {value} || Current: {Toggle.lightsActive}");

            Toggle.SetLightsActive(value);

            QuickLogger.Info($"Setting Lights Parent to {Toggle.lightsActive}");


            //Toggle.lightsActive = value;

            //if (Toggle.lightsParent != null)
            //{
            //    QuickLogger.Info($"Setting Lights Parent to {value}");
            //    Toggle.lightsParent.SetActive(value);
            //    QuickLogger.Info($"Lights Parent was set to {Toggle.lightsParent.activeSelf}");

            //}

            //__instance.toggleLights.lightsActive = data.Value;
        }
    }
}

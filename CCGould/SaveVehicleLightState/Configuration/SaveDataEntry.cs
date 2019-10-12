using Oculus.Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MAC.SaveVehicleLightState.Configuration
{
    [Serializable]
    internal class SaveDataEntry
    {
        [JsonProperty] internal string ID;
        [JsonProperty] internal bool Value { get; set; }
    }

    [Serializable]
    internal class SaveData
    {
        [JsonProperty] internal List<SaveDataEntry> Entries = new List<SaveDataEntry>();
    }
}

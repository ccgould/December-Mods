using Oculus.Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MAC.OxStation.Config
{
    [Serializable]
    internal class SaveDataEntry
    {
        [JsonProperty] internal float Fuel { get; set; }
        [JsonProperty] internal string ID { get; set; }
        [JsonProperty] internal bool HasTank { get; set; }
    }

    [Serializable]
    internal class SaveData
    {
        [JsonProperty] internal List<SaveDataEntry> Entries = new List<SaveDataEntry>();
    }
}

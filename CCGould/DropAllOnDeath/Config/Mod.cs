using Oculus.Newtonsoft.Json;
using SMLHelper.V2.Options;
using System;
using System.IO;

namespace MAC.DropAllOnDeath.Config
{
    internal static class Mod
    {
        #region Internal Properties
        internal static string ModName => "DropAllOnDeath";
        internal static string MODFOLDERLOCATION => GetModPath();
        internal static ModConfiguration Configuration { get; set; }
        #endregion

        #region Internal Methods
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
        #endregion

        #region Private Methods
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

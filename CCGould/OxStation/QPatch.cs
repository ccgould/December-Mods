using Common.Utilities;
using Harmony;
using MAC.OxStation.Buildables;
using MAC.OxStation.Config;
using Oculus.Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using QModManager.API.ModLoading;

namespace MAC.OxStation
{

    [QModCore]
    public static class QPatch
    {
        internal static Configuration Configuration { get; set; }

        [QModPatch]
        public static void Patch()
        {
            QuickLogger.Info("Started patching. Version: " + QuickLogger.GetAssemblyVersion());

#if DEBUG
            QuickLogger.DebugLogsEnabled = true;
            QuickLogger.Debug("Debug logs enabled");
#endif

            try
            {
                LoadConfiguration();

                OxStationBuildable.PatchHelper();

                var harmony = HarmonyInstance.Create("com.oxstation.MAC");

                harmony.PatchAll(Assembly.GetExecutingAssembly());

                QuickLogger.Info("Finished patching");
            }
            catch (Exception ex)
            {
                QuickLogger.Error(ex);
            }
        }

        private static void LoadConfiguration()
        {
            // == Load Configuration == //
            string configJson = File.ReadAllText(Mod.ConfigurationFile().Trim());

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;

            //LoadData
            Configuration = JsonConvert.DeserializeObject<Configuration>(configJson, settings);
        }
    }
}


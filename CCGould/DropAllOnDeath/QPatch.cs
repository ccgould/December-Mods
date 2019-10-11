using Common.Utilities;
using Harmony;
using MAC.DropAllOnDeath.Config;
using Oculus.Newtonsoft.Json;
using SMLHelper.V2.Handlers;
using System;
using System.IO;
using System.Reflection;

namespace MAC.DropAllOnDeath
{
    public class QPatch
    {
        public static void Patch()
        {
            QuickLogger.Info($"Started patching. Version: {QuickLogger.GetAssemblyVersion(Assembly.GetExecutingAssembly())}");

#if DEBUG
            QuickLogger.DebugLogsEnabled = true;
            QuickLogger.Debug("Debug logs enabled");
#endif

            try
            {
                LoadConfiguration();

                OptionsPanelHandler.RegisterModOptions(new Options());

                HarmonyInstance.Create("com.dropallondeath.MAC").PatchAll(Assembly.GetExecutingAssembly());

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
            Configuration = JsonConvert.DeserializeObject<ModConfiguration>(configJson, settings);
        }

        private static void SaveConfiguration()
        {
            var output = JsonConvert.SerializeObject(Configuration, Formatting.Indented);
            File.WriteAllText(Mod.ConfigurationFile().Trim(), output);
        }

        public static ModConfiguration Configuration { get; set; }

        internal static void SetModEnabled(bool eValue)
        {
            Configuration.Config.Enabled = eValue;
            SaveConfiguration();
        }
    }
}

using Common.Utilities;
using Harmony;
using MAC.DropAllOnDeath.Config;
using SMLHelper.V2.Handlers;
using System;
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
                Mod.LoadConfiguration();

                OptionsPanelHandler.RegisterModOptions(new Options());

                HarmonyInstance.Create("com.dropallondeath.MAC").PatchAll(Assembly.GetExecutingAssembly());

                QuickLogger.Info("Finished patching");
            }
            catch (Exception ex)
            {
                QuickLogger.Error(ex);
            }
        }
    }
}

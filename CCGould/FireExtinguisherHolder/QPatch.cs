using Common.Utilities;
using Harmony;
using MAC.CCgould.FireExtinguisherHolder.Buildable;
using System;
using System.Reflection;

namespace MAC.CCgould.FireExtinguisherHolder
{
    public static class QPatch
    {
        public static void Patch()
        {
            QuickLogger.Info("Started patching. Version: " + QuickLogger.GetAssemblyVersion());

#if DEBUG
            QuickLogger.DebugLogsEnabled = true;
            QuickLogger.Debug("Debug logs enabled");
#endif

            try
            {
                FEHolderBuildable.PatchHelper();

                var harmony = HarmonyInstance.Create("com.fireextinguishergholder.MAC");

                harmony.PatchAll(Assembly.GetExecutingAssembly());

                QuickLogger.Info("Finished patching");
            }
            catch (Exception ex)
            {
                QuickLogger.Error(ex);
            }
        }
    }
}

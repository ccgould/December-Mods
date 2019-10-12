using Common.Utilities;
using Harmony;
using MAC.SaveVehicleLightState.Mod;

namespace MAC.SaveVehicleLightState.Patches
{
    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("Start")]
    internal class Exosuit_Awake_Patcher
    {
        public static void Postfix(ref Exosuit __instance)
        {
            //if (!Mod.Configuration.Config.Enabled) return;
            var id = __instance?.GetComponent<PrefabIdentifier>().Id;

            if (id == null)
            {
                QuickLogger.Info($"{__instance?.name} doesn't have a prefabid");
                return;
            }

            var toggleLights = __instance.gameObject.GetComponent<ToggleLights>();

            if (toggleLights != null)
            {
                if (__instance.gameObject.GetComponent<VehicleManager>() == null)
                {
                    var vm = __instance.gameObject.AddComponent<VehicleManager>();
                    vm.Initialize(id, toggleLights);
                }
            }
            else
            {
                QuickLogger.Info($"No Toggle Lights found on Exosuit.");
            }
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("SubConstructionComplete")]
    internal class Exosuit_SubConstructionComplete_Patcher
    {
        public static void Postfix(ref Exosuit __instance)
        {
            var id = __instance?.GetComponent<PrefabIdentifier>().Id;

            if (id == null)
            {
                QuickLogger.Info($"{__instance?.name} doesn't have a prefabid");
                return;
            }

            var vm = __instance.gameObject.GetComponent<VehicleManager>();

            if (vm != null)
            {
                var toggleLights = __instance.gameObject.GetComponent<ToggleLights>();
                toggleLights.lightsActive = true;
                vm.Initialize(id, toggleLights);
            }
            else
            {
                QuickLogger.Info($"On Sub Construction Complete Vehicle Manager not found.");
            }
        }
    }

    [HarmonyPatch(typeof(ToggleLights))]
    [HarmonyPatch("OnPoweredChanged")]
    internal class Exosuit_OnPoweredChanged_Patcher
    {
        private static void Postfix(ref ToggleLights __instance, ref bool powered)
        {
            QuickLogger.Info($"Loading {Configuration.Mod.ModName}");
            QuickLogger.Info($"Get Prefab ID from GameObject {__instance?.name}");
            var prefabIdentifier = __instance.GetComponentInParent<PrefabIdentifier>();

            if (prefabIdentifier == null)
            {
                QuickLogger.Error($"No PrefabIdentifier found on {__instance?.name}");
                return;
            }

            var manager = __instance.GetComponentInParent<VehicleManager>();

            if (manager == null)
            {
                QuickLogger.Info($"No Manager found on {__instance?.name}");
                return;
            }

            var id = prefabIdentifier?.Id ?? string.Empty;

            var data = Configuration.Mod.GetSaveData(id);

            if (data == null)
            {
                QuickLogger.Info($"No save found for PrefabId {id}");
                return;
            }

            manager.SetToggle(data.Value);

            QuickLogger.Info($"Loaded {Configuration.Mod.ModName} for PrefabId {id}");
        }
    }
}

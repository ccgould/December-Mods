using Common.Utilities;
using Harmony;
using MAC.SaveVehicleLightState.Mod;

namespace MAC.SaveVehicleLightState.Patches
{
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Awake")]
    internal class Seamoth_Awake_Patcher
    {
        public static void Postfix(ref SeaMoth __instance)
        {
            //if (!Mod.Configuration.Config.Enabled) return;
            var id = __instance?.GetComponent<PrefabIdentifier>().Id;

            if (id == null)
            {
                QuickLogger.Info($"{__instance?.name} doesn't have a prefabid");
                return;
            }

            if (__instance.gameObject.GetComponent<VehicleManager>() == null)
            {
                var vm = __instance.gameObject.AddComponent<VehicleManager>();
                vm.Initialize(id, __instance.toggleLights);
            }


            //VehicleManager.AddVehicle(id, __instance.toggleLights.lightsActive);
        }
    }

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("SubConstructionComplete")]
    internal class Seamoth_SubConstructionComplete_Patcher
    {
        public static void Postfix(ref SeaMoth __instance)
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
                __instance.toggleLights.lightsActive = true;
                vm.Initialize(id, __instance.toggleLights);
            }
            else
            {
                QuickLogger.Info($"On Sub Construction Complete Vehicle Manager not found.");
            }
        }
    }

    [HarmonyPatch(typeof(ToggleLights))]
    [HarmonyPatch("OnPoweredChanged")]
    internal class Seamoth_OnPoweredChanged_Patcher
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
                QuickLogger.Error($"No Manager found on {__instance?.name}");
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

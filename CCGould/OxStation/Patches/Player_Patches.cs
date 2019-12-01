using Common.Utilities;
using Harmony;
using MAC.OxStation.Managers;
using MAC.OxStation.Mono;
using UnityEngine;

namespace MAC.OxStation.Patches
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("CanBreathe")]
    internal class Player_Patches
    {
        private static readonly float _oxygenPerSecond = 10f;
        public static bool Prefix(ref Player __instance, ref bool __result)
        {
            bool canBreathe = false;

            if (!__instance.IsInBase() || __instance.IsUnderwater()) return true;

            QuickLogger.Debug($"Is InBase {__instance.IsInBase()}", true);

            var curBase = __instance.GetCurrentSub();

            var manager = BaseManager.FindManager(curBase);

            var unitsAvailable = manager.BaseUnits.Count > 0;

            if (!unitsAvailable)
            {
                QuickLogger.Debug($"Can Breathe {canBreathe}", true);
                __result = canBreathe;
                return false;
            }

            if (__instance.oxygenMgr.GetOxygenAvailable() >= __instance.oxygenMgr.GetOxygenCapacity())
            {
                QuickLogger.Debug($"Can Breathe Check 2 {canBreathe}", true);
                __result = canBreathe;
                return false;
            }

            foreach (OxStationController baseUnit in manager.BaseUnits)
            {
                if (baseUnit.OxygenManager.GetO2Level() <= 0 || !baseUnit.IsConstructed || baseUnit.HealthManager.IsDamageApplied()) continue;

                if (Player.main.oxygenMgr.GetOxygenAvailable() < Player.main.oxygenMgr.GetOxygenCapacity())
                {
                    var amount = _oxygenPerSecond * Time.deltaTime;
                    var result = baseUnit.OxygenManager.RemoveOxygen(amount);

                    if (result)
                    {
                        Player.main.oxygenMgr.AddOxygen(amount);
                    }
                    canBreathe = true;
                }
                break;
            }
            QuickLogger.Debug($"Can Breathe Check 2 {canBreathe}", true);
            __result = canBreathe;
            return false;
        }
    }
}

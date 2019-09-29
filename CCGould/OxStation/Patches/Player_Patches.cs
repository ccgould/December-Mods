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
        public static void Postfix(ref Player __instance)
        {
            if (!__instance.IsInBase()) return;

            var curBase = __instance.GetCurrentSub();

            if (curBase == null || curBase.isCyclops) return;

            var manager = BaseManager.FindManager(curBase);

            var unitsAvailable = manager.BaseUnits.Count > 0;

            if (!unitsAvailable) return;

            foreach (OxStationController baseUnit in manager.BaseUnits)
            {
                if (baseUnit.OxygenManager.GetO2Level() <= 0 || !baseUnit.IsConstructed || baseUnit.HealthManager.IsDamageApplied()) continue;

                if (Player.main.oxygenMgr.GetOxygenAvailable() < Player.main.oxygenMgr.GetOxygenCapacity())
                {
                    var amount = _oxygenPerSecond * Time.deltaTime;
                    Player.main.oxygenMgr.AddOxygen(amount);
                    baseUnit.OxygenManager.RemoveOxygen(amount);
                }
                break;
            }
        }
    }
}

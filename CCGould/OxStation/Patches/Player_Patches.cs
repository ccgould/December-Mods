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
        public static bool Prefix(ref Player __instance)
        {
            bool o2Available = false;

            if (__instance.IsInBase())
            {
                var curBase = __instance.GetCurrentSub();

                if (curBase != null)
                {
                    var manager = BaseManager.FindManager(curBase);

                    var unitsAvailable = manager.BaseUnits.Count > 0;

                    if (unitsAvailable)
                    {
                        foreach (OxStationController baseUnit in manager.BaseUnits)
                        {
                            if (baseUnit.OxygenManager.GetO2Level() <= 0) continue;

                            if (Player.main.oxygenMgr.GetOxygenAvailable() < Player.main.oxygenMgr.GetOxygenCapacity())
                            {
                                var amount = _oxygenPerSecond * Time.deltaTime;
                                Player.main.oxygenMgr.AddOxygen(amount);
                                baseUnit.OxygenManager.RemoveOxygen(amount);
                            }
                            o2Available = true;
                            break;
                        }
                    }

                    QuickLogger.Debug($"Can Breath: {o2Available}");
                    return o2Available;
                }
            }

            return o2Available;
        }
    }
}

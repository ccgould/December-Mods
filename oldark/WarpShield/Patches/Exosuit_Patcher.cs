using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Harmony;

namespace MAC.WarpShield {
    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("OnUpgradeModuleChange")]
    public class Exosuit_OnUpgradeModuleChange_Patch {

        [HarmonyPostfix]
        static void PostFix(Exosuit __instance, TechType techType)
        {
            int count = __instance.modules.GetCount(ExoSuitModule.ExoSuitWarpShieldModule);
            if (count > 0)
            {
                MainPatcher.isPrawnWarpShielded = true;
            }
            else
            {
                MainPatcher.isPrawnWarpShielded = false;
            }
        }
    }
}

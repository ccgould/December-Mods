using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Harmony;

namespace MAC.WarpShield {
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("OnUpgradeModuleChange")]
    public class Seamoth_OnUpgradeModuleChange_Patch {

        [HarmonyPostfix]
        static void PostFix(SeaMoth __instance, TechType techType)
        {
            int count = __instance.modules.GetCount(SeamothModule.SeamothWarpShieldModule);
            if(count > 0)
            {
                MainPatcher.isSeamothWarpShielded = true;
            }
            else
            {
                MainPatcher.isSeamothWarpShielded = false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace MAC.WarpShield {
    
    [HarmonyPatch(typeof(WarpBall))]
    [HarmonyPatch("Warp")]
    internal class WarpBall_Warp_Patch {
        [HarmonyPrefix]
        public static bool Prefix(WarpBall __instance)
        {
            return true;
        }
    }
}

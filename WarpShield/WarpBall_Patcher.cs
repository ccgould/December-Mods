using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace MAC.WarpShield {
    
    [HarmonyPatch(typeof(WarpBall))]
    [HarmonyPatch("Warp")]
    internal class WarpBall_Warp_Patch {
        [HarmonyPrefix]
        public static bool Prefix(WarpBall __instance, ref GameObject target, ref Vector3 position)
        {
            return true;
        }
    }
}

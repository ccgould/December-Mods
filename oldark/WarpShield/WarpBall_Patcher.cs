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
        public static bool Prefix(WarpBall __instance, GameObject target, Vector3 position)
        {
            Player component = target.GetComponent<Player>();
            if(component != null && component.GetMode() == Player.Mode.LockedPiloting && component.GetVehicle() != null)
            {
                return false;
            }
            return true;
        }
    }
}
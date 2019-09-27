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

            if (MainPatcher.isSeamothWarpShielded && component != null && component.GetMode() == Player.Mode.LockedPiloting && component.GetVehicle() != null && component.GetVehicle().vehicleName.Equals("SEAMOTH"))
            {
                return false;
            }
            else if (MainPatcher.isPrawnWarpShielded && component != null && component.GetMode() == Player.Mode.LockedPiloting && component.GetVehicle() != null && component.GetVehicle().vehicleName.Equals("PRAWN SUIT"))
            {
                 return false;
            }

            return true;
        }
    }
}
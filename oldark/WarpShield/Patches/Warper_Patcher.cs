using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Harmony;

namespace MAC.WarpShield{
    [HarmonyPatch(typeof(Warper))]
    [HarmonyPatch("OnKill")]
    public class Warper_OnKill_Patch {

        [HarmonyPrefix]
        public static bool Prefix()
        {
            KnownTech.Add(WarpShieldModule.TechTypeID);
            return true;
        }
    }
}

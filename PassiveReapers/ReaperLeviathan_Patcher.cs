using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Harmony;
using System.Reflection;

namespace MAC.PassiveReapers {
    [HarmonyPatch(typeof(ReaperLeviathan))]
    [HarmonyPatch("Update")]
    internal class ReaperLeviathan_Update_Patch {
        [HarmonyPostfix]
        public static void Postfix(ReaperLeviathan __instance)
        {
            __instance.Aggression.Value = 0.0f;
        }
    }

    //[HarmonyPatch(typeof(ReaperLeviathan))]
    //[HarmonyPatch("GrabSeamoth")]
    //internal class ReaperLeviathan_GrabSeamoth_Patch {
    //    [HarmonyPrefix]
    //    public static bool Prefix(ReaperLeviathan __instance)
    //    {
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(ReaperLeviathan))]
    //[HarmonyPatch("GrabExosuit")]
    //internal class ReaperLeviathan_GrabExosuit_Patch {
    //    [HarmonyPrefix]
    //    public static bool Prefix(ReaperLeviathan __instance)
    //    {
    //        return false;
    //    }
    //}

}
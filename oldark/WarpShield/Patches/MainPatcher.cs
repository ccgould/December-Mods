using Harmony;
using System.Reflection;
using SMLHelper.V2.Handlers;
using UnityEngine;
using System;

namespace MAC.WarpShield {
    public class MainPatcher {

        public static bool isSeamothWarpShielded = false;
        public static bool isPrawnWarpShielded = false;
        public static bool isWarperSlain = false;

        public static void Patch()
        {
            ExoSuitWarpShieldModule.PatchSMLHelper();
            SeamothWarpShieldModule.PatchSMLHelper();

            var harmony = HarmonyInstance.Create("com.oldark.subnautica.warpshield.mod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());


        }
    }
}
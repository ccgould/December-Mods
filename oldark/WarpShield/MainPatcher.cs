using Harmony;
using System.Reflection;
using SMLHelper.V2.Handlers;
using UnityEngine;
using System;

namespace MAC.WarpShield {
    public class MainPatcher {

        public static bool isSeamothWarpShielded = false;

        public static void Patch()
        {
            var harmony = HarmonyInstance.Create("com.oldark.subnautica.warpshield.mod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            PrefabHandler.RegisterPrefab(new SeamothWarpShieldModule());
        }
    }
}
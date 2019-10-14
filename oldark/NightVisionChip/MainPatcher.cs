using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace MAC.NightVisionChip
{
    public class MainPatcher
    {
        public static float ambientIntensity;
        public static Color ambientLight;

        public static void Patch()
        {
            NightVisionChip.PatchSMLHelper();
            var harmony = HarmonyInstance.Create("com.oldark.subnautica.nightvisionchip.mod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}

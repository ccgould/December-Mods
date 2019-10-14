using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace MAC.NightVisionChip {

    [HarmonyPatch(typeof(Equipment))]
    [HarmonyPatch("UpdateCount")]
    internal class Equipment_UpdateCount_Patcher {
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (MainPatcher.ambientLight == null)
            {
                MainPatcher.ambientIntensity = RenderSettings.ambientIntensity;
                MainPatcher.ambientLight = RenderSettings.ambientLight;
            }

            if (Inventory.main.equipment.GetCount(NightVisionChip.TechTypeID) <= 0)
            {

                RenderSettings.ambientIntensity = MainPatcher.ambientIntensity;
                RenderSettings.ambientLight = MainPatcher.ambientLight;
            }
            else
            {
                RenderSettings.ambientIntensity = 100f;
                RenderSettings.ambientLight = Color.green;
            }
        }
    }
}

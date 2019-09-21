using Harmony;
using UnityEngine;

namespace MAC.Senna.Fahrenheit
{
    [HarmonyPatch(typeof(Vehicle))]
    [HarmonyPatch("GetTemperature")]
    public class Vehicle_GetTemperature_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Vehicle __instance, ref float __result)
        {
            WaterTemperatureSimulation main = WaterTemperatureSimulation.main;
            __result = (!(main != null)) ? 0f : main.GetTemperature(__instance.transform.position) * 1.8f + 32;
            return false;
        }
    }

    [HarmonyPatch(typeof(SubRoot))]
    [HarmonyPatch("GetTemperature")]
    public class SubRoot_GetTemperature_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(SubRoot __instance, ref float __result)
        {
            WaterTemperatureSimulation main = WaterTemperatureSimulation.main;
            __result = (!(main != null)) ? 0f : main.GetTemperature(__instance.transform.position) * 1.8f + 32;
            return false;
        }
    }

    [HarmonyPatch(typeof(uGUI_SeamothHUD))]
    [HarmonyPatch("Update")]
    public class uGUI_SeamothHUD_Update_Patch
    {
        private static Color textColor = new Color32(255, 220, 0, 255);

        [HarmonyPostfix]
        public static void Postfix(uGUI_SeamothHUD __instance)
        {
            Player main = Player.main;

            if (main != null && main.inSeamoth)
            {                                   
                __instance.textTemperatureSuffix.color = textColor;
                __instance.textTemperatureSuffix.text = "\u00b0F";                
            }
        }
    }

    [HarmonyPatch(typeof(uGUI_ExosuitHUD))]
    [HarmonyPatch("Update")]
    public class uGUI_ExosuitHUD_Update_Patch
    {
        private static Color textColor = new Color32(255, 220, 0, 255);

        [HarmonyPostfix]
        public static void Postfix(uGUI_ExosuitHUD __instance)
        {
            Player main = Player.main;

            if (main != null && main.inExosuit)
            {
                __instance.textTemperatureSuffix.color = textColor;
                __instance.textTemperatureSuffix.text = "\u00b0F";                
            }
        }
    }

    [HarmonyPatch(typeof(ThermalPlant))]
    [HarmonyPatch("UpdateUI")]
    public class ThermalPlant_UpdateUI_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(ThermalPlant __instance)
        {
            float fahrenheit = __instance.temperature * 1.8f + 32;

            __instance.temperatureText.text = $"{string.Format("{0:0}", fahrenheit)}\u00b0F";

            float value = fahrenheit / 212f;

            Material material = __instance.temperatureBar.material;

            material.SetFloat(ShaderPropertyID._Amount, value);

            if (fahrenheit < 77f)
            {
                __instance.temperatureBar.color = Color.red;
                __instance.temperatureText.color = Color.red;
            }
            else
            {
                float num = (fahrenheit - 77f) / 167f;

                Color color;

                if (num < 0.3f)
                {
                    color = UWE.Utils.LerpColor(Color.red, Color.yellow, num / 0.4f);
                }
                else
                {
                    color = UWE.Utils.LerpColor(Color.yellow, Color.green, (num - 0.4f) / 0.6f);
                }

                __instance.temperatureBar.color = color;
                __instance.temperatureText.color = color;
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(ThermalPlant))]
    [HarmonyPatch("AddPower")]
    public class ThermalPlant_AddPower_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(ThermalPlant __instance)
        {
            float fahrenheit = __instance.temperature * 1.8f + 32;

            if (__instance.constructable.constructed && fahrenheit > 77f)
            {
                float num = 2f * DayNightCycle.main.dayNightSpeed;

                float num2 = 1.6500001f * num * Mathf.Clamp01(Mathf.InverseLerp(77f, 212f, fahrenheit));

                UWE.Utils.Assert(num2 >= 0f, "ThermalPlant must produce positive amounts", __instance);

                __instance.powerSource.AddEnergy(num2, out float num3);
            }

            return false;
        }
    }
}


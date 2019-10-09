using AlexejheroYTB.Common;
using Harmony;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Options;
using UnityEngine;

namespace MAC.GlobalSeaglideSettings
{
    public static class Mod
    {
        public static void Patch()
        {
            HarmonyInstance.Create("MAC.GlobalSeaglideSettings").PatchAll();
            OptionsPanelHandler.RegisterModOptions(new Options());
        }
    }

    public class Options : ModOptions
    {
        public static bool MapOn
        {
            get => PlayerPrefs.GetInt("GSSMAP", 0).ToBool();
            set => PlayerPrefs.SetInt("GSSMAP", value.ToInt());
        }

        public static bool FlashlightOn
        {
            get => PlayerPrefs.GetInt("GSSFLASH", 0).ToBool();
            set => PlayerPrefs.SetInt("GSSFLASH", value.ToInt());
        }

        public Options() : base("Global Seaglide Settings")
        {
            ToggleChanged += OnToggleChanged;
        }

        public void OnToggleChanged(object sender, ToggleChangedEventArgs e)
        {
            if (e.Id == "GSS.EnableMap") MapOn = e.Value;
            if (e.Id == "GSS.EnableFlashlight") FlashlightOn = e.Value;
        }

        public override void BuildModOptions()
        {
            AddToggleOption("GSS.EnableMap", "Enable Map", MapOn);
            AddToggleOption("GSS.EnableFlashlight", "Enable Flashlight", FlashlightOn);
        }
    }

    public static class Patches
    {

    }
}

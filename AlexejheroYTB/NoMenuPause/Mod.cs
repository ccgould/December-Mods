using AlexejheroYTB.Common;
using Harmony;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Options;
using UnityEngine;
using UWE;

namespace MAC.NoMenuPause
{
    public static class Mod
    {
        public static void Patch()
        {
            HarmonyInstance.Create("NoMenuPause").PatchAll();
            OptionsPanelHandler.RegisterModOptions(new Options());
        }
    }

    public class Options : ModOptions
    {
        public static bool Off
        {
            get => PlayerPrefs.GetInt("NPM", 0).ToBool();
            set => PlayerPrefs.SetInt("NPM", value.ToInt());
        }

        public Options() : base("No Menu Pause")
        {
            ToggleChanged += OnToggleChanged;
        }

        private void OnToggleChanged(object sender, ToggleChangedEventArgs e)
        {
            if (e.Id == "NPM") Off = e.Value;
        }

        public override void BuildModOptions()
        {
            AddToggleOption("NPM", "Pause while menu is open", Off);
        }
    }

    [HarmonyPatch(typeof(FreezeTime))]
    [HarmonyPatch("Begin")]
    public static class FreezeTime_Begin_Patch
    {
        public static bool Prefix(string userId)
        {
            if (userId == "IngameMenu" && !Options.Off) return false;
            return true;
        }
    }
}

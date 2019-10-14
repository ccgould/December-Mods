using Harmony;
using UWE;

namespace MAC.NoMenuPause
{
    public static class Mod
    {
        public static void Patch()
        {
            HarmonyInstance.Create("NoMenuPause").PatchAll();
        }
    }

    [HarmonyPatch(typeof(FreezeTime))]
    [HarmonyPatch("Begin")]
    public static class FreezeTime_Begin_Patch
    {
        public static bool Prefix(string userId)
        {
            if (userId == "IngameMenu") return false;
            return true;
        }
    }
}

using Harmony;

namespace MAC.ConfigurableOutcropCount
{
    public static class Mod
    {
        public static void Patch()
        {
            HarmonyInstance.Create("ConfigurableOutcropCount").PatchAll();
        }
    }

    
}

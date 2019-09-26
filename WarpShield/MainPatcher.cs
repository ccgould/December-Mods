using Harmony;
using System.Reflection;

namespace MAC.WarpShield {
    public class MainPatcher {
        public static void Patch()
        {
            var harmony = HarmonyInstance.Create("com.oldark.subnautica.warpshield.mod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
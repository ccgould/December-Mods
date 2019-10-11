using Common.Utilities;
using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MAC.DropAllOnDeath
{
    public class QPatch
    {
        public static void Patch()
        {
            QuickLogger.Info($"Started patching. Version: {QuickLogger.GetAssemblyVersion(Assembly.GetExecutingAssembly())}");

#if DEBUG
            QuickLogger.DebugLogsEnabled = true;
            QuickLogger.Debug("Debug logs enabled");
#endif

            try
            {

                HarmonyInstance.Create("com.dropallondeath.MAC").PatchAll(Assembly.GetExecutingAssembly());

                QuickLogger.Info("Finished patching");
            }
            catch (Exception ex)
            {
                QuickLogger.Error(ex);
            }
        }
    }

    [HarmonyPatch(typeof(Inventory))]
    [HarmonyPatch("LoseItems")]
    internal class Inventory_Patcher
    {
        public static void Postfix(ref Inventory __instance)
        {
            List<InventoryItem> list = new List<InventoryItem>();

            foreach (InventoryItem inventoryItem in Inventory.main.container)
            {
                list.Add(inventoryItem);
            }
            foreach (InventoryItem inventoryItem2 in ((IItemsContainer)Inventory.main.equipment))
            {
                list.Add(inventoryItem2);
            }

            for (int i = 0; i < list.Count; i++)
            {
                __instance.InternalDropItem(list[i].item, true);
            }
        }
    }
}

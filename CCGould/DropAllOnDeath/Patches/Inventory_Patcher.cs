﻿using Harmony;
using MAC.DropAllOnDeath.Config;
using System.Collections.Generic;

namespace MAC.DropAllOnDeath.Patches
{
    [HarmonyPatch(typeof(Inventory))]
    [HarmonyPatch("LoseItems")]
    internal class Inventory_Patcher
    {
        public static void Postfix(ref Inventory __instance)
        {
            if (!Mod.Configuration.Config.Enabled) return;

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

using AlexejheroYTB.Common;
using Harmony;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using Story;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MAC.PocketRadio
{
    public static class Mod
    {
        public static void Patch()
        {
            HarmonyInstance.Create("MAC.PocketRadio").PatchAll();
            new PocketRadioItem().Patch();
        }
    }

    public class PocketRadioItem : Craftable
    {
        public new void Patch()
        {
            base.Patch();

            KnownTechHandler.SetAnalysisTechEntry(TechType.Exosuit, new TechType[] { this.TechType });
            SpriteHandler.RegisterSprite(this.TechType, SpriteManager.Get(TechType.Radio));

            ItemActionHelper.RegisterAction(MouseButton.Left, this.TechType, PocketRadio.OnClick, "Play message", PocketRadio.Condition);
        }

        public PocketRadioItem() : base("PocketRadio", "Pocket Radio", "Receive short-range communications and play them wirelessly.") { }

        protected override TechData GetBlueprintRecipe() => new TechData() { Ingredients = new List<Ingredient>() { new Ingredient(TechType.Titanium, 2), new Ingredient(TechType.Copper, 1), new Ingredient(TechType.CopperWire, 2), new Ingredient(TechType.Battery, 1) } };
        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        public override string[] StepsToFabricatorTab => "Personal/Tools".Split('/');

        public override TechGroup GroupForPDA => TechGroup.Personal;
        public override TechCategory CategoryForPDA => TechCategory.Tools;

        public override string AssetsFolder => Path.Combine(new DirectoryInfo(Path.Combine(Assembly.GetExecutingAssembly().Location, "..")).Name, "Assets");

        public override GameObject GetGameObject()
        {
            GameObject prefab = Resources.Load<GameObject>("worldentities/tools/battery");
            GameObject obj = GameObject.Instantiate(prefab);

            Pickupable pickupable = obj.GetComponent<Pickupable>();
            pickupable.destroyOnDeath = false;

            obj.AddComponent<PocketRadio>();

            GameObject.DestroyImmediate(obj.GetComponent<Battery>());

            return obj;
        }
    }

    public class PocketRadio : Radio
    {
        public static bool Condition(InventoryItem item)
        {
            return (bool)AccessTools.Field(typeof(Radio), "hasMessage").GetValue(item.item.GetComponent<PocketRadio>());
        }

        public static void OnClick(InventoryItem item)
        {
            item.item.GetComponent<PocketRadio>().OnHandClick(null);
        }
    }

    public static class Patches
    {
        [HarmonyPatch(typeof(Radio))]
        [HarmonyPatch("IsFullHealth")]
        public static class Radio_IsFullHealth
        {
            [HarmonyPrefix]
            public static bool Prefix(Radio __instance, ref bool __result)
            {
                if (__instance.GetComponent<PocketRadio>())
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(Radio))]
        [HarmonyPatch("OnHandClick")]
        public static class Radio_OnHandClick
        {
            [HarmonyPrefix]
            public static bool Prefix(Radio __instance)
            {
                if (__instance.GetComponent<PocketRadio>())
                {
                    if (!(bool)AccessTools.Field(typeof(PocketRadio), "hasMessage").GetValue(__instance) && !__instance.IsInvoking("PlayRadioMessage"))
                    {
                        __instance.playSound.Play();
                        __instance.Invoke("PlayRadioMessage", 1.25f);
                        var cancelIconEvent = AccessTools.Field(typeof(PocketRadio), "CancelIconEvent").GetValue(__instance) as Radio.CancelIcon;
                        cancelIconEvent?.Invoke();
                    }
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(Radio))]
        [HarmonyPatch("OnHandHover")]
        public static class Radio_OnHandHover
        {
            [HarmonyPrefix]
            public static bool Prefix(Radio __instance)
            {
                if (__instance.GetComponent<PocketRadio>()) return false;
                return true;
            }
        }

        [HarmonyPatch(typeof(Radio))]
        [HarmonyPatch("OnRepair")]
        public static class Radio_OnRepair
        {
            [HarmonyPrefix]
            public static bool Prefix(Radio __instance)
            {
                if (__instance.GetComponent<PocketRadio>()) return false;
                return true;
            }
        }

        [HarmonyPatch(typeof(Radio))]
        [HarmonyPatch("PlayRadioRepairVO")]
        public static class Radio_PlayRadioRepairVO
        {
            [HarmonyPrefix]
            public static bool Prefix(Radio __instance)
            {
                if (__instance.GetComponent<PocketRadio>()) return false;
                return true;
            }
        }

        [HarmonyPatch(typeof(Radio))]
        [HarmonyPatch("ToggleBlink")]
        public static class Radio_ToggleBlink
        {
            [HarmonyPrefix]
            public static bool Prefix(Radio __instance, bool on)
            {
                if (__instance.GetComponent<PocketRadio>())
                {
                    //if (on)
                    //{
                    //    __instance.radioSound.Play();
                    //}
                    //else
                    //{
                    //    __instance.radioSound.Stop();
                    //}
                    return false;
                }
                return true;
            }
        }
    }
}

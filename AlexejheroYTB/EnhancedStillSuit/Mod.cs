using Harmony;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MAC.EnhancedStillSuit
{
    public static class Mod
    {
        public static void Patch()
        {
            HarmonyInstance.Create("EnhancedStillSuit").PatchAll();
            new ESS().Patch();
        }
    }

    public class ESS : Craftable
    {
        public ESS() : base("enhancedstillsuit", "Enhanced Stillsuit", "Just like a normal stillsuit, but it automatically drinks the generated reclaimed water.")
        {
            OnFinishedPatching += OnPatchFinished;
        }

        public void OnPatchFinished()
        {
            CraftDataHandler.SetItemSize(this.TechType, 2, 2);
            CraftDataHandler.SetEquipmentType(this.TechType, EquipmentType.Body);
            SpriteHandler.RegisterSprite(this.TechType, SpriteManager.Get(TechType.Stillsuit));
        }

        protected override TechData GetBlueprintRecipe() => new TechData()
        {
            craftAmount = 1,
            Ingredients = new List<Ingredient>()
            {
                new Ingredient(TechType.Stillsuit, 1),
                new Ingredient(TechType.ComputerChip, 1),
                new Ingredient(TechType.CopperWire, 2),
                new Ingredient(TechType.Silver, 1),
            },
        };
        public override CraftTree.Type FabricatorType => CraftTree.Type.Workbench;
        public override string[] StepsToFabricatorTab => new string[0];

        public override TechCategory CategoryForPDA => TechCategory.Equipment;
        public override TechGroup GroupForPDA => TechGroup.Personal;

        public override string AssetsFolder => Path.Combine(new DirectoryInfo(Path.Combine(Assembly.GetExecutingAssembly().Location, "..")).Name, "Assets");

        public override TechType RequiredForUnlock => TechType.Stillsuit;

        public override GameObject GetGameObject()
        {
            GameObject prefab = CraftData.GetPrefabForTechType(TechType.Stillsuit);
            GameObject obj = GameObject.Instantiate(prefab);

            obj.AddComponent<ESSBehaviour>();

            return obj;
        }
    }

    public class ESSBehaviour : MonoBehaviour { }

    [HarmonyPatch(typeof(Stillsuit))]
    [HarmonyPatch("UpdateEquipped")]
    public static class StillSuit_UpdateEquipped
    {
        [HarmonyPrefix]
        public static bool Prefix(Stillsuit __instance)
        {
            if (!__instance.GetComponent<ESSBehaviour>()) return true;

            if (GameModeUtils.RequiresSurvival() && !Player.main.GetComponent<Survival>().freezeStats)
            {
                float num = Time.deltaTime / 1800f * 100f;
                __instance.waterCaptured += num * 0.75f;
                if (__instance.waterCaptured >= __instance.waterPrefab.waterValue)
                {
                    ErrorMessage.AddDebug("Enhanced Stillsuit activated!");

                    GameObject gameObject = GameObject.Instantiate(__instance.waterPrefab.gameObject);
                    Utils.Assert(gameObject != null, "see log", null);
                    Pickupable component = gameObject.GetComponent<Pickupable>();
                    Utils.Assert(component != null, "see log", null);
                    Player.main.GetComponent<Survival>().Eat(component.gameObject);
                    __instance.waterCaptured -= __instance.waterPrefab.waterValue;
                }
            }

            return false;
        }
    }
}

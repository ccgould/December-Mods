using Harmony;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MAC.HeadLamp
{
    public static class Mod
    {
        public static void Patch()
        {
            HarmonyInstance.Create("EnhancedStillSuit").PatchAll();
            new HeadLampItem().Patch();
        }
    }

    public class HeadLampItem : Craftable
    {
        public HeadLampItem() : base("HeadLamp", "Head Lamp", "A flashlight that you can wear on your head. Recharges with solar energy, and does not need any batteries. Can be toggled on/off with X (configurable option)")
        {
            OnFinishedPatching += OnPatchFinished;
        }

        public void OnPatchFinished()
        {
            CraftDataHandler.SetItemSize(this.TechType, 2, 2);
            CraftDataHandler.SetEquipmentType(this.TechType, EquipmentType.Head);
            SpriteHandler.RegisterSprite(this.TechType, SpriteManager.Get(TechType.Flashlight));
        }

        protected override TechData GetBlueprintRecipe() => new TechData()
        {
            craftAmount = 1,
            Ingredients = new List<Ingredient>()
            {
                new Ingredient(TechType.Flashlight, 1),
                new Ingredient(TechType.ComputerChip, 1),
                new Ingredient(TechType.Battery, 1),
                new Ingredient(TechType.Quartz, 3),
                new Ingredient(TechType.Silicone, 2),
            },
        };
        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        public override string[] StepsToFabricatorTab => "Personal/Equipment".Split('/');

        public override TechCategory CategoryForPDA => TechCategory.Equipment;
        public override TechGroup GroupForPDA => TechGroup.Personal;

        public override string AssetsFolder => Path.Combine(new DirectoryInfo(Path.Combine(Assembly.GetExecutingAssembly().Location, "..")).Name, "Assets");

        public override GameObject GetGameObject()
        {
            GameObject prefab = CraftData.GetPrefabForTechType(TechType.Flashlight);
            GameObject obj = GameObject.Instantiate(prefab);

            HeadLampComponent lamp = obj.AddComponent<HeadLampComponent>();
            lamp.lights = obj.GetComponent<FlashLight>().toggleLights;
            lamp.id = HeadLampComponent.CreateString(15);

            GameObject.DestroyImmediate(obj.GetComponent<FlashLight>());

            return obj;
        }
    }

    public class HeadLampComponent : MonoBehaviour
    {
        public ToggleLights lights;
        public string id;

        public void Update()
        {
            lights.SetLightsActive(Inventory.main.equipment.GetItemInSlot("Slot").item.GetComponent<HeadLampComponent>().id == id);
        }

        public static System.Random rd = new System.Random();
        public static string CreateString(int stringLength)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}

using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MAC.ScubaManifold
{
    public static class Mod
    {
        public static void Patch()
        {
            new ScubaManifoldItem().Patch();
        }
    }

    public class ScubaManifoldItem : Craftable
    {
        public new void Patch()
        {
            base.Patch();

            CraftDataHandler.SetEquipmentType(this.TechType, EquipmentType.Tank);
            CraftDataHandler.SetBackgroundType(this.TechType, CraftData.BackgroundType.ExosuitArm);
            CraftDataHandler.SetCraftingTime(this.TechType, 10);
            CraftDataHandler.SetItemSize(this.TechType, 4, 4);
            KnownTechHandler.SetAnalysisTechEntry(TechType.Exosuit, new TechType[] { this.TechType });

            ScubaManifold.techType = this.TechType;
        }

        public ScubaManifoldItem() : base("ScubaManifold", "Scuba Manifold", "Combines the oxygen supply of all carried tanks") { }

        protected override TechData GetBlueprintRecipe() => new TechData() { Ingredients = new List<Ingredient>() { new Ingredient(TechType.Silicone, 1), new Ingredient(TechType.PlasteelIngot, 1), new Ingredient(TechType.Kyanite, 2)} };
        public override CraftTree.Type FabricatorType => CraftTree.Type.Workbench;
        public override string[] StepsToFabricatorTab => new string[] { "TankMenu" };

        public override TechGroup GroupForPDA => TechGroup.Personal;
        public override TechCategory CategoryForPDA => TechCategory.Equipment;

        public override string AssetsFolder => Path.Combine(new DirectoryInfo(Path.Combine(Assembly.GetExecutingAssembly().Location, "..")).Name, "Assets");
        public override string IconFileName => "ScubaManifold.png";

        public override GameObject GetGameObject()
        {
            GameObject prefab = Resources.Load<GameObject>("worldentities/tools/tank");
            GameObject obj = GameObject.Instantiate(prefab);

            Pickupable pickupable = obj.GetComponent<Pickupable>();
            pickupable.destroyOnDeath = false;

            ScubaManifold scuba = Player.mainObject.GetComponent<ScubaManifold>() ?? Player.mainObject.AddComponent<ScubaManifold>();

            GameObject.DestroyImmediate(obj.GetComponent<Oxygen>());

            return obj;
        }
    }

    public class ScubaManifold : MonoBehaviour
    {
        public static ScubaManifold main;
        public static TechType techType;

        public void Awake()
        {
            if (!main) main = this;
            else
            {
                Destroy(this);
                Console.WriteLine("[ScubaManifold] [ERROR] ScubaManifold component must be a singleton!");
            }
        }

        public void Update()
        {
            List<InventoryItem> items = new List<InventoryItem>();
            Inventory.main.container.GetItemTypes().ForEach(type => items.AddRange(Inventory.main.container.GetItems(type)));
            List<Oxygen> sources = items.Where(item => item.item.gameObject.GetComponent<Oxygen>() != null).Select(item => item.item.gameObject.GetComponent<Oxygen>()).ToList();

            if (Inventory.main.equipment.GetItemInSlot("Tank")?.item?.GetTechType() == ScubaManifold.techType)
            {
                ErrorMessage.AddDebug("Equipped!");
                sources.ForEach(source =>
                {
                    Player.main.oxygenMgr.RegisterSource(source);
                });
            }
            else
            {
                sources.ForEach(source =>
                {
                    Player.main.oxygenMgr.UnregisterSource(source);
                });
            }
        }
    }
}

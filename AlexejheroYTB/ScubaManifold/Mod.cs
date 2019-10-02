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
        }

        public ScubaManifoldItem() : base("ScubaManifold", "Scuba Manifold", "Combines the oxygen supply of all carried tanks") { }

        protected override TechData GetBlueprintRecipe() => new TechData() { craftAmount = 1 };
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

            ScubaManifold scuba = obj.AddComponent<ScubaManifold>();
            scuba.TechType = this.TechType;

            return obj;
        }
    }

    public class ScubaManifold : MonoBehaviour
    {
        public TechType TechType;

        public void Update()
        {
            if (Inventory.main.equipment.GetItemInSlot("Tank").item.GetComponent<ScubaManifold>() == this)
            {
                List<InventoryItem> items = new List<InventoryItem>();

                Inventory.main.container.GetItemTypes().ForEach(type => items.AddRange(Inventory.main.container.GetItems(type)));

                items = items.Where(item => item.item.gameObject.GetComponent<Oxygen>() != null && item.item.GetTechType() != this.TechType).ToList();

                float oxygen = items.Select(item => item.item.gameObject.GetComponent<Oxygen>().oxygenCapacity).Sum();
                items.ForEach(item => item.item.gameObject.GetComponent<Oxygen>().oxygenAvailable = 0);

                if (GetComponent<Oxygen>().oxygenCapacity != oxygen)
                {
                    GetComponent<Oxygen>().oxygenCapacity = oxygen;
                    Player.main.oxygenMgr.UnregisterSource(GetComponent<Oxygen>());
                    Player.main.oxygenMgr.RegisterSource(GetComponent<Oxygen>());
                }

                ErrorMessage.AddDebug((45f + oxygen).ToString());
            }
        }
    }
}

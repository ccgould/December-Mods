using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Assets;
using UnityEngine;
using System.Collections;

namespace MAC.NightVisionChip {
    public class NightVisionChip : Craftable {

        private static readonly NightVisionChip main = new NightVisionChip();

        internal static TechType TechTypeID { get; private set; }

        public NightVisionChip() :
            base("NightVisionChip", "Night Vision HUD", "Adds night vision capabilities to your scuba HUD.")
        {
            OnFinishedPatching += AdditionalPatching;
        }

        public override CraftTree.Type FabricatorType { get; } = CraftTree.Type.Fabricator;
        public override TechGroup GroupForPDA { get; } = TechGroup.Personal;
        public override TechCategory CategoryForPDA { get; } = TechCategory.Equipment;
        public override string AssetsFolder { get; } = "NightVisionChip/Assets";
        public override string[] StepsToFabricatorTab { get; } = new[] { "Personal", "Equipment" };

        public override GameObject GetGameObject()
        {
            GameObject prefab = CraftData.GetPrefabForTechType(TechType.Compass);

            return GameObject.Instantiate(prefab);

        }

        protected override TechData GetBlueprintRecipe()
        {
            return new TechData
            {
                craftAmount = 1,
                Ingredients =
                {
                    new Ingredient(TechType.AdvancedWiringKit, 1),
                    new Ingredient(TechType.Magnetite, 1),
                }
            };
        }

        public static void PatchSMLHelper()
        {
            main.Patch();
        }

        private void AdditionalPatching()
        {
            TechTypeID = this.TechType;
            CraftDataHandler.SetEquipmentType(this.TechType, EquipmentType.Chip);
        }

    }
}

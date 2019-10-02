using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Assets;
using UnityEngine;

namespace MAC.WarpShield {
    public class WarpShieldModule : Craftable {

        private static readonly WarpShieldModule main = new WarpShieldModule();

        internal static TechType TechTypeID { get; private set; }

        public WarpShieldModule() : base("WarpShieldModule", " Phasic Stabilization Module", "Stabilizes reality in the vicinity, preventing most forms of teleportion.")
        {
            OnFinishedPatching += AdditionalPatching;
        }

        public override CraftTree.Type FabricatorType { get; } = CraftTree.Type.SeamothUpgrades;
        public override TechGroup GroupForPDA { get; } = TechGroup.VehicleUpgrades;
        public override TechCategory CategoryForPDA { get; } = TechCategory.VehicleUpgrades;
        public override string AssetsFolder { get; } = "WarpShield/Assets";
        public override string[] StepsToFabricatorTab { get; } = new[] { "CommonModules" };
        public override TechType RequiredForUnlock { get; } = TechType.PrecursorPrisonIonGenerator;

        public override GameObject GetGameObject()
        {
            GameObject prefab = CraftData.GetPrefabForTechType(TechType.HullReinforcementModule);
            return GameObject.Instantiate(prefab);

        }

        protected override TechData GetBlueprintRecipe()
        {
            return new TechData
            {
                craftAmount = 1,
                Ingredients =
                {
                    new Ingredient(TechType.Diamond, 3),
                    new Ingredient(TechType.AdvancedWiringKit, 1),
                    new Ingredient(TechType.Polyaniline, 1)
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
            CraftDataHandler.SetEquipmentType(this.TechType, EquipmentType.VehicleModule);
        }
    }
}

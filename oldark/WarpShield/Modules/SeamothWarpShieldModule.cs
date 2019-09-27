using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Assets;
using UnityEngine;

namespace MAC.WarpShield {
    public class SeamothWarpShieldModule : Craftable {

        private static readonly SeamothWarpShieldModule main = new SeamothWarpShieldModule();

        internal static TechType TechTypeID { get; private set; }

        public SeamothWarpShieldModule() : base("SeamothWarpShieldModule", "Seamoth Phasic Stabilizer", "Stabilizes reality in the vicinity, preventing most forms of teleportion.")
        {
            OnFinishedPatching += AdditionalPatching;
        }

        public override CraftTree.Type FabricatorType { get; } = CraftTree.Type.SeamothUpgrades;
        public override TechGroup GroupForPDA { get; } = TechGroup.VehicleUpgrades;
        public override TechCategory CategoryForPDA { get; } = TechCategory.VehicleUpgrades;
        public override string AssetsFolder { get; } = "WarpShield/Assets";
        public override string[] StepsToFabricatorTab { get; } = new[] { "SeamothModules" };
        public override TechType RequiredForUnlock { get; } = TechType.None;

        public override GameObject GetGameObject()
        {
            GameObject prefab = CraftData.GetPrefabForTechType(TechType.SeamothSonarModule);
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
            CraftDataHandler.SetEquipmentType(this.TechType, EquipmentType.SeamothModule);
        }
    }
}

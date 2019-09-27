using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMLHelper.V2.Crafting;

namespace MAC.WarpShield {
    public class SeamothWarpShieldModule : SeamothModule {
        public SeamothWarpShieldModule() :
            base("SeamothWarpShieldModule",
                "Seamoth Warp Shield",
                "Prevents being forced out of your vehicle via teleportation.",
                CraftTree.Type.SeamothUpgrades,
                new string[1] { "SeamothModules" },
                TechType.ExosuitThermalReactorModule,
                TechType.ExosuitThermalReactorModule)
        {
            SeamothWarpShieldModule = TechType;
        }

        public override TechData GetTechData()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient(TechType.Diamond, 3),
                    new Ingredient(TechType.AdvancedWiringKit, 1),
                    new Ingredient(TechType.Polyaniline, 1)
                }
            };

        }
    }
}

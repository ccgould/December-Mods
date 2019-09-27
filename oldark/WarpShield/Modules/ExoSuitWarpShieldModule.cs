using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Assets;
using UnityEngine;

namespace MAC.WarpShield {
    public class ExoSuitWarpShieldModule : ExoSuitModule {
        public ExoSuitWarpShieldModule() :
            base("ExoSuitWarpShieldModule",
                "Exosuit Phasic Stabilizer",
                "Prevents being forced out of your vehicle via teleportation.",
                CraftTree.Type.SeamothUpgrades,
                new string[1] { "ExosuitModules" },
                TechType.ExosuitThermalReactorModule,
                TechType.ExosuitThermalReactorModule)
        {
            ExoSuitWarpShieldModule = TechType;
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

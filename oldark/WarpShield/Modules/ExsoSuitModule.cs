﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Assets;
using UnityEngine;

namespace MAC.WarpShield {
    public abstract class ExoSuitModule : ModPrefab {
        public static TechType ExoSuitWarpShieldModule { get; protected set; }

        public readonly string ID;
        public readonly string DisplayName;
        public readonly string Tooltip;
        public readonly TechType RequiredForUnlock;
        public readonly CraftTree.Type Fabricator;
        public readonly string[] StepsToTab;
        public readonly Atlas.Sprite Sprite;
        public readonly TechType AddAfter;

        protected ExoSuitModule(string id, string displayName, string tooltip, CraftTree.Type fabricator, string[] stepsToTab, TechType requiredToUnlock = TechType.None, TechType addAfter = TechType.None, Atlas.Sprite sprite = null) : base(id, $"WorldEntities/Tools/{id}", TechType.None)
        {
            ID = id;
            DisplayName = displayName;
            Tooltip = tooltip;
            Fabricator = fabricator;
            RequiredForUnlock = requiredToUnlock;
            StepsToTab = stepsToTab;
            Sprite = sprite;
            AddAfter = addAfter;

            Patch();
        }

        public void Patch()
        {
            TechType = TechTypeHandler.AddTechType(ID, DisplayName, Tooltip, RequiredForUnlock == TechType.None);

            if (RequiredForUnlock != TechType.None)
                KnownTechHandler.SetAnalysisTechEntry(RequiredForUnlock, new TechType[] { TechType });

            if (Sprite == null)
                SpriteHandler.RegisterSprite(TechType, $"./QMods/WarpShield/Assets/{ID}.png");
            else
                SpriteHandler.RegisterSprite(TechType, Sprite);

            switch (Fabricator)
            {
                case CraftTree.Type.Workbench:
                    CraftDataHandler.AddToGroup(TechGroup.Workbench, TechCategory.Workbench, TechType, AddAfter);
                    break;

                case CraftTree.Type.SeamothUpgrades:
                    CraftDataHandler.AddToGroup(TechGroup.VehicleUpgrades, TechCategory.VehicleUpgrades, TechType, AddAfter);
                    break;
            }

            CraftDataHandler.SetEquipmentType(TechType, EquipmentType.ExosuitModule);
            CraftDataHandler.SetTechData(TechType, GetTechData());
            CraftTreeHandler.AddCraftingNode(Fabricator, TechType, StepsToTab);
        }

        public override GameObject GetGameObject()
        {
            // Get the ElectricalDefense module prefab and instantiate it
            var path = "WorldEntities/Tools/SeamothElectricalDefense";
            var prefab = Resources.Load<GameObject>(path);
            var obj = GameObject.Instantiate(prefab);

            // Get the TechTags and PrefabIdentifiers
            var techTag = obj.GetComponent<TechTag>();
            var prefabIdentifier = obj.GetComponent<PrefabIdentifier>();

            // Change them so they fit to our requirements.
            techTag.type = TechType;
            prefabIdentifier.ClassId = ClassID;

            return obj;
        }

        public abstract TechData GetTechData();
    }

}

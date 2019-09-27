using Common.Extensions;
using Common.Utilities;
using MAC.OxStation.Managers;
using MAC.OxStation.Mono;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MAC.OxStation.Buildables
{
    internal partial class OxStationBuildable : Buildable
    {
        #region Public Overrides
        public override TechGroup GroupForPDA { get; } = TechGroup.ExteriorModules;
        public override TechCategory CategoryForPDA { get; } = TechCategory.ExteriorModule;
        public override string AssetsFolder { get; } = $"OxStation/Assets";
        #endregion

        #region Private Members
        private static readonly OxStationBuildable Singleton = new OxStationBuildable();
        #endregion

        #region Contructor
        public OxStationBuildable() : base("OxStation", "OxStation", "A oxygen producing unity for your habitat.")
        {
            OnFinishedPatching += AdditionalPatching;
        }
        #endregion

        #region Buildable Overrides

        public override GameObject GetGameObject()
        {
            GameObject prefab = null;

            try
            {
                prefab = GameObject.Instantiate<GameObject>(_prefab);

                prefab.name = this.PrefabFileName;

                PrefabIdentifier prefabID = prefab.GetOrAddComponent<PrefabIdentifier>();
                prefabID.ClassId = this.ClassID;

                prefab.AddComponent<AnimationManager>();
                prefab.AddComponent<OxStationController>();

                var techTag = prefab.GetOrAddComponent<TechTag>();
                techTag.type = TechType;

            }
            catch (Exception e)
            {
                QuickLogger.Error(e.Message);
            }

            return prefab;
        }

        private void Register()
        {
            if (_prefab != null)
            {
                var meshRenderers = _prefab.GetComponentsInChildren<MeshRenderer>();

                //========== Allows the building animation and material colors ==========// 
                Shader shader = Shader.Find("MarmosetUBER");
                Renderer[] renderers = _prefab.GetComponentsInChildren<Renderer>(true);
                SkyApplier skyApplier = _prefab.GetOrAddComponent<SkyApplier>();
                skyApplier.renderers = renderers;
                skyApplier.anchorSky = Skies.Auto;

                //========== Allows the building animation and material colors ==========// 

                // Add constructible
                var constructable = _prefab.GetOrAddComponent<Constructable>();
                constructable.allowedOnWall = false;
                constructable.allowedOnGround = true;
                constructable.allowedInSub = false;
                constructable.allowedInBase = false;
                constructable.allowedOnCeiling = false;
                constructable.allowedOutside = true;
                constructable.model = _prefab.FindChild("model");
                constructable.techType = TechType;
                constructable.rotationEnabled = true;

                // Add large world entity ALLOWS YOU TO SAVE ON TERRAIN
                var lwe = _prefab.AddComponent<LargeWorldEntity>();
                lwe.cellLevel = LargeWorldEntity.CellLevel.Global;

                _prefab.AddComponent<PlayerInteractionManager>();
            }
        }

        protected override TechData GetBlueprintRecipe()
        {
            QuickLogger.Debug($"Creating recipe...");
            // Create and associate recipe to the new TechType
            var customFabRecipe = new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient(TechType.TitaniumIngot, 1),
                    new Ingredient(TechType.WiringKit, 1),
                    new Ingredient(TechType.Silicone, 2),
                    new Ingredient(TechType.Lubricant, 2),
                    new Ingredient(TechType.Tank, 1),
                }
            };
            QuickLogger.Debug($"Created Ingredients");
            return customFabRecipe;
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// Patches the mod for use in subnautica
        /// </summary>
        internal static void PatchHelper()
        {
            if (!Singleton.GetPrefabs())
            {
                throw new FileNotFoundException($"Failed to retrieve the {Singleton.FriendlyName} prefab from the asset bundle");
            }
            Singleton.Register();
            Singleton.Patch();
        }
        #endregion
    }
}

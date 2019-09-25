using Common.Helpers;
using Common.Utilities;
using MAC.FireExtinguisherHolder.Config;
using UnityEngine;

namespace MAC.FireExtinguisherHolder.Buildable
{
    internal partial class FEHolderBuildable
    {
        private GameObject _prefab;

        private bool GetPrefabs()
        {
            QuickLogger.Debug("GetPrefabs");
            AssetBundle assetBundle = AssetHelper.Asset(Mod.ModName, Mod.BundleName);

            //If the result is null return false.
            if (assetBundle == null)
            {
                QuickLogger.Error($"AssetBundle is Null!");
                return false;
            }

            QuickLogger.Debug($"AssetBundle Set");

            //We have found the asset bundle and now we are going to continue by looking for the model.
            GameObject prefab = assetBundle.LoadAsset<GameObject>("FireExtinguisherHolder");

            //If the prefab isn't null lets add the shader to the materials
            if (prefab != null)
            {
                // Scale
                prefab.transform.localScale *= 2f;
                ApplyShaders(prefab, assetBundle);
                _prefab = prefab;

                QuickLogger.Debug($"{this.FriendlyName} Prefab Found!");
            }
            else
            {
                QuickLogger.Error($"{this.FriendlyName} Prefab Not Found!");
                return false;
            }

            return true;
        }

        internal static void ApplyShaders(GameObject prefab, AssetBundle bundle)
        {
            #region fire_extinguisher_01_holder
            MaterialHelpers.ApplySpecShader("fire_extinguisher_01_holder", "fire_extinguisher_01_holder_spec", prefab, 1, 0.5f, bundle);
            MaterialHelpers.ApplyNormalShader("fire_extinguisher_01_holder", "fire_extinguisher_01_holder_normal", prefab, bundle);
            MaterialHelpers.ApplyEmissionShader("fire_extinguisher_01_holder", "fire_extinguisher_01_holder_illum", prefab, bundle, new Color(0.08235294f, 1f, 1f));
            #endregion

            #region fire_extinguisher_01
            MaterialHelpers.ApplySpecShader("fire_extinguisher_01", "fire_extinguisher_01_spec", prefab, 1, 0.5f, bundle);
            MaterialHelpers.ApplyEmissionShader("fire_extinguisher_01", "fire_extinguisher_01_illum", prefab, bundle, Color.white);
            MaterialHelpers.ApplyNormalShader("fire_extinguisher_01", "fire_extinguisher_01_normal", prefab, bundle);
            #endregion
        }
    }
}

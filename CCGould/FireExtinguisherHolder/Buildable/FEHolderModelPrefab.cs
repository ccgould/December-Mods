using Common.Utilities;
using UnityEngine;

namespace MAC.CCgould.FireExtinguisherHolder.Buildable
{
    internal partial class FEHolderBuildable
    {
        private GameObject _prefab;

        private bool GetPrefabs()
        {
            //We have found the asset bundle and now we are going to continue by looking for the model.
            GameObject prefab = assetBundle.LoadAsset<GameObject>("AlterraDeepDriller");

            //If the prefab isn't null lets add the shader to the materials
            if (prefab != null)
            {
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
    }
}

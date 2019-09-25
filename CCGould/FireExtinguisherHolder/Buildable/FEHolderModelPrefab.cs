using Common.Utilities;
using UnityEngine;

namespace MAC.FireExtinguisherHolder.Buildable
{
    internal partial class FEHolderBuildable
    {
        private GameObject _prefab;

        private bool GetPrefabs()
        {
            var resourcePath = "WorldEntities/Doodads/Debris/Wrecks/Decoration/fireextinguisher_holder";

            //We have found the asset bundle and now we are going to continue by looking for the model.
            GameObject prefab = Resources.Load<GameObject>(resourcePath);

            //If the prefab isn't null lets add the shader to the materials
            if (prefab != null)
            {
                // Scale
                prefab.transform.localScale *= 2f;

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

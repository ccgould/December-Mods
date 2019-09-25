using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Helpers
{
    public class MaterialHelpers
    {
        /// <summary>
        /// Finds a <see cref="Texture2D"/> in the asset bundle with the specified name.
        /// </summary>
        /// <param name="textureName"></param>
        /// <param name="assetBundle"></param>
        /// <returns></returns>
        public static Texture2D FindTexture2D(string textureName, AssetBundle assetBundle)
        {
            if (assetBundle != null)
            {
                var objects = new List<object>(assetBundle.LoadAllAssets(typeof(object)));

                for (int i = 0; i < objects.Count; i++)
                {
                    if (objects[i] is Texture2D)
                    {
                        if (((Texture2D)objects[i]).name.Equals(textureName))
                        {
                            return ((Texture2D)objects[i]);
                        }
                    }
                }
            }

            //QuickLogger.Error($"Couldn't Find Texture: {textureName}");

            return null;
        }

        /// <summary>
        /// Applies the properties for the MarmosetUBER shader that has a emission texture.
        /// </summary>
        /// <param name="materialName">The name of the material to look for on the object.</param>
        /// <param name="textureName">The name of the texture to look for in the assetBundle.</param>
        /// <param name="gameObject">The game object to process.</param>
        /// <param name="assetBundle">The assetBundle to search in.</param>
        /// <param name="emissionColor">The color to use on the emission material.</param>
        public static void ApplyEmissionShader(string materialName, string textureName, GameObject gameObject, AssetBundle assetBundle, Color emissionColor, float emissionMuli = 1.0f)
        {
            //Use this to do the Emission
            var shader = Shader.Find("MarmosetUBER");
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (RemoveInstance(material.name).Equals(materialName))
                    {
                        material.shader = shader;
                        //material.EnableKeyword("_EMISSION");
                        material.EnableKeyword("MARMO_EMISSION");
                        material.SetVector("_EmissionColor", emissionColor * emissionMuli);
                        material.SetTexture("_Illum", FindTexture2D(textureName, assetBundle));
                        material.SetVector("_Illum_ST", new Vector4(1.0f, 1.0f, 0.0f, 0.0f));
                    }
                }
            }
        }

        /// <summary>
        /// Applies the properties for the MarmosetUBER shader that has a metallic texture.
        /// </summary>
        /// <param name="materialName">The name of the material to look for on the object.</param>
        /// <param name="textureName">The name of the texture to look for in the assetBundle.</param>
        /// <param name="gameObject">The game object to process.</param>
        /// <param name="assetBundle">The assetBundle to search in.</param>
        public static void ApplyNormalShader(string materialName, string textureName, GameObject gameObject, AssetBundle assetBundle)
        {
            var shader = Shader.Find("MarmosetUBER");
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (RemoveInstance(material.name).Equals(materialName))
                    {
                        material.shader = shader;

                        material.EnableKeyword("_NORMALMAP");

                        material.SetTexture("_BumpMap", FindTexture2D(textureName, assetBundle));
                    }
                }
            }
        }

        /// <summary>
        /// Applies the properties for the MarmosetUBER shader that has a specular texture.
        /// </summary>
        /// <param name="materialName">The name of the material to look for on the object.</param>
        /// <param name="textureName">The name of the texture to look for in the assetBundle.</param>
        /// <param name="gameObject">The game object to process.</param>
        /// <param name="specInt">The amount of specular to apply in <see cref="float"/>.</param>
        /// <param name="shininess">The amount of shine to apply to the specular in <see cref="float"/>.</param>
        /// <param name="assetBundle">The assetBundle to search in.</param>
        public static void ApplySpecShader(string materialName, string textureName, GameObject gameObject, float specInt, float shininess, AssetBundle assetBundle)
        {
            var shader = Shader.Find("MarmosetUBER");
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (RemoveInstance(material.name).Equals(materialName))
                    {
                        material.shader = shader;

                        material.EnableKeyword("MARMO_SPECMAP");

                        material.SetColor("_SpecColor", new Color(0.796875f, 0.796875f, 0.796875f, 0.796875f));
                        material.SetFloat("_SpecInt", specInt);
                        material.SetFloat("_Shininess", shininess);

                        var texture = FindTexture2D(textureName, assetBundle);
                        if (texture != null)
                        {
                            material.SetTexture("_SpecTex", texture);
                        }

                        material.SetFloat("_Fresnel", 0f);
                        material.SetVector("_SpecTex_ST", new Vector4(1.0f, 1.0f, 0.0f, 0.0f));
                    }
                }
            }
        }

        private static string RemoveInstance(string name)
        {
            return name.Replace("(Instance)", String.Empty).Trim();

        }
    }
}

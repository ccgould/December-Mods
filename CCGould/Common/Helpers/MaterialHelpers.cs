﻿using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Finds a material in the assetBundle of the specified name.
        /// </summary>
        /// <param name="materialName">Name of the material to locate in the asset bundle.</param>
        /// <param name="assetBundle">The asset Bundle to search in.</param>
        /// <returns>Returns the <see cref="Material"/> of the specified type</returns>
        public static Material FindMaterial(string materialName, AssetBundle assetBundle)
        {
            var objects = new List<object>(assetBundle.LoadAllAssets(typeof(object)));

            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is Material)
                {
                    if (((Material)objects[i]).name.Equals(materialName))
                    {
                        return ((Material)objects[i]);
                    }
                }
            }

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
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (material.name.Equals(RemoveInstance(materialName)))
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
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (material.name.StartsWith(RemoveInstance(materialName)))
                    {
                        material.shader = shader;

                        material.EnableKeyword("_NORMALMAP");

                        material.SetTexture("_BumpMap", FindTexture2D(textureName, assetBundle));
                    }
                }
            }
        }

        public static void ChangeMaterialColor(string materialName, GameObject gameObject, Color color)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (material.name.StartsWith(materialName))
                    {
                        material.SetColor("_Color", color);
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
        /// <param name="glossiness">The amount of gloss to apply to the metallic material.</param>
        public static void ApplyMetallicShader(string materialName, string textureName, GameObject gameObject, AssetBundle assetBundle, float glossiness)
        {
            var shader = Shader.Find("MarmosetUBER");
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (material.name.StartsWith(materialName))
                    {
                        material.shader = shader;
                        material.EnableKeyword("_METALLICGLOSSMAP");
                        material.SetColor("_Color", Color.white);
                        material.SetTexture("_Metallic", MaterialHelpers.FindTexture2D(textureName, assetBundle));
                        material.SetFloat("_Glossiness", glossiness);
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
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    QuickLogger.Debug($"Material Name: {RemoveInstance(materialName)}");
                    if (material.name.Equals(RemoveInstance(materialName)))
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

        /// <summary>
        /// Applies the properties for the MarmosetUBER shader to make a material that has a transparency layer become transparent.
        /// </summary>
        /// <param name="materialName">The name of the material to look for on the object.</param>
        /// <param name="gameObject">The game object to process.</param>
        public static void ApplyAlphaShader(string materialName, GameObject gameObject)
        {
            var shader = Shader.Find("MarmosetUBER");
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (material.name.StartsWith(materialName))
                    {
                        material.shader = shader;
                        material.EnableKeyword("_ZWRITE_ON");
                        material.EnableKeyword("MARMO_ALPHA");
                        material.EnableKeyword("MARMO_ALPHA_CLIP");
                    }
                }
            }
        }

        /// <summary>
        /// Applies the properties for the standard shader to make a material appear like glass.
        /// </summary>
        /// <param name="materialName">The name of the material to look for on the object.</param>
        /// <param name="gameObject">The game object to process.</param>
        /// <param name="glossiness">The amount of gloss for the metallic property.</param>
        public static void ApplyGlassShaderTemplate(string materialName, GameObject gameObject, float glossiness)
        {
            var shader = Shader.Find("Standard");
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (material.name.StartsWith(materialName))
                    {

                        material.shader = shader;
                        material.EnableKeyword("MARMO_SPECMAP");
                        material.EnableKeyword("MARMO_SIMPLE_GLASS");
                        material.EnableKeyword("WBOIT");
                        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                        //material.SetColor("_SpecColor", new Color(0.796875f, 0.796875f, 0.796875f, 0.796875f));
                        //material.SetFloat("_SpecInt", 8f);
                        //material.SetFloat("_Shininess", 0.5f);
                        //material.SetVector("_SpecTex_ST", new Vector4(1.0f, 1.0f, 0.0f, 0.0f));

                    }
                }
            }
        }

        public static void ApplyPrecursorShader(string materialName, string normalMap, string metalicmap, GameObject gameObject, AssetBundle assetBundle, float glossiness)
        {
            var shader = Shader.Find("UWE/Marmoset/IonCrystal");

            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (material.name.StartsWith(materialName))
                    {
                        material.shader = shader;
                        material.EnableKeyword("_NORMALMAP");

                        material.EnableKeyword("_METALLICGLOSSMAP");

                        material.SetTexture("_BumpMap", FindTexture2D(normalMap, assetBundle));

                        material.SetColor("_BorderColor", new Color(0.14f, 0.55f, 0.43f));

                        material.SetColor("_Color", new Color(0.33f, 0.83f, 0.17f));

                        material.SetColor("_DetailsColor", new Color(0.42f, 0.85f, 0.26f));

                        material.SetTexture("_MarmoSpecEnum", MaterialHelpers.FindTexture2D(metalicmap, assetBundle));

                        material.SetFloat("_Glossiness", glossiness);

                    }
                }
            }
        }

        public static void ApplyColorMaskShader(string materialName, string normalMap, string metalicmap, string maskmap, GameObject gameObject, AssetBundle assetBundle, float glossiness)
        {
            Shader[] assets = assetBundle.LoadAllAssets<Shader>();

            Shader customColorShader = assets.FirstOrDefault(shader => shader.name.Equals("Custom/ColorMask"));

            if (customColorShader == null)
            {
                QuickLogger.Error("Custom Shader: Null");
            }
            else
            {
                QuickLogger.Debug("Custom Shader: Found");
                Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

                foreach (Renderer renderer in renderers)
                {
                    foreach (Material material in renderer.materials)
                    {
                        if (material.name.StartsWith(materialName))
                        {
                            material.shader = customColorShader;

                            material.EnableKeyword("_NORMALMAP");

                            material.EnableKeyword("_METALLICGLOSSMAP");

                            material.SetTexture("_Normal", FindTexture2D(normalMap, assetBundle));

                            material.SetTexture("_MaskTex", FindTexture2D(maskmap, assetBundle));

                            material.SetColor("_Color1", Color.green);
                            material.SetColor("_Color2", Color.white);
                            material.SetColor("_Color3", Color.white);

                            material.SetTexture("_Metallic", MaterialHelpers.FindTexture2D(metalicmap, assetBundle));

                            material.SetFloat("_Glossiness", glossiness);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Change the color of the body of the gameobject
        /// </summary>
        /// <param name="matNameColor">The material to change</param>
        /// <param name="color">The color to change to</param>
        /// <param name="gameObject">The game object to apply the change too.</param>
        public static void ChangeBodyColor(string matNameColor, Color color, GameObject gameObject)
        {
            MaterialHelpers.ChangeMaterialColor(matNameColor, gameObject, color);
        }

        public static void ReplaceEmissionTexture(string materialName, string replacementTexture, GameObject gameObject, AssetBundle assetBundle)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (material.name.StartsWith(materialName))
                    {
                        material.SetTexture("_Illum", FindTexture2D(replacementTexture, assetBundle));
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

using System;
using UnityEngine;
using Harmony;
using System.Reflection;

namespace MAC.Senna.Fahrenheit
{
    public static class Main
    {
        public static void Load()
        {
            try
            {
                HarmonyInstance.Create("com.Fahrenheit.MAC").PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}

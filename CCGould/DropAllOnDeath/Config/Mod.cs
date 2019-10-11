using System;
using System.IO;

namespace MAC.DropAllOnDeath.Config
{
    internal static class Mod
    {
        #region Internal Properties
        internal static string ModName => "DropAllOnDeath";
        internal static string MODFOLDERLOCATION => GetModPath();

        #endregion

        #region Internal Methods
        internal static string ConfigurationFile()
        {
            return Path.Combine(MODFOLDERLOCATION, "mod.json");
        }
        #endregion

        #region Private Methods
        private static string GetModPath()
        {
            return Path.Combine(GetQModsPath(), ModName);
        }
        private static string GetQModsPath()
        {
            return Path.Combine(Environment.CurrentDirectory, "QMods");
        }
        #endregion
    }
}

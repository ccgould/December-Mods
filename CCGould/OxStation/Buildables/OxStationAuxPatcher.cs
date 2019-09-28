using SMLHelper.V2.Handlers;

namespace MAC.OxStation.Buildables
{
    internal partial class OxStationBuildable
    {
        #region Private Members
        private const string PowerUsageKey = "MAC_PowerUsage";
        private const string PerMinuteKey = "MAC_PerMinute";
        private const string TakeOxygenKey = "MAC_TakeOxygen";

        #endregion

        #region Internal Properties
        internal static string BuildableName { get; private set; }
        internal static TechType TechTypeID { get; private set; }
        #endregion

        #region Private Methods
        private void AdditionalPatching()
        {
            BuildableName = this.FriendlyName;
            TechTypeID = this.TechType;

            LanguageHandler.SetLanguageLine(PowerUsageKey, "Power Usage");
            LanguageHandler.SetLanguageLine(PerMinuteKey, "per minute");
            LanguageHandler.SetLanguageLine(TakeOxygenKey, "Take Oxygen");
        }
        #endregion

        #region Internal Methods
        internal static string PowerUsage()
        {
            return Language.main.Get(PowerUsageKey);
        }

        internal static string PerMinute()
        {
            return Language.main.Get(PerMinuteKey);
        }

        internal static string TakeOxygen()
        {
            return Language.main.Get(TakeOxygenKey);
        }
        #endregion
    }
}

using SMLHelper.V2.Handlers;

namespace MAC.OxStation.Buildables
{
    internal partial class OxStationBuildable
    {
        #region Private Members
        private const string OnHandHoverEmptyKey = "MAC_OnHandOverEmpty";
        private const string OnHandHoverNotEmptyKey = "MAC_OnHandOverNotEmpty";
        private const string OnHolderNotEmptyKey = "MAC_HolderNotEmpty";
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

            LanguageHandler.SetLanguageLine(OnHandHoverEmptyKey, "Click place extinguisher.");
            LanguageHandler.SetLanguageLine(OnHandHoverNotEmptyKey, "Click remove extinguisher.");
            LanguageHandler.SetLanguageLine(OnHolderNotEmptyKey, "Cannot deconstruct please remove extinguisher first.");
        }
        #endregion

        #region Internal Methods
        internal static string OnHandOverEmpty()
        {
            return Language.main.Get(OnHandHoverEmptyKey);
        }

        internal static string HolderNotEmptyMessage()
        {
            return Language.main.Get(OnHolderNotEmptyKey);
        }
        #endregion

        internal static string OnHandOverNotEmpty()
        {
            return Language.main.Get(OnHandHoverNotEmptyKey);
        }
    }
}

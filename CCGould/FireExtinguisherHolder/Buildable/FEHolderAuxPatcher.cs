using SMLHelper.V2.Handlers;

namespace MAC.CCgould.FireExtinguisherHolder.Buildable
{
    internal partial class FEHolderBuildable
    {
        #region Private Memebers
        private const string StorageLabelKey = "DD_StorageLabel";
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

            LanguageHandler.SetLanguageLine(StorageLabelKey, "FCS Deep Driller Receptacle.");
        }
        #endregion

        #region Internal Methods
        internal static string StorageLabel()
        {
            return Language.main.Get(StorageLabelKey);
        }
        #endregion
    }
}

using SMLHelper.V2.Options;

namespace MAC.DropAllOnDeath.Config
{
    public class Options : ModOptions
    {
        private const string ToggleID = "DropAllOnDeathEnabled";
        private bool _enabled;

        public Options() : base("Drop All On Death Settings")
        {
            ToggleChanged += OnToggleChanged;
            _enabled = QPatch.Configuration.Config.Enabled;
        }

        public void OnToggleChanged(object sender, ToggleChangedEventArgs e)
        {
            if (e.Id != ToggleID) return;
            _enabled = e.Value;
            QPatch.SetModEnabled(e.Value);
        }

        public override void BuildModOptions()
        {
            AddToggleOption(ToggleID, "Enable Drop All On Death", _enabled);
        }
    }
}

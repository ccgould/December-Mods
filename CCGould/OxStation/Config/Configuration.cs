using System.Collections.Generic;

namespace MAC.OxStation.Config
{
    public class Config
    {
        public float TankCapacity { get; set; }
        public float OxygenPerSecond { get; set; }
    }

    public class Configuration
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string AssemblyName { get; set; }
        public string EntryMethod { get; set; }
        public List<string> Dependencies { get; set; }
        public List<string> LoadAfter { get; set; }
        public Config Config { get; set; }
        public bool Enable { get; set; }
    }
}
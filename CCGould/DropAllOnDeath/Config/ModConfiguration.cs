using System.Collections.Generic;

namespace MAC.DropAllOnDeath.Config
{
    public class Config
    {
        public bool Enabled { get; set; }
    }

    public class ModConfiguration
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public bool Enable { get; set; }
        public string AssemblyName { get; set; }
        public string EntryMethod { get; set; }
        public List<string> Dependencies { get; set; }
        public Config Config { get; set; }
    }
}

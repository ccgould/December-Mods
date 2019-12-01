using System.Collections.Generic;

namespace MAC.OxStation.Config
{
    public class Configuration
    {
        public bool PlaySFX;
        public float TankCapacity { get; set; }
        public float OxygenPerSecond { get; set; }
        public float EnergyPerSec { get; set; }
    }
}
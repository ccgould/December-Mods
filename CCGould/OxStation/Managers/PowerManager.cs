using Common.Enumerator;
using Common.Utilities;
using MAC.OxStation.Mono;
using System;

namespace MAC.OxStation.Managers
{
    internal class PowerManager
    {
        private PowerStates _powerState;
        private OxStationController _mono;
        private SubRoot _habitat;

        private PowerStates PowerState
        {
            get => _powerState;
            set
            {
                _powerState = value;
                QuickLogger.Debug($"Current PowerState: {value}");
                OnPowerUpdate?.Invoke(value);
            }
        }

        internal Action<PowerStates> OnPowerUpdate;

        internal void Initialize(OxStationController mono)
        {
            _mono = mono;
            var habitat = mono?.gameObject.transform?.parent?.gameObject.GetComponentInParent<SubRoot>();

            if (habitat != null)
            {
                _habitat = habitat;
            }
        }

        internal void UpdatePowerState()
        {
            if (_habitat == null)
            {
                QuickLogger.Error($"Habitat returned null");
                return;
            }
            if (_habitat.powerRelay.GetPowerStatus() == PowerSystem.Status.Offline && GetPowerState() != PowerStates.UnPowered)
            {
                SetPowerStates(PowerStates.UnPowered);
                return;
            }

            if (_habitat.powerRelay.GetPowerStatus() != PowerSystem.Status.Offline && GetPowerState() != PowerStates.Powered)
            {
                SetPowerStates(PowerStates.Powered);
            }
        }

        /// <summary>
        /// Gets the powerState of the unit.
        /// </summary>
        /// <returns></returns>
        internal PowerStates GetPowerState()
        {
            return PowerState;
        }

        /// <summary>
        /// Sets the powerstate
        /// </summary>
        /// <param name="powerState">The power state to set this unit</param>
        internal void SetPowerStates(PowerStates powerState)
        {
            PowerState = powerState;
        }
    }
}

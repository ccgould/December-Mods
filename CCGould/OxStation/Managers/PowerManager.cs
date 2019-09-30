using Common.Enumerator;
using Common.Utilities;
using MAC.OxStation.Mono;
using System;
using UnityEngine;

namespace MAC.OxStation.Managers
{
    internal class PowerManager
    {
        private PowerStates _powerState;
        private OxStationController _mono;
        private SubRoot _habitat;
        private PowerRelay _connectedRelay = null;
        private float EnergyConsumptionPerSecond { get; set; } = QPatch.Configuration.Config.EnergyPerSec;
        private float AvailablePower => this.ConnectedRelay.GetPower();

        private PowerRelay ConnectedRelay
        {
            get
            {
                while (_connectedRelay == null)
                    UpdatePowerRelay();

                return _connectedRelay;
            }
        }
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
        private bool _hasPowerToConsume;
        private float _energyToConsume;

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
            if (_habitat == null) return;

            if (_mono.HealthManager.IsDamageApplied())
            {
                SetPowerStates(PowerStates.UnPowered);
                return;
            }

            if (_habitat.powerRelay.GetPowerStatus() == PowerSystem.Status.Offline || !_hasPowerToConsume && GetPowerState() != PowerStates.UnPowered)
            {
                SetPowerStates(PowerStates.UnPowered);
                return;
            }

            if (_habitat.powerRelay.GetPowerStatus() != PowerSystem.Status.Offline || _hasPowerToConsume && GetPowerState() != PowerStates.Powered)
            {
                SetPowerStates(PowerStates.Powered);
            }
        }

        internal void ConsumePower()
        {
            if (_mono.HealthManager.IsDamageApplied()) return;

            _energyToConsume = EnergyConsumptionPerSecond * DayNightCycle.main.deltaTime;
            bool requiresEnergy = GameModeUtils.RequiresPower();
            _hasPowerToConsume = !requiresEnergy || AvailablePower >= _energyToConsume;

            if (!requiresEnergy) return;
            ConnectedRelay.ConsumeEnergy(_energyToConsume, out float amountConsumed);
            QuickLogger.Debug($"Energy Consumed: {amountConsumed}");
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

        internal float GetPowerUsage()
        {
            if (_mono.HealthManager.IsDamageApplied() || PowerState != PowerStates.Powered)
            {
                return 0f;
            }
            return Mathf.Round(EnergyConsumptionPerSecond * 60) / 10f;
        }

        private void UpdatePowerRelay()
        {
            PowerRelay relay = PowerSource.FindRelay(_mono.transform);
            if (relay != null && relay != _connectedRelay)
            {
                _connectedRelay = relay;
                QuickLogger.Debug("PowerRelay found at last!");
            }
            else
            {
                _connectedRelay = null;
            }
        }
    }
}

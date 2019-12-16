using Common.Enumerator;
using Common.Utilities;
using MAC.OxStation.Mono;
using System;
using System.Collections;
using UnityEngine;

namespace MAC.OxStation.Managers
{
    internal class PowerManager : MonoBehaviour
    {
        private PowerStates _powerState;
        private OxStationController _mono;

        private PowerRelay _connectedRelay = null;
        private float EnergyConsumptionPerSecond { get; set; } = QPatch.Configuration.EnergyPerSec;
        private float AvailablePower => GetPower();

        private float GetPower()
        {
            float power = 0;

            if (_mono.PowerRelay != null)
            {
                return _mono.PowerRelay.GetPower();
            }

            return power;
        }

        private PowerStates PowerState
        {
            get => _powerState;
            set
            {
                _powerState = value; 
                //QuickLogger.Debug($"Current PowerState: {value}");
                OnPowerUpdate?.Invoke(value);
            }
        }

        internal Action<PowerStates> OnPowerUpdate;
        private bool _hasPowerToConsume;
        private float _energyToConsume;
        private Coroutine _relayCoroutine;
        private bool _checkingForRelay;

        internal void Initialize(OxStationController mono)
        {
            _mono = mono;
            var habitat = mono?.gameObject.transform?.parent?.gameObject.GetComponentInParent<SubRoot>();

            if (habitat != null)
            {
                PowerRelay relay = PowerSource.FindRelay(_mono.transform);
                if (relay != null && relay != _connectedRelay)
                {
                    _mono.PowerRelay.AddInboundPower(relay);
                    _checkingForRelay = false;
                    QuickLogger.Debug("PowerRelay found at last!", true);
                }
                _mono.PowerRelay.dontConnectToRelays = true;
            }
        }

        internal void UpdatePowerState()
        {
            if (_mono.HealthManager.IsDamageApplied())
            {
                SetPowerStates(PowerStates.UnPowered);
                return;
            }

            if (_mono.PowerRelay.GetPower() >= EnergyConsumptionPerSecond)
            {
                SetPowerStates(PowerStates.Powered);
                return;
            }

            SetPowerStates(PowerStates.UnPowered);
        }

        internal void ConsumePower()
        {
            if (_mono.HealthManager.IsDamageApplied() || _mono.PowerRelay == null) return;
            float amountConsumed;
            _energyToConsume = EnergyConsumptionPerSecond * DayNightCycle.main.deltaTime;
            bool requiresEnergy = GameModeUtils.RequiresPower();
            _hasPowerToConsume = !requiresEnergy || AvailablePower >= _energyToConsume;

            if (!requiresEnergy) return;

            _mono.PowerRelay.ConsumeEnergy(_energyToConsume, out amountConsumed);
            
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
        
        private void OnDestroy()
        {
            if(_relayCoroutine != null)
                StopCoroutine(_relayCoroutine);
        }
    }
}

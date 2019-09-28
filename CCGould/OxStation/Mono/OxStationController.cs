using Common.Enumerator;
using Common.Utilities;
using MAC.OxStation.Config;
using MAC.OxStation.Managers;
using System;
using System.Collections;
using UnityEngine;

namespace MAC.OxStation.Mono
{
    [RequireComponent(typeof(WeldablePoint))]
    internal partial class OxStationController : MonoBehaviour, IConstructable
    {
        private PrefabIdentifier _prefabId;
        internal BaseManager Manager { get; private set; }
        internal SubRoot SubRoot { get; private set; }
        internal bool IsConstructed { get; private set; }
        internal Ox_OxygenManager OxygenManager { get; private set; }
        internal PowerManager PowerManager { get; private set; }
        internal HealthManager HealthManager { get; private set; }
        internal AnimationManager AnimationManager { get; private set; }
        internal Managers.DisplayManager DisplayManager { get; private set; }
        internal AudioManager AudioManager;
        internal int IsRunningHash { get; set; }

        private void Initialize()
        {
            if (OxygenManager == null)
            {
                OxygenManager = new Ox_OxygenManager();
                OxygenManager.SetAmountPerSecond(QPatch.Configuration.Config.OxygenPerSecond);
                OxygenManager.Initialize(this);
                StartCoroutine(GenerateOxygen());
            }

            if (PowerManager == null)
            {
                PowerManager = new PowerManager();
                PowerManager.Initialize(this);
                PowerManager.OnPowerUpdate += OnPowerUpdate;
                StartCoroutine(UpdatePowerState());
            }

            if (HealthManager == null)
            {
                HealthManager = new HealthManager();
                HealthManager.Initialize(this);
                HealthManager.SetHealth(100);
                HealthManager.OnDamaged += OnDamaged;
                HealthManager.OnRepaired += OnRepaired;
                StartCoroutine(HealthCheck());
            }

            if (AudioManager == null)
            {
                AudioManager = new AudioManager(gameObject.GetComponent<FMOD_CustomLoopingEmitter>());
                InvokeRepeating("UpdateAudio", 0, 1);
            }

            AnimationManager = gameObject.GetComponent<AnimationManager>();
            IsRunningHash = Animator.StringToHash("IsRunning");

            if (AnimationManager == null)
            {
                QuickLogger.Error($"Animation Manager was not found");
            }

            var playerInterationManager = gameObject.GetComponent<PlayerInteractionManager>();

            if (playerInterationManager != null)
            {
                playerInterationManager.Initialize(this);
            }
        }

        private IEnumerator GenerateOxygen()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                OxygenManager.GenerateOxygen();
            }
        }

        private void OnPowerUpdate(PowerStates obj)
        {
            AnimationManager.SetBoolHash(IsRunningHash, obj == PowerStates.Powered);
        }

        private IEnumerator UpdatePowerState()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                PowerManager.UpdatePowerState();
            }
        }

        private void OnRepaired()
        {

        }

        private void OnDamaged()
        {
            PowerManager?.SetPowerStates(PowerStates.UnPowered);
        }

        private IEnumerator HealthCheck()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                HealthManager.HealthChecks();
            }
        }

        private void Update()
        {
            HealthManager?.UpdateHealthSystem();
            PowerManager?.UpdatePower();
        }

        internal void AddToBaseManager(BaseManager managers = null)
        {
            if (SubRoot == null)
            {
                SubRoot = GetComponentInParent<SubRoot>();
            }

            Manager = managers ?? BaseManager.FindManager(SubRoot);
            Manager.AddBaseUnit(this);
        }

        internal void Save(SaveData newSaveData)
        {

        }

        internal string GetPrefabIDString()
        {
            if (_prefabId == null)
            {
                _prefabId = GetPrefabID();
            }

            return _prefabId.Id;
        }

        private PrefabIdentifier GetPrefabID()
        {
            return GetComponentInParent<PrefabIdentifier>();
        }

        public bool CanDeconstruct(out string reason)
        {
            reason = String.Empty;
            return true;
        }

        public void OnConstructedChanged(bool constructed)
        {
            IsConstructed = constructed;

            if (constructed)
            {
                Initialize();
                AddToBaseManager();
                AnimationManager.SetBoolHash(IsRunningHash, true);

                if (DisplayManager == null)
                {
                    DisplayManager = gameObject.AddComponent<Managers.DisplayManager>();
                    DisplayManager.Setup(this);
                }
            }
        }

        private void UpdateAudio()
        {
            if (!IsConstructed || PowerManager == null || AudioManager == null) return;

            if (IsConstructed && PowerManager.GetPowerState() == PowerStates.Powered)
            {
                AudioManager.PlayMachineAudio();
                return;
            }
            AudioManager.StopMachineAudio();
        }

        private void OnDestroy()
        {
            StopCoroutine(UpdatePowerState());
            StopCoroutine(GenerateOxygen());
            StopCoroutine(HealthCheck());
            CancelInvoke("UpdateAudio");
            BaseManager.RemoveBaseUnit(this);
        }
    }
}

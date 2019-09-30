using Common.Enumerator;
using Common.Utilities;
using MAC.OxStation.Config;
using MAC.OxStation.Enums;
using MAC.OxStation.Managers;
using System;
using System.Collections;
using UnityEngine;

namespace MAC.OxStation.Mono
{
    [RequireComponent(typeof(WeldablePoint))]
    internal partial class OxStationController : MonoBehaviour, IConstructable, IProtoEventListener
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
        private SaveDataEntry _saveData;
        private bool _initialized;
        internal int IsRunningHash { get; set; }

        private void Initialize()
        {
            QuickLogger.Debug("Initializing");
            if (OxygenManager == null)
            {
                OxygenManager = new Ox_OxygenManager();
                OxygenManager.SetAmountPerSecond(QPatch.Configuration.Config.OxygenPerSecond);
                OxygenManager.Initialize(this);
                StartCoroutine(GenerateOxygen());
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

            if (PowerManager == null)
            {
                PowerManager = new PowerManager();
                PowerManager.Initialize(this);
                PowerManager.OnPowerUpdate += OnPowerUpdate;
                StartCoroutine(UpdatePowerState());
            }

            if (AudioManager == null)
            {
                AudioManager = new AudioManager(gameObject.GetComponent<FMOD_CustomLoopingEmitter>());
                InvokeRepeating(nameof(UpdateAudio), 0, 1);
            }

            AnimationManager = gameObject.GetComponent<AnimationManager>();
            IsRunningHash = Animator.StringToHash("IsRunning");

            if (AnimationManager == null)
            {
                QuickLogger.Error($"Animation Manager was not found");
            }

            QuickLogger.Debug("Initialized");
            _initialized = true;
        }



        private void OnRepaired()
        {
            DisplayManager.ChangeTakeO2State(ButtonStates.Enabled);
        }

        private void OnDamaged()
        {
            DisplayManager.ChangeTakeO2State(ButtonStates.Disabled);
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

        private IEnumerator HealthCheck()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (PowerManager == null) yield return null;
                HealthManager.HealthChecks();
            }
        }

        private void Update()
        {
            HealthManager?.UpdateHealthSystem();
            PowerManager?.ConsumePower();
        }

        internal void AddToBaseManager(BaseManager managers = null)
        {
            SubRoot = GetComponentInParent<SubRoot>();

            if (SubRoot == null) return;

            Manager = managers ?? BaseManager.FindManager(SubRoot);
            Manager.AddBaseUnit(this);
        }

        internal void Save(SaveData newSaveData)
        {
            var prefabIdentifier = GetComponent<PrefabIdentifier>();
            var id = prefabIdentifier.Id;

            if (_saveData == null)
            {
                _saveData = new SaveDataEntry();
            }
            _saveData.ID = id;
            _saveData.OxygenLevel = OxygenManager.GetO2Level();
            _saveData.HealthLevel = HealthManager.GetHealth();
            newSaveData.Entries.Add(_saveData);
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

                AddToBaseManager();

                if (SubRoot == null)
                {
                    var playerInterationManager = gameObject.GetComponent<PlayerInteractionManager>();

                    if (playerInterationManager != null)
                    {
                        playerInterationManager.Initialize(this);
                    }

                    return;
                }

                if (!_initialized)
                {
                    Initialize();
                }

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
            CancelInvoke(nameof(UpdateAudio));
            BaseManager.RemoveBaseUnit(this);
        }

        public void OnProtoSerialize(ProtobufSerializer serializer)
        {
            if (!Mod.IsSaving())
            {
                QuickLogger.Info($"Saving {Mod.FriendlyName}");
                Mod.Save();
                QuickLogger.Info($"Saved {Mod.FriendlyName}");
            }
        }

        public void OnProtoDeserialize(ProtobufSerializer serializer)
        {
            QuickLogger.Info($"Loading {Mod.FriendlyName}");
            var prefabIdentifier = GetComponent<PrefabIdentifier>();
            var id = prefabIdentifier?.Id ?? string.Empty;
            var data = Mod.GetSaveData(id);

            if (data == null)
            {
                QuickLogger.Info($"No save found for PrefabId {id}");
                return;
            }

            HealthManager.SetHealth(data.HealthLevel);
            OxygenManager.SetO2Level(data.OxygenLevel);

            QuickLogger.Info($"Loaded {Mod.FriendlyName}");
        }
    }
}

using Common.Helpers;
using Common.Utilities;
using MAC.FireExtinguisherHolder.Buildable;
using MAC.FireExtinguisherHolder.Config;
using System;
using System.Collections;
using UnityEngine;


namespace MAC.FireExtinguisherHolder.Mono
{
    internal class FEHolderController : HandTarget, IHandTarget, IConstructable, IProtoEventListener
    {
        private GameObject _tankMesh;
        private bool _initialized;
        private bool _hasTank = false;
        private float _fuel = 100f;
        private FEHolderSaveDataEntry _saveData;

        internal bool IsConstructed { get; set; }

        public void OnHandHover(GUIHand hand)
        {
            HandReticle main = HandReticle.main;
            QuickLogger.Debug($"Fuel: {_fuel} || HasTank = {_hasTank}", true);
            if (!_hasTank)
            {
                main.SetInteractText(LanguageHelpers.GetLanguage(FEHolderBuildable.OnHandOverEmpty()));
            }
            else
            {
                main.SetInteractText(LanguageHelpers.GetLanguage(FEHolderBuildable.OnHandOverNotEmpty()), $"Fire Extinguisher: {_fuel}");
            }


            main.SetIcon(HandReticle.IconType.Hand, 1f);
        }

        public void OnHandClick(GUIHand hand)
        {
            if (_hasTank)
            {
                TakeTank();
            }
            else
            {
                TryStoreTank();
            }
        }

        private void TryStoreTank()
        {
            Pickupable pickupable = Inventory.main.container.RemoveItem(TechType.FireExtinguisher);

            if (pickupable != null)
            {
                FireExtinguisher component = pickupable.GetComponent<FireExtinguisher>();
                if (component != null)
                {
                    _fuel = component.fuel;
                }
                _hasTank = true;
                _tankMesh.SetActive(true);
            }
        }

        private void TakeTank()
        {
            GameObject gameObject = CraftData.AddToInventory(TechType.FireExtinguisher, 1, false, false);
            if (gameObject != null)
            {
                _hasTank = false;
                _tankMesh.SetActive(false);
                gameObject.GetComponent<FireExtinguisher>().fuel = _fuel;
            }
        }

        private bool FindComponents()
        {
            var tank = gameObject.FindChild("model").FindChild("fire_extinguisher_tube_01");

            if (tank == null)
            {
                QuickLogger.Error($"Cant find fire_extinguisher_tube_01 on the prefab");
                return false;
            }

            _tankMesh = tank;
            return true;
        }

        public bool CanDeconstruct(out string reason)
        {
            reason = String.Empty;
            return true;
        }

        public void OnConstructedChanged(bool constructed)
        {
            IsConstructed = constructed;

            if (!_initialized)
            {
                if (!FindComponents())
                {
                    _initialized = false;
                    return;
                }
                _tankMesh.SetActive(_hasTank);
                _initialized = true;
            }

            if (constructed)
            {

            }
        }

        internal void Save(FEHolderSaveData saveDataList)
        {
            var prefabIdentifier = GetComponent<PrefabIdentifier>();
            var id = prefabIdentifier.Id;

            if (_saveData == null)
            {
                _saveData = new FEHolderSaveDataEntry();
            }

            _saveData.ID = id;
            _saveData.Fuel = _fuel;
            _saveData.HasTank = _hasTank;
            saveDataList.Entries.Add(_saveData);
        }

        public void OnProtoSerialize(ProtobufSerializer serializer)
        {
            if (!Mod.IsSaving())
            {
                QuickLogger.Info("Saving FEExtinguishers");
                Mod.Save();
                QuickLogger.Info("Saved FEExtinguishers");
            }
        }

        public void OnProtoDeserialize(ProtobufSerializer serializer)
        {
            QuickLogger.Info("Loading FEExtinguishers");
            var prefabIdentifier = GetComponent<PrefabIdentifier>();
            var id = prefabIdentifier?.Id ?? string.Empty;
            var data = Mod.GetSaveData(id);

            if (data == null) return;

            _fuel = data.Fuel;
            _hasTank = data.HasTank;
            QuickLogger.Debug($"Loaded Fuel: {data.Fuel} || HasTank = {data.HasTank}");

            if (_tankMesh != null)
            {
                StartCoroutine(ShowTank(data));
            }
            else
            {
                QuickLogger.Error("Tank Mesh Not Found");
            }
            QuickLogger.Info("Loaded FEExtinguishers");
            QuickLogger.Debug($"Fuel: {_fuel} || HasTank = {_hasTank}");
        }

        private IEnumerator ShowTank(FEHolderSaveDataEntry data)
        {
            if (!data.HasTank) yield break;

            while (!_tankMesh.activeSelf)
            {
                QuickLogger.Debug("Trying to show Tank");
                _tankMesh.SetActive(true);
                yield return null;
            }
        }
    }
}

using Common.Helpers;
using Common.Utilities;
using MAC.FireExtinguisherHolder.Buildable;
using System;
using UnityEngine;


namespace MAC.FireExtinguisherHolder.Mono
{
    internal class FEHolderController : HandTarget, IHandTarget, IConstructable
    {
        private bool _holderEmpty = true;
        private GameObject _tankMesh;
        private bool _initalized;

        internal bool IsConstructed { get; set; }

        private void Start()
        {
            if (!FindComponents())
            {
                _initalized = false;
            }

            _initalized = true;
        }

        public void OnHandHover(GUIHand hand)
        {
            if (!_initalized || !IsConstructed) return;

            HandReticle main = HandReticle.main;

            if (_holderEmpty)
            {
                main.SetInteractText(LanguageHelpers.GetLanguage(FEHolderBuildable.OnHandOverEmpty()));
            }
            else
            {
                main.SetInteractText(LanguageHelpers.GetLanguage(FEHolderBuildable.OnHandOverNotEmpty()));
            }


            main.SetIcon(HandReticle.IconType.Hand, 1f);
        }

        public void OnHandClick(GUIHand hand)
        {
            if (!_initalized || !IsConstructed) return;

            if (Player.main == null || !IsConstructed) return;

            if (_holderEmpty)
            {
                QuickLogger.Debug($"Added Fire Extinguisher", true);
                _tankMesh.SetActive(true);
                _holderEmpty = false;
            }
            else
            {
                QuickLogger.Debug($"Removed Fire Extinguisher", true);
                _tankMesh.SetActive(false);
                _holderEmpty = true;

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

            if (constructed)
            {

            }
        }
    }
}

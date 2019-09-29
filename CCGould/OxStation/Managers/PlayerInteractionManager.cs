﻿using MAC.OxStation.Mono;

namespace MAC.OxStation.Managers
{
    internal class PlayerInteractionManager : HandTarget, IHandTarget
    {
        private OxStationController _mono;

        internal void Initialize(OxStationController mono)
        {
            _mono = mono;
        }
        public void OnHandHover(GUIHand hand)
        {
            if (_mono == null) return;
            HandReticle main = HandReticle.main;
            main.SetIcon(HandReticle.IconType.Default);
            main.SetInteractText($"Health: {_mono.HealthManager.GetHealth()}| Oxygen Level: {_mono.OxygenManager.GetO2Level()}", false, HandReticle.Hand.None);
        }

        public void OnHandClick(GUIHand hand)
        {
            //Not needed for this mod
        }
    }
}

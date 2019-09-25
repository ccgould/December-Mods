using Common.Helpers;
using Common.Utilities;
using MAC.FireExtinguisherHolder.Buildable;
using System;


namespace MAC.FireExtinguisherHolder.Mono
{
    internal class FEHolderController : HandTarget, IHandTarget, IConstructable
    {
        private bool _holderEmpty = true;

        internal bool IsConstructed { get; set; }

        public void OnHandHover(GUIHand hand)
        {
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
            if (Player.main == null || !IsConstructed) return;

            if (_holderEmpty)
            {
                QuickLogger.Debug($"Added Fire Extinguisher", true);
                _holderEmpty = false;
            }
            else
            {
                QuickLogger.Debug($"Removed Fire Extinguisher", true);
                _holderEmpty = true;

            }

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
                // Rotate
                //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + -90.0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }
        }
    }
}

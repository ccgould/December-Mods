﻿using Common.Utilities;
using UnityEngine;

namespace MAC.OxStation.Managers
{
    internal class AnimationManager : MonoBehaviour
    {
        #region Unity Methods   
        private void Start()
        {
            this.Animator = this.transform.GetComponentInChildren<Animator>();

            if (this.Animator == null)
            {
                QuickLogger.Error("Animator component not found on the GameObject.");
            }

            if (this.Animator != null && this.Animator.enabled == false)
            {
                QuickLogger.Debug("Animator was disabled and now has been enabled");
                this.Animator.enabled = true;
            }

            //_audioHandler = new AudioHandler(transform);
        }
        #endregion

        #region Public Methods
        public Animator Animator { get; set; }
        #endregion

        #region Internal Methods
        /// <summary>
        /// Sets the an animator float to a certain value (For use with setting the page on the screen)
        /// </summary>
        /// <param name="stateHash">The hash of the parameter</param>
        /// <param name="value">Float to set</param>
        internal void SetFloatHash(int stateHash, float value)
        {
            this.Animator.SetFloat(stateHash, value);
        }

        /// <summary>
        /// Sets the an animator boolean to a certain value
        /// </summary>
        /// <param name="stateHash">The hash of the parameter</param>
        /// <param name="value">Float to set</param>
        internal void SetBoolHash(int stateHash, bool value)
        {
            if (Animator == null)
            {
                this.Animator = this.transform.GetComponentInChildren<Animator>();
            }

            this.Animator.SetBool(stateHash, value);
        }

        /// <summary>
        /// Sets the an animator integer to a certain value
        /// </summary>
        /// <param name="stateHash">The hash of the parameter</param>
        /// <param name="value">Float to set</param>
        internal void SetIntHash(int stateHash, int value)
        {
            if (Animator == null)
            {
                this.Animator = this.transform.GetComponent<Animator>();
            }

            if (Animator == null) return;

            this.Animator.SetInteger(stateHash, value);
        }

        internal int GetIntHash(int hash)
        {
            if (Animator == null)
            {
                this.Animator = this.transform.GetComponent<Animator>();
            }

            if (Animator == null) return 0;

            return this.Animator.GetInteger(hash);
        }

        internal bool GetBoolHash(int hash)
        {
            if (Animator == null)
            {
                this.Animator = this.transform.GetComponent<Animator>();
            }

            if (Animator == null) return false;

            return this.Animator.GetBool(hash);
        }

        #endregion
    }
}
